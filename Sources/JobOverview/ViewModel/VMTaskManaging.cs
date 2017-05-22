using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{
    public class VMTaskManaging : ViewModelBase
    {
        public List<Employee> ListEmployee { get; set; }
        public VMTaskManaging(List<Employee> listEmployee)
        {
            ListEmployee = listEmployee;
            //ListEmployee = DAL.GetListEmployeeWithoutTask(Properties.Settings.Default.EmployeeId);
            //ListEmployee.Add();
        }
    }
}
