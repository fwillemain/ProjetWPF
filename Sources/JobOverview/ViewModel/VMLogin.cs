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
		public List<Employee> ListPeople { get; private set; }
        public Employee LastEmployee { get; set; }
        public VMLogin()
		{
            // TODO VMLogin: utiliser un appel à une méthode de DAL
            ListPeople = new List<Employee>();
            ListPeople.Add(new Employee { Login = "JP", LastName = "Jean-Paul", Habilitation=Habilitation.Employee});
            ListPeople.Add(new Employee { Login = "BD", LastName = "Bidule" });
            if (ListPeople.Select(c => c.Login).Contains(Properties.Settings.Default.EmployeeId))
                LastEmployee = ListPeople.Where(c => c.Login == Properties.Settings.Default.EmployeeId).FirstOrDefault();
            else LastEmployee = ListPeople[0];
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
