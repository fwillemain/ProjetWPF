using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class TaskProd : Task
    {
        [XmlAttribute]
        public int Number { get; set; }
        [XmlAttribute]
        public float EstimatedRemainingTime { get; set; }
        [XmlAttribute]
        public float PredictedTime { get; set; }
        [XmlAttribute]
        public Module Module { get; set; }
        [XmlAttribute]
        public Version Version { get; set; }
    }
}
