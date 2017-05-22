using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class Employee
    {
        [XmlAttribute]
        public string Login { get; set; }
        [XmlAttribute]
        public string LastName { get; set; }
        [XmlAttribute]
        public string FirstName { get; set; }
        [XmlAttribute]
        public Job Job { get; set; }
        [XmlAttribute]
        public Habilitation Habilitation { get; set; }
        [XmlAttribute]
        public float Productivity { get; set; }
        [XmlAttribute]
        public List<Task> ListTask { get; set; }
    }
    public enum Habilitation
    {
        Manager, Employee
    }
}
