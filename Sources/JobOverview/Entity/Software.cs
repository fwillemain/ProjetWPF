﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class Software
    {
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlElement]
        public List<Module> ListModule { get; set; }
        [XmlElement]
        public List<Version> ListVersion { get; set; }
    }
}
