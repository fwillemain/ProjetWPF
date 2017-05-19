using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.ViewModel;
using System.Data;

namespace JobOverview.Model
{
    static public class DAL
    {
        #region Méthodes privées
        private static void DataReaderToListEmployee(SqlDataReader reader, List<Employee> listEmployee)
        {
            while (reader.Read())
            {
                // Si la liste d'employé est vide ou si l'employé change ajouter un nouvel employé
                if (!listEmployee.Any() || listEmployee.Last().Login != (string)reader["Login"])
                {
                    Employee employee = new Employee()
                    {
                        Login = (string)reader["Login"],
                        LastName = (string)reader["Nom"],
                        FirstName = (string)reader["Prenom"],
                        Habilitation = reader["Manager"] == DBNull.Value ? Habilitation.Manager : Habilitation.Employee,
                        Productivity = (float)reader["TauxProductivite"],
                        ListTask = new List<Entity.Task>(),
                        Job = new Job()
                        {
                            Code = (string)reader["CodeMetier"],
                            Label = (string)reader["Libelle"],
                            ListActivity = new List<Activity>()
                        }
                    };

                    listEmployee.Add(employee);
                }

                Employee currentEmployee = listEmployee.Last();

                // Si la liste des activités de l'employé courrant est vide ou si l'activité change ajouter une nouvelle activité
                if (!currentEmployee.Job.ListActivity.Any() || currentEmployee.Job.ListActivity.Last().Code != (string)reader["CodeActiviteMetier"])
                {
                    Activity activity = new Activity()
                    {
                        Code = (string)reader["CodeActiviteMetier"],
                        Label = (string)reader["LibelleActiviteMetier"],
                        IsAnnex = (bool)reader["AnnexeActiviteMetier"]
                    };

                    currentEmployee.Job.ListActivity.Add(activity);
                }

                // Si la liste des taches de l'employé courrant est vide ou si la tache change ajouter une nouvelle activité
                if (!currentEmployee.ListTask.Any() || currentEmployee.ListTask.Last().Id != (Guid)reader["IdTache"])
                {
                    Entity.Task task;
                    if ((bool)reader["Annexe"])
                        task = new Entity.Task();
                    else
                    {
                        task = new TaskProd()
                        {
                            Number = (int)reader["Numero"],
                            EstimatedRemainingTime = (float)reader["DureeRestanteEstimee"],
                            PredictedTime = (float)reader["DureePrevue"],
                            Module = new Module() { Code = (string)reader["CodeModule"] },
                            Version = new Entity.Version() { Number = (float)reader["NumeroVersion"] }
                        };
                    }

                    task.Id = (Guid)reader["IdTache"];
                    if (reader["Description"] != DBNull.Value)
                        task.Description = (string)reader["Description"];
                    task.Label = (string)reader["Libelle"];

                    task.Activity = new Activity()
                    {
                        Code = (string)reader["CodeActiviteTache"],
                        Label = (string)reader["LibelleActiviteTache"],
                        IsAnnex = (bool)reader["AnnexeActiviteTache"]
                    };
                    task.ListWorkTime = new List<WorkTime>();

                    currentEmployee.ListTask.Add(task);
                }

                Entity.Task currentTask = currentEmployee.ListTask.Last();

                if(!currentTask.ListWorkTime.Any() || currentTask.ListWorkTime.Last().WorkingDate != (DateTime)reader["DateTravail"])
                {
                    WorkTime workTime = new WorkTime()
                    {
                        Hours = (float)reader["Heures"],
                        Productivity = (float)reader["TauxProductivite"],
                        WorkingDate = (DateTime)reader["DateTravail"]
                    };

                    currentTask.ListWorkTime.Add(workTime);
                }

            }
        }
        #endregion

        #region Méthodes publiques
        static public List<Employee> GetListEmployee(string loginManager)
        {
            List<Employee> listEmployee = new List<Employee>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
            {
                string query = @"select p.Login, p.Nom, p.Prenom, p.Manager, p.TauxProductivite,
	                                            m.CodeMetier, m.Libelle,
	                                            act1.CodeActivite CodeActiviteMetier, act1.Libelle LibelleActiviteMetier, act1.Annexe AnnexeActiviteMetier,
	                                            t.IdTache, t.Description, t.Libelle, t.Annexe, 
	                                            act2.CodeActivite CodeActiviteTache, act2.Libelle LibelleActiviteTache, act2.Annexe AnnexeActiviteTache,
	                                            tr.Heures, tr.TauxProductivite, tr.DateTravail,
	                                            tp.Numero, tp.DureeRestanteEstimee, tp.DureePrevue, tp.CodeModule, tp.NumeroVersion
                                from jo.Personne p 
                                inner join jo.Metier m on p.CodeMetier = m.CodeMetier
                                inner join jo.ActiviteMetier am on m.CodeMetier = am.MetierCodeMetier
                                inner join jo.Activite act1 on am.ActiviteCodeActivite = act1.CodeActivite
                                inner join jo.Tache t on p.Login = t.Login
                                inner join jo.Activite act2 on t.CodeActivite = act2.CodeActivite
                                left outer join  jo.Travail tr on t.IdTache = tr.IdTache 
                                left outer join jo.TacheProd tp on t.IdTache = tp.IdTache
                                where p.Manager = @LoginManager
                                order by p.Login, m.CodeMetier, act1.CodeActivite, t.IdTache, act2.CodeActivite, tr.IdTache";

                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.Add(new SqlParameter("@LoginManager", SqlDbType.VarChar));
                cmd.Parameters["@LoginManager"].Value = loginManager;

                cnx.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderToListEmployee(reader, listEmployee);
                }
            }

            return listEmployee;
        }

        #endregion
    }
}
