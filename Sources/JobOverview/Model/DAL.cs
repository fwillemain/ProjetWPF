﻿using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.ViewModel;
using System.Data;
using System.Xml.Serialization;
using System.IO;

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

                // Si il n'y a pas de tache à ajouter, lire la ligne suivante
                if (reader["Annexe"] == DBNull.Value) continue;

                // Si la liste des taches de l'employé courrant est vide ou si la tache est déjà dans la liste ajouter une nouvelle activité
                if (!currentEmployee.ListTask.Any() || !currentEmployee.ListTask.Where(t => t.Id == (Guid)reader["IdTache"]).Any())
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

                // Si il n'y a pas de temps de travail à ajouter, passer à la ligne suivante
                if (reader["DateTravail"] == DBNull.Value) continue;

                Entity.Task currentTask = currentEmployee.ListTask.Where(t => t.Id == (Guid)reader["IdTache"]).First();

                if (!currentTask.ListWorkTime.Any() || 
                    currentTask.ListWorkTime.Last().WorkingDate != (DateTime)reader["DateTravail"])
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
        /// <summary>
        /// Renvoi la liste des employés dont le login du manager est passé en paramètre.
        /// </summary>
        /// <param name="loginManager"></param>
        /// <returns></returns>
        static public List<Employee> GetListEmployee(string loginManager)
        {
            List<Employee> listEmployee = new List<Employee>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringFloMaison))
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
                                left outer join jo.Tache t on p.Login = t.Login
                                inner join jo.Activite act2 on t.CodeActivite = act2.CodeActivite
                                left outer join  jo.Travail tr on t.IdTache = tr.IdTache 
                                left outer join jo.TacheProd tp on t.IdTache = tp.IdTache
                                where p.Manager = @LoginManager
                                order by p.Login, m.CodeMetier, act1.CodeActivite, t.IdTache, act2.CodeActivite, tr.DateTravail";

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
        /// <summary>
        /// Renvoi l'employé dont le login est passé passé en paramètre.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        static public Employee GetEmployee(string login)
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
                                where p.Login = @Login
                                order by p.Login, m.CodeMetier, act1.CodeActivite, t.IdTache, act2.CodeActivite, tr.DateTravail";

                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.Add(new SqlParameter("@Login", SqlDbType.VarChar));
                cmd.Parameters["@Login"].Value = login;

                cnx.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderToListEmployee(reader, listEmployee);
                }
            }

            return listEmployee.FirstOrDefault();
        }

        /// <summary>
        /// Renvoi la liste des logiciels.
        /// </summary>
        /// <returns></returns>
        static public List<Software> GetListSoftware()
        {
            List<Software> listSoftware = new List<Software>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringFloMaison))
            {
                string query = @"";

                SqlCommand cmd = new SqlCommand(query, cnx);

                cnx.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderToListSoftware(reader, listSoftware);
                }
            }

            return listSoftware;
        }

        private static void DataReaderToListSoftware(SqlDataReader reader, List<Software> listSoftware)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Exporte la liste donnée en paramètre sous forme xml et la stock au chemin spécifié.
        /// </summary>
        /// <param name="listTask">Liste à sauvegrder</param>
        /// <param name="path">chemin du dossier et nom du fichier avec extension.</param>
        static public void ExportListTaskToXML(List<Entity.Task> listTask, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Entity.Task>),
                                       new XmlRootAttribute("listTask"));

            using (var sw = new StreamWriter(path))
            {
                serializer.Serialize(sw, listTask);
            }
        }
        /// <summary>
        /// Importe une liste dont le chemin est donné en paramètre.
        /// </summary>
        /// <param name="path">chemin du fichier xml à importer.</param>
        /// <returns></returns>
        static public List<Entity.Task> GetListTaskFromXml(string path)
        {
            List<Entity.Task> ListTask = null;

            XmlSerializer deserializer = new XmlSerializer(typeof(List<Entity.Task>),
               new XmlRootAttribute("listTask"));

            using (var sr = new StreamReader(path))
            {
                ListTask = (List<Entity.Task>)deserializer.Deserialize(sr);
            }

            return ListTask;
        }
        #endregion
    }
}