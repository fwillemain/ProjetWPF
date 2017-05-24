﻿using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace JobOverview.ViewModel
{
    /// <summary>
    /// Enumérable des différents modes d'édition
    /// </summary>
    public enum EditionModes { Consultation, Edition, Modification };
    public class VMTaskConsultation : ViewModelBase
    {

        #region Propriétés
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public Employee CurrentEmployee { get; set; }
        public List<TaskProd> CurrentEmployeeListTaskProd { get; set; }
        public List<Entity.Task> CurrentEmployeeListTaskAnx { get; set; }
        public List<Software> ListSoftware { get; set; }
        private List<Entity.Task> ListTaskToAddOrDelete { get; set; }
        private static Entity.Task _currentTask;
        private EditionModes _currentModeEdition;
        public EditionModes CurrentModeEdition
        {
            get { return _currentModeEdition; }
            set
            {
                SetProperty(ref _currentModeEdition, value);
            }
        }

        public static Entity.Task CurrentTask
        {
            get { return _currentTask ?? ViewModel.VMMain.CurrentEmployee.ListTask.FirstOrDefault(); }
            set { _currentTask = value;
                StaticPropertyChanged(CurrentTask, new PropertyChangedEventArgs("CurrentTask"));
            }
        }

        private static Entity.WorkTime _currentWorkTime;

        public static Entity.WorkTime CurrentWorkTime
        {
            get { return _currentWorkTime ?? ViewModel.VMMain.CurrentEmployee.ListTask.FirstOrDefault().ListWorkTime.FirstOrDefault(); }
            set
            {
                _currentWorkTime = value;
                StaticPropertyChanged(CurrentWorkTime, new PropertyChangedEventArgs("CurrentWorkTime"));
            }
        }



        #endregion

        #region Constructeurs
        public VMTaskConsultation(Employee currentEmployee)
        {
            //Rempli la liste de tâche de l'employé courant si elle n'a pas encore été rempli
            if (currentEmployee.ListTask == null)
                currentEmployee.ListTask = new System.Collections.ObjectModel.ObservableCollection<Entity.Task>(DAL.GetListTask(currentEmployee.Login));

            CurrentEmployee = currentEmployee;
            CurrentEmployeeListTaskProd = CurrentEmployee.ListTask.OfType<TaskProd>().ToList();
            CurrentEmployeeListTaskAnx = CurrentEmployee.ListTask.Where(t => t.Activity.IsAnnex).ToList();
            ListSoftware = DAL.GetListSoftware();
            CurrentModeEdition = EditionModes.Consultation;
            ListTaskToAddOrDelete = new List<Entity.Task>();
        }
        #endregion

        #region Commandes
        private ICommand _cmdSort;
        private ICommand _cmdAddWorkTime;
        private ICommand _cmdModifyWorkTime;
        private ICommand _cmdDeleteWorkTime;
        private ICommand _cmdValidateWorkTime;
        private ICommand _cmdCancelWorkTime;
        private ICommand _cmdSaveModification;
        public ICommand CmdSort
        {
            get
            {
                if (_cmdSort == null)
                    _cmdSort = new RelayCommand(SortAction);

                return _cmdSort;
            }
        }

        private void SortAction()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(CurrentEmployeeListTaskProd);

        }

        public ICommand CmdAddWorkTime
        {
            get
            {
                if (_cmdAddWorkTime == null)
                    _cmdAddWorkTime = new RelayCommand(AddWorkTime, ActivateAddModifyDelete);

                return _cmdAddWorkTime;
            }
        }

        private void AddWorkTime()
        {
            CurrentWorkTime = new WorkTime { WorkingDate = DateTime.Today, Productivity=CurrentEmployee.Productivity };
            CurrentModeEdition = EditionModes.Edition;

        }

        public ICommand CmdModifyWorkTime
        {
            get
            {
                if (_cmdModifyWorkTime == null)
                    _cmdModifyWorkTime = new RelayCommand(ModifyWorkTime, ActivateAddModifyDelete);

                return _cmdModifyWorkTime;
            }
        }

        private void ModifyWorkTime()
        {
            CurrentModeEdition = EditionModes.Modification;
        }

        public ICommand CmdDeleteWorkTime
        {
            get
            {
                if (_cmdDeleteWorkTime == null)
                    _cmdDeleteWorkTime = new RelayCommand(DeleteWorkTime, ActivateAddModifyDelete);

                return _cmdDeleteWorkTime;
            }
        }

        private void DeleteWorkTime()
        {
            //Ajout du temps de travail à supprimer à la liste listTask avec une 
            //productivité de -1 pour la différencier des tâches à modifier
            var listWorkTime = new System.Collections.ObjectModel.ObservableCollection<WorkTime>();
            CurrentWorkTime.Productivity = -1;
            listWorkTime.Add(CurrentWorkTime);
            ListTaskToAddOrDelete.Add(new Entity.Task { Id = CurrentTask.Id, ListWorkTime = listWorkTime });
            CurrentTask.ListWorkTime.Remove(CurrentWorkTime);
        }

        public ICommand CmdValidateWorkTime
        {
            get
            {
                if (_cmdValidateWorkTime == null)
                    _cmdValidateWorkTime = new RelayCommand(ValidateWorkTime, ActivateValidate);

                return _cmdValidateWorkTime;
            }
        }

        private void ValidateWorkTime()
        {
            //Recherche tous les temps de travail pour la date du temps de travail courant
            var listWorktime = new List<WorkTime>();
            foreach (var item in CurrentEmployee.ListTask)
            {
                foreach (var ite in item.ListWorkTime)
                {
                    if (ite.WorkingDate.ToShortDateString() == VMTaskConsultation.CurrentWorkTime.WorkingDate.ToShortDateString())
                        listWorktime.Add(ite);
                }
            }

            // En mode édition vérifie si la date existe pour la tâche courante et si le total d'heure pour cette date ne dépasse pas 8h
            if (CurrentModeEdition == EditionModes.Edition)
            {
                if (!(CurrentTask.ListWorkTime.Select(c => c.WorkingDate.ToShortDateString()).Contains(CurrentWorkTime.WorkingDate.ToShortDateString())) &&
                     listWorktime.Sum(c => c.Hours) + CurrentWorkTime.Hours <= 8)
                {
                    CurrentWorkTime.Productivity = VMMain.CurrentEmployee.Productivity;
                    CurrentTask.ListWorkTime.Add(CurrentWorkTime);

                    var listTask = new System.Collections.ObjectModel.ObservableCollection<WorkTime>();
                    listTask.Add(CurrentWorkTime);
                    ListTaskToAddOrDelete.Add(new Entity.Task { Id = CurrentTask.Id, ListWorkTime = listTask });
                    CurrentModeEdition = EditionModes.Consultation;
                }
                else
                    MessageBox.Show("La date existe déjà ou le nombre d'heures dépasse 8.");
            }
            // En mode modification vérifie si le total d'heure pour cette date ne dépasse pas 8h
            else
            {
                if (listWorktime.Sum(c => c.Hours) <= 8)
                {
                    var listTask = new System.Collections.ObjectModel.ObservableCollection<WorkTime>();
                    listTask.Add(CurrentWorkTime);
                    ListTaskToAddOrDelete.Add(new Entity.Task { Id = CurrentTask.Id, ListWorkTime = listTask });
                    CurrentModeEdition = EditionModes.Consultation;
                }
                else
                    MessageBox.Show("La date existe déjà ou le nombre d'heures dépasse 8.");
            }
           
        }

        public ICommand CmdCancelWorkTime
        {
            get
            {
                if (_cmdCancelWorkTime == null)
                    _cmdCancelWorkTime = new RelayCommand(CancelWorkTime, ActivateCancel);

                return _cmdCancelWorkTime;
            }
        }

        private void CancelWorkTime()
        {
            CurrentWorkTime = CurrentTask.ListWorkTime.FirstOrDefault();
            CurrentModeEdition = EditionModes.Consultation;
        }

        public ICommand CmdSaveModification
        {
            get
            {
                if (_cmdSaveModification == null)
                    _cmdSaveModification = new RelayCommand(SaveModification);

                return _cmdSaveModification;
            }
        }

        private void SaveModification()
        {
            DAL.UpdateDatabaseWorkTimeOfTaskList(ListTaskToAddOrDelete);
            ListTaskToAddOrDelete = new List<Entity.Task>();
        }

        //Les 3 méthodes suivantes permettent d'activé ou de désactivé les commandes en fonction du mode d'édition courant
        private bool ActivateAddModifyDelete()
        {
            return CurrentModeEdition == EditionModes.Consultation;
        }

        private bool ActivateCancel()
        {
            return CurrentModeEdition == EditionModes.Edition;
        }
        private bool ActivateValidate()
        {
            return CurrentModeEdition == EditionModes.Edition || CurrentModeEdition == EditionModes.Modification;
        }
        #endregion

    }
}
