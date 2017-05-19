using JobOverview.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Entity
{
    public class Employee
    {
        public string Login { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Job Job { get; set; }
        public Habilitation Habilitation { get; set; }
        public float Productivity { get; set; }
        public List<Task> ListTask { get; set; }
    }
}
