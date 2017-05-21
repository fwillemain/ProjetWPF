using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{
    public class VMTaskConsultation : ViewModelBase
    {
        public Employee CurrentEmployee { get; set; }
        public List<TaskProd> CurrentEmployeeListTaskProd { get; set; }
        public List<Entity.Task> CurrentEmployeeListTaskAnx { get; set; }

        public VMTaskConsultation()
        {
            CurrentEmployee = DAL.GetEmployee("RBEAUMONT");
            CurrentEmployeeListTaskProd = CurrentEmployee.ListTask.OfType<TaskProd>().ToList();
            CurrentEmployeeListTaskAnx = CurrentEmployee.ListTask.Where(t => t.Activity.IsAnnex).ToList();
            // TODO : débuger la méthode getEmployee 
        }
    }
}
