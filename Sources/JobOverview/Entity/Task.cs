using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    [XmlInclude(typeof(TaskProd))]
    public class Task
    {
        [XmlAttribute]
        public Guid Id { get; set; }
        [XmlAttribute]
        public string Description { get; set; }
        [XmlAttribute]
        public string Label { get; set; }
        public Activity Activity { get; set; }
        public List<WorkTime> ListWorkTime { get; set; }
    }
}
