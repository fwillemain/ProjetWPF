using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{
    public class VMConnection : ViewModelBase
    {
        public List<object> ListConnectionString { get; set; }
        public string SelectedConnectionString { get; set; }
        public VMConnection()
        {
            ListConnectionString = new List<object>();

            // Affiche toutes les chaines de connexion au format "JobOverviewConnectionStringXXX" dans la cb
            foreach (SettingsProperty p in Properties.Settings.Default.Properties)
            {
                if (p.Name.Contains("JobOverviewConnectionString"))
                    ListConnectionString.Add(new { Nom = p.Name, Valeur = p.DefaultValue });
            }
        }

        public override ValidationResult Validate()
        {
            // Affecte à la propriété JobOverviewConnectionStringDefault la chaine de connexion sélectionnée
            if (SelectedConnectionString != null)
            {
                Properties.Settings.Default.JobOverviewConnectionStringDefault = SelectedConnectionString;
                Properties.Settings.Default.Save();
            }

            return base.Validate();
        }
    }
}
