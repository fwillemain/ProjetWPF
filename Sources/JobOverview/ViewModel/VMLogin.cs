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
		public List<Employe> ListPeople { get; private set; }
        public Employe LastEmploye { get; set; }

        public VMLogin(Habilitation habilitation)
		{
            // TODO : à remplacer par un appel à une méthode de DAL
            ListPeople = new List<Employe>();
            ListPeople.Add(new Employe { Id = 1, LastName = "Jean-Paul" });
            ListPeople.Add(new Employe { Id = 2, LastName = "Bidule" });
            if (ListPeople.Select(c => c.Id).Contains(Properties.Settings.Default.EmployeId))
                LastEmploye = ListPeople.Where(c => c.Id == Properties.Settings.Default.EmployeId).FirstOrDefault();
            else LastEmploye = ListPeople[0];
        }
        public override ValidationResult Validate()
        {
            Properties.Settings.Default.EmployeId = LastEmploye.Id;
            Properties.Settings.Default.Save();
            
            return base.Validate();
        }
    }
}
