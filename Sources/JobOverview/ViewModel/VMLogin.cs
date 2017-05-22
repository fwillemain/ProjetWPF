using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JobOverview.ViewModel
{
	public class VMLogin : ViewModelBase
	{
		public List<Employee> ListEmployee { get; private set; }
        public Employee LastEmployee { get; set; }
        public VMLogin()
		{
            ListEmployee = Model.DAL.GetListEmployeeWithoutTasks();
            if (ListEmployee.Select(c => c.Login).Contains(Properties.Settings.Default.EmployeeId))
                LastEmployee = ListEmployee.Where(c => c.Login == Properties.Settings.Default.EmployeeId).FirstOrDefault();
            else LastEmployee = ListEmployee[0];
        }
        public override ValidationResult Validate()
        {
            Properties.Settings.Default.EmployeeId = LastEmployee.Login;
            Properties.Settings.Default.Save();
            VMMain.CurrentEmployee = LastEmployee;
            return base.Validate();
        }
    }
}
