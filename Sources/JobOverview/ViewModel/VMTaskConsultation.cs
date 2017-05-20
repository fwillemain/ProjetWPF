using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{
    public class VMTaskConsultation : ViewModelBase
    {
        public Employee CurrentEmployee { get; set; }

        public VMTaskConsultation()
        {
            CurrentEmployee = DAL.GetEmployee("JROUSSET");
        }
    }
}
