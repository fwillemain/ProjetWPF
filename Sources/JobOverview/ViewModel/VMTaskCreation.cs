using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{
    public class VMTaskCreation : ViewModelBase
    {
        public List<Employee> ListEmployee { get; set; }
        public VMTaskCreation()
        {
            //ListEmployee = DAL.GetListEmployee(Properties.Settings.Default.EmployeId);
        }
    }
}
