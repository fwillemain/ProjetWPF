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
        public Employee CurrentEmployee { get; set; }
        public Employee LastEmployee { get; set; }
        public VMLogin()
		{
            // TODO VMLogin: utiliser un appel à une méthode de DAL
            //ListPeople = new List<Employee>();
            //ListPeople.Add(new Employee { Login = "JP", LastName = "Jean-Paul" });
            //ListPeople.Add(new Employee { Login = "BD", LastName = "Bidule" });
            //if (ListPeople.Select(c => c.Login).Contains(Properties.Settings.Default.EmployeId))
            //    LastEmploye = ListPeople.Where(c => c.Login == Properties.Settings.Default.EmployeId).FirstOrDefault();
            //else LastEmploye = ListPeople[0];
        }
        public override ValidationResult Validate()
        {
            Properties.Settings.Default.EmployeId = LastEmployee.Login;
            Properties.Settings.Default.Save();


            return base.Validate();
        }
    }
}
