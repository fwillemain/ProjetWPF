using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Entity
{
    public class Software
    {
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public List<Module> ListModule { get; set; }
        [XmlAttribute]
        public List<Version> ListVersion { get; set; }
    }
}
