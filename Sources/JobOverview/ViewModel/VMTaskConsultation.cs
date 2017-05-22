using JobOverview.Entity;
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
    public class VMTaskConsultation : ViewModelBase
    {
        #region Propriétés
        public Employee CurrentEmployee { get; set; }
        public List<TaskProd> CurrentEmployeeListTaskProd { get; set; }
        public List<Entity.Task> CurrentEmployeeListTaskAnx { get; set; }
        public List<Software> ListSoftware { get; set; }

        #endregion

        #region Constructeurs
        public VMTaskConsultation(Employee currentEmployee)
        {
            // TODO VMTaskConsultation::VMTaskConsultation() : voir pour faire un stockage des listes et de l'employé plus propre si nécessaire
            // TODO VMTaskConsultation::VMTaskConsultation() : faire un groupement par version?
            if (currentEmployee.ListTask==null)
                currentEmployee.ListTask = DAL.GetListTask(currentEmployee.Login);

            CurrentEmployee = currentEmployee;
            CurrentEmployeeListTaskProd = CurrentEmployee.ListTask.OfType<TaskProd>().ToList();
            CurrentEmployeeListTaskAnx = CurrentEmployee.ListTask.Where(t => t.Activity.IsAnnex).ToList();
            ListSoftware = DAL.GetListSoftware();
        }
        #endregion

        #region Commandes
        private ICommand _cmdSort;
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

        #endregion

    }
}
