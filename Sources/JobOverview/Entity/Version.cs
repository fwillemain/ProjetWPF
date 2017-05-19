using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Entity
{
    public class Version
    {
        public float Number { get; set; }
        public short Year { get; set; }
        public DateTime ActualReleaseDate { get; set; }
        public DateTime EstimatedReleaseDate { get; set; }
        public DateTime SettingDate { get; set; }
        public int NumberOfReleases { get; set; }
    }
}
