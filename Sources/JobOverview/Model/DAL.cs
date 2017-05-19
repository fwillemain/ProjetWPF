using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Model
{
    static public class DAL
    {
        #region Méthodes privées
        private static void DataReaderToListEmployee(SqlDataReader reader, List<Employee> listEmployee)
        {
            while (reader.Read())
            {
                if (!listEmployee.Any())
                {
                    Employee e = new Employee()
                    {
                        Login = (string)reader["Login"],
                        LastName = (string)reader["Nom"],
                        FirstName
                    }
                }


            }
        }
        #endregion

        #region Méthodes publiques
        static public List<Employee> GetListEmployee()
        {
            List<Employee> listEmployee = new List<Employee>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
            {
                string query = @"select p.Login, p.Nom, p.Prenom, p.Manager, p.TauxProductivite,
	                                            m.CodeMetier, m.Libelle,
	                                            act1.CodeActivite CodeActiviteMetier, act1.Libelle LibelleActiviteMetier, act1.Annexe AnnexeActiviteMetier,
	                                            t.IdTache, t.Description, t.Libelle, 
	                                            act2.CodeActivite CodeActiviteTache, act2.Libelle LibelleActiviteTache, act2.Annexe AnnexeActiviteTache,
	                                            tr.Heures, tr.TauxProductivite, tr.DateTravail,
	                                            tp.Numero, tp.DureeRestanteEstimee, tp.DureePrevue, tp.CodeModule, tp.CodeLogicielVersion
                                from jo.Personne p 
                                inner join jo.Metier m on p.CodeMetier = m.CodeMetier
                                inner join jo.ActiviteMetier am on m.CodeMetier = am.MetierCodeMetier
                                inner join jo.Activite act1 on am.ActiviteCodeActivite = act1.CodeActivite
                                inner join jo.Tache t on p.Login = t.Login
                                inner join jo.Activite act2 on t.CodeActivite = act2.CodeActivite
                                inner join jo.Travail tr on t.IdTache = tr.IdTache 
                                left outer join jo.TacheProd tp on t.IdTache = tp.IdTache";

                SqlCommand cmd = new SqlCommand(query, cnx);
                cnx.Open();

                using(var reader = cmd.ExecuteReader())
                {
                    DataReaderToListEmployee(reader, listEmployee);
                }
            }

            return listEmployee;
        }

        #endregion
    }
}
