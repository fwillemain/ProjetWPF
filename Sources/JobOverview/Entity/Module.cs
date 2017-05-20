using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class Module
    {
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string Label { get; set; }
    }
}
