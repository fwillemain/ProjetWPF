using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Entity
{
    public class TaskProd : Task
    {
        public int Number { get; set; }
        public float EstimatedRemainingTime { get; set; }
        public float PredictedTime { get; set; }
        public Module Module { get; set; }
        public Version Version { get; set; }
    }
}
