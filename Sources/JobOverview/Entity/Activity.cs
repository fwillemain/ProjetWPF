﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class Activity
    {
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string Label { get; set; }
        [XmlAttribute]
        public bool IsAnnex { get; set; }
    }
}
