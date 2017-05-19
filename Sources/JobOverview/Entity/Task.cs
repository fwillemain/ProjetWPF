using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Entity
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public Activity Activity { get; set; }
        public List<WorkTime> ListWorkTime { get; set; }
    }
}
