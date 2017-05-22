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

namespace JobOverview.ViewModel
{
    public class VMTaskManaging : ViewModelBase
    {
        #region Champs privé et propriétés
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get
            { return _selectedEmployee; }
            set
            {
                if (_listEmployee.Where(e => e.Login == value.Login).FirstOrDefault().ListTask == null)
                {
                    _listEmployee.Where(e => e.Login == value.Login).FirstOrDefault().ListTask = DAL.GetListTask(_selectedEmployee.Login);
                }
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
        #endregion

        public VMTaskManaging(List<Employee> listEmployee)
        {
            ListEmployee = (ObservableCollection<Employee>)listEmployee.Where(e => e.CodeTeam == VMMain.CurrentEmployee.CodeTeam);
        }
    }
}
