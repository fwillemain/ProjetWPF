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
        #region Champs privé et propriétés
        public Software SelectedSoftware { get; set; }
        public Entity.Version SelectedVersion { get; set; }
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get
            { return _selectedEmployee; }
            set
            {
                if (_listEmployee.Where(e => e.Login == value.Login).FirstOrDefault().ListTask == null)
                {
                    _listEmployee.Where(e => e.Login == value.Login).FirstOrDefault().ListTask = DAL.GetListTask(value.Login);
                }
                _selectedEmployee = _listEmployee.Where(e => e.Login == value.Login).FirstOrDefault();
                if (_selectedEmployee != null)
                {
                    ListTaskAnnex = new ObservableCollection<Entity.Task>(_selectedEmployee.ListTask.Where(t => t.Activity.IsAnnex));
                    ListTaskProd = new ObservableCollection<TaskProd>(_selectedEmployee.ListTask.OfType<TaskProd>()); 
                }
                FilterSoftwareVersion();
                SetProperty(ref _selectedEmployee, value);
            }
        }
        private ObservableCollection<Employee> _listEmployee;
        public ObservableCollection<Employee> ListEmployee
        {
            get
            { return _listEmployee; }
            set { SetProperty(ref _listEmployee, value); }
        }
        private ObservableCollection<TaskProd> _listTaskProd;
        public ObservableCollection<TaskProd> ListTaskProd
        {
            get
            { return _listTaskProd; }
            set { SetProperty(ref _listTaskProd, value); }
        }
        private ObservableCollection<Entity.Task> _listTaskAnnex;
        public ObservableCollection<Entity.Task> ListTaskAnnex
        {
            get
            { return _listTaskAnnex; }
            set { SetProperty(ref _listTaskAnnex, value); }
        }
        public List<Software> ListSoftware { get; set; }
        #endregion

        public VMTaskManaging(List<Employee> listEmployee)
        {
            ListSoftware = DAL.GetListSoftware();
            ListEmployee = new ObservableCollection<Employee>(VMMain.ListEmployee.Where(e => e.CodeTeam == VMMain.CurrentEmployee.CodeTeam));
            SelectedEmployee = ListEmployee.FirstOrDefault();
        }

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

        private void FilterSoftwareVersion()
        {
            if (SelectedSoftware != null && SelectedVersion != null)
                ListTaskProd = new ObservableCollection<TaskProd>(SelectedEmployee.ListTask.OfType<TaskProd>().Where(t => t.Version.Number == SelectedVersion.Number && t.Software.Code == SelectedSoftware.Code).ToList());
        }
    }
}
