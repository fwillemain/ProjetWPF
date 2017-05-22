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
        public Job Job { get; set; }
        public Habilitation Habilitation { get; set; }
        [XmlAttribute]
        public float Productivity { get; set; }
        [XmlAttribute]
        public string CodeTeam { get; set; }
        public List<Task> ListTask { get; set; }
        [XmlAttribute]
        public float RemainingTimeReport
        {
            get
            {
                return ListTask.OfType<TaskProd>().Sum(t => t.EstimatedRemainingTime);
            }
        }
        [XmlAttribute]
        public float SpentTimeReport
        {
            get
            {
                return ListTask.OfType<TaskProd>().Where(t => t.EstimatedRemainingTime != 0).Sum(w => w.ListWorkTime.Sum(wt => wt.Hours));
            }
        }
    }
    public enum Habilitation
    {
        Manager, Employee
    }
}
