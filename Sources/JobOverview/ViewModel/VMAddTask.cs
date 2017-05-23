using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{
    public class VMAddTask :ViewModelBase
    {
        public Employee CurrentEmployee { get; set; }
        public VMAddTask(Employee selectedEmployee)
        {
            CurrentEmployee = selectedEmployee;
        }
    }
}
