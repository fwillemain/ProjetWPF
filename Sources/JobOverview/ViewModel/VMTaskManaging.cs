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

namespace JobOverview.ViewModel
{
    public class VMTaskManaging : ViewModelBase
    {
        private ObservableCollection<Employee> _listEmployee;
        public ObservableCollection<Employee> ListEmployee
        { get
            { return _listEmployee; }
            set { SetProperty(ref _listEmployee, value); } }
        public VMTaskManaging(List<Employee> listEmployee)
        {
            ListEmployee = listEmployee.Where( e => e.CodeTeam == VMMain.CurrentEmployee.CodeTeam);
        }
    }
}
