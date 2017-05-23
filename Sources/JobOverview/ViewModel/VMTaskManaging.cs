using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace JobOverview.ViewModel
{
    public class VMTaskManaging : ViewModelBase
    {
        #region Champs privé
        private Software _selectedSoftware;
        private Entity.Version _selectedVersion;
        private ObservableCollection<TaskProd> _listTaskProd;
        private ObservableCollection<Entity.Task> _listTaskAnnex;
        private float _remainingTimeReport;
        private float _spentTimeReport;
        private ObservableCollection<Employee> _listEmployee;
        private Employee _selectedEmployee;
        #endregion

        #region Propriétés publiques
        public List<Software> ListSoftware { get; set; }
        public Software SelectedSoftware
        {
            get
            { return _selectedSoftware != null ? _selectedSoftware : ListSoftware.FirstOrDefault(); }
            set
            { SetProperty(ref _selectedSoftware, value); }
        }
        public Entity.Version SelectedVersion
        {
            get
            { return _selectedVersion != null ? _selectedVersion : ListSoftware.Where(s => s.Code == SelectedSoftware.Code).FirstOrDefault().ListVersion.FirstOrDefault(); }
            set
            { SetProperty(ref _selectedVersion, value); }
        }
        public bool FinishedTaskVisible { get; set; }
        public bool RemainingTaskVisible { get; set; }
        public ObservableCollection<TaskProd> ListTaskProd
        {
            get
            { return _listTaskProd; }
            set { SetProperty(ref _listTaskProd, value); }
        }
        public ObservableCollection<Entity.Task> ListTaskAnnex
        {
            get
            { return _listTaskAnnex; }
            set { SetProperty(ref _listTaskAnnex, value); }
        }
        public Entity.Task CurrentTask { get; set; }
        public float RemainingTimeReport
        {
            get
            { return _remainingTimeReport; }
            set { SetProperty(ref _remainingTimeReport, value); }
        }
        public float SpentTimeReport
        {
            get
            { return _spentTimeReport; }
            set { SetProperty(ref _spentTimeReport, value); }
        }
        public ObservableCollection<Employee> ListEmployee
        {
            get
            { return _listEmployee; }
            set { SetProperty(ref _listEmployee, value); }
        }
        public List<Guid> ListSuppTasks { get; set; }
        public List<Employee> ListEmployeeWithAddedTasks { get; set; }
        public Employee SelectedEmployee
        {
            get
            { return _selectedEmployee; }
            set
            {
                // si l'employé séléctionné a une liste de tache nulle alors on va la récupérer avec une méthade de DAL.
                if (_listEmployee.Where(e => e.Login == value.Login).FirstOrDefault().ListTask == null)
                {
                    _listEmployee.Where(e => e.Login == value.Login).FirstOrDefault().ListTask = new ObservableCollection<Entity.Task>(DAL.GetListTask(value.Login));
                }
                // Définition de l'employé séléctionné sur SelectedEmployee
                _selectedEmployee = _listEmployee.Where(e => e.Login == value.Login).FirstOrDefault();
                if (_selectedEmployee != null)
                {
                    // Remplissage des liste de tâche de production et annexe pour l'employé séléctionné.
                    ListTaskAnnex = new ObservableCollection<Entity.Task>(_selectedEmployee.ListTask.Where(t => t.Activity.IsAnnex));
                    ListTaskProd = new ObservableCollection<TaskProd>(_selectedEmployee.ListTask.OfType<TaskProd>());
                    if (ListTaskProd != null && ListTaskProd.Select(w => w.ListWorkTime).Any())
                    {
                        // on récupère le temps passé total et restant
                        SpentTimeReport = ListTaskProd.Where(t => t.Version.Number == SelectedVersion.Number && t.Software.Code == SelectedSoftware.Code).Sum(w => w.ListWorkTime.Sum(wt => wt.Hours));
                    }
                    else
                        SpentTimeReport = 0;
                    RemainingTimeReport = ListTaskProd != null ? ListTaskProd.Where(t => t.Version.Number == SelectedVersion.Number && t.Software.Code == SelectedSoftware.Code).Sum(t => t.EstimatedRemainingTime) : 0;
                }
                FilterSoftwareVersion();
                SetProperty(ref _selectedEmployee, value);
            }
        }
        public Employee UpdatedEmployee { get; set; }
        #endregion

        public VMTaskManaging(List<Employee> listEmployee)
        {
            ListSoftware = DAL.GetListSoftware();
            ListEmployee = new ObservableCollection<Employee>(VMMain.ListEmployee.Where(e => e.CodeTeam == VMMain.CurrentEmployee.CodeTeam));
            SelectedEmployee = ListEmployee.FirstOrDefault();
            // Création d'une copie de la liste des employé
            Employee[] empArray = new Employee[ListEmployee.Count];
            ListEmployee.CopyTo(empArray, 0);
            ListEmployeeWithAddedTasks = new List<Employee>(empArray);
            foreach (var item in ListEmployeeWithAddedTasks)
                item.ListTask = new ObservableCollection<Entity.Task>();
            ListSuppTasks = new List<Guid>();
            RemainingTaskVisible = true;
        }

        #region Commandes
        private ICommand _cmdFilter;
        public ICommand CmdFilter
        {
            get
            {
                if (_cmdFilter == null)
                    _cmdFilter = new RelayCommand(FilterSoftwareVersion);
                return _cmdFilter;
            }
        }
        private ICommand _cmdAddTask;
        public ICommand CmdAddTask
        {
            get
            {
                if (_cmdAddTask == null)
                    _cmdAddTask = new RelayCommand(AddTask);
                return _cmdAddTask;
            }
        }

        private ICommand _cmdSuppTask;
        public ICommand CmdSuppTask
        {
            get
            {
                if (_cmdSuppTask == null)
                    _cmdSuppTask = new RelayCommand(SuppTask);
                return _cmdSuppTask;
            }
        }

        private ICommand _cmdSave;
        public ICommand CmdSave
        { get
            {
                if (_cmdSave == null)
                    _cmdSave = new RelayCommand(Save);
                return _cmdSave;
            }
        }
        #endregion

        #region Méthodes privées
        /// <summary>
        /// Filtre la liste de tâche de production par rapport au logiciel et version séléctionnés et en fonction du temps restant
        /// à réalisé sur les tâches.
        /// </summary>
        private void FilterSoftwareVersion()
        {
            if (SelectedSoftware != null && SelectedVersion != null)
            {
                ListTaskProd = new ObservableCollection<TaskProd>(SelectedEmployee.ListTask.OfType<TaskProd>()
                    .Where(t => t.Software.Code == SelectedSoftware.Code &&
                     t.Version.Number == SelectedVersion.Number &&
                    (   // si les 2 check box sont cochées tout est affiché
                        (RemainingTaskVisible && FinishedTaskVisible) ||
                        // Sinon si tâche courante est cochée les taches courantes sont affichées (il reste du temps à passer dessus)
                        ((t.EstimatedRemainingTime != 0) == RemainingTaskVisible &&
                        // et si tâche terminé est cochée les tâches terminées sont affichées (il n'y a plus de temps à passer dessus)
                        (t.EstimatedRemainingTime == 0) == FinishedTaskVisible))
                    ).ToList());
                SpentTimeReport = ListTaskProd.Where(t => t.Version.Number == SelectedVersion.Number && t.Software.Code == SelectedSoftware.Code).Sum(w => w.ListWorkTime.Sum(wt => wt.Hours));
                RemainingTimeReport = ListTaskProd != null ? ListTaskProd.Where(t => t.Version.Number == SelectedVersion.Number && t.Software.Code == SelectedSoftware.Code).Sum(t => t.EstimatedRemainingTime) : 0;
            }
        }
        /// <summary>
        /// Ajoute les tâches créée dans la fenêtre de création de tâche dans une liste de tâche à créer qui seront ajouter lors de l'appel de la méthode Save.
        /// </summary>
        private void AddTask()
        {
            UpdatedEmployee = new Employee() {Login = SelectedEmployee.Login, ListTask = new ObservableCollection<Entity.Task>(), Job = SelectedEmployee.Job };
            var inputBox = new View.AddTaskWindow( new VMAddTask(UpdatedEmployee));
            inputBox.ShowDialog();
            if (ListEmployeeWithAddedTasks.Where(e => e.Login == UpdatedEmployee.Login).Count() == 0)
            {
                ListEmployeeWithAddedTasks.Add(UpdatedEmployee); 
            }
            else
            {
                foreach (var task in UpdatedEmployee.ListTask)
                {
                    ListEmployeeWithAddedTasks.Where(e => e.Login == UpdatedEmployee.Login).FirstOrDefault().ListTask.Add(task);
                    if (task is TaskProd)
                        ListTaskProd.Add((TaskProd)task);
                    else
                        ListTaskAnnex.Add(task);
                }
            }
        }
        /// <summary>
        /// Ajoute la tâche marquée à supprimer dans une liste de tâche qui seront supprimer lors de l'appel de la méthode Save.
        /// </summary>
        private void SuppTask()
        {
            if (CurrentTask != null && CurrentTask.TotalWorkingTime == 0)
            {
                if (ListEmployeeWithAddedTasks.Where(e => e.Login == SelectedEmployee.Login).FirstOrDefault().ListTask.Where(t => t.Id == CurrentTask.Id).Any())
                {
                    // Si la tâche à supprimé était déjà présente dans la liste (elle a été ajouté pendant la session courrante) alors on la supprime de la liste des tâches modifiées.
                    ListEmployeeWithAddedTasks.Where(e => e.Login == SelectedEmployee.Login).FirstOrDefault().
                ListTask.Remove(CurrentTask);
                }
                else
                {
                    ListSuppTasks.Add(CurrentTask.Id);
                }
                ListEmployee.Where(e => e.Login == SelectedEmployee.Login).FirstOrDefault().
            ListTask.Remove(CurrentTask);
                if (CurrentTask is TaskProd)
                    ListTaskProd.Remove((TaskProd)CurrentTask);
                else
                    ListTaskAnnex.Remove(CurrentTask);
            }
        }
        /// <summary>
        /// Sauvegarde les ajout et suppression de tâche dans la base de donnée.
        /// </summary>
        private void Save()
        {
            //DAL.UpdateDatabaseTaskListOfEmployee(ListEmployeeWithAddedTasks, ListSuppTasks); 
        }
        #endregion
    }
}
