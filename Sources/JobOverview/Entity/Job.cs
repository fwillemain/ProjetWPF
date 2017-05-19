using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Entity
{
    public class Job
    {
        public string Code { get; set; }
        public string Label { get; set; }
        public List<Activity> ListActivity { get; set; }
    }
}
