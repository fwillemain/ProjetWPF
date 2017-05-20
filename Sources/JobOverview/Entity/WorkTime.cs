using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class WorkTime
    {
        [XmlAttribute]
        public float Hours { get; set; }
        [XmlAttribute]
        public float Productivity { get; set; }
        [XmlAttribute]
        public DateTime WorkingDate { get; set; }
    }
}
