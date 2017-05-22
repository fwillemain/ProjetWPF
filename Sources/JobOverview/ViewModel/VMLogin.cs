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
		// TODO : à remplacer par une vraie liste de personnes
		public List<Employee> ListPeople { get; private set; }
        public Employee CurrentEmployee { get; set; }
        public Employee LastEmployee { get; set; }
        public VMLogin()
		{
           // CurrentEmployee = currentEmployee;
            //// TODO : à remplacer par un appel à une méthode de DAL
            ListPeople = new List<Employee>();
            ListPeople.Add(new Employee { Login = "JP", LastName = "Jean-Paul" });
            ListPeople.Add(new Employee { Login = "BD", LastName = "Bidule" });
            if (ListPeople.Select(c => c.Login).Contains(Properties.Settings.Default.EmployeId))
                LastEmployee = ListPeople.Where(c => c.Login == Properties.Settings.Default.EmployeId).FirstOrDefault();           
            else LastEmployee = ListPeople[0];
        }
        public override ValidationResult Validate()
        {
            Properties.Settings.Default.EmployeId = LastEmployee.Login;
            Properties.Settings.Default.Save();


            return base.Validate();
        }
    }
}
