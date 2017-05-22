using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{
    public class VMTaskManaging : ViewModelBase
    {
        public List<Employee> ListEmployee { get; set; }
        public VMTaskManaging()
        {
            ListEmployee = DAL.GetListEmployeeWithoutTask(Properties.Settings.Default.EmployeeId);
            ListEmployee.Add(VMMain.CurrentEmployee);
        }

        #region Gestion de MAJ de l'affichage après MAJ des données
        private void RaisePropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
