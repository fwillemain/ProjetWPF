using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class Version
    {
        [XmlAttribute]
        public float Number { get; set; }
        [XmlAttribute]
        public short Year { get; set; }
        [XmlAttribute]
        public DateTime? ActualReleaseDate { get; set; }
        [XmlAttribute]
        public DateTime EstimatedReleaseDate { get; set; }
        [XmlAttribute]
        public DateTime SettingDate { get; set; }
        [XmlAttribute]
        public int NumberOfReleases { get; set; }
    }
}
