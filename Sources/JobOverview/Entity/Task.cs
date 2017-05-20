using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class Task
    {
        [XmlAttribute]
        public Guid Id { get; set; }
        [XmlAttribute]
        public string Description { get; set; }
        [XmlAttribute]
        public string Label { get; set; }
        [XmlAttribute]
        public Activity Activity { get; set; }
        [XmlAttribute]
        public List<WorkTime> ListWorkTime { get; set; }
    }
}
