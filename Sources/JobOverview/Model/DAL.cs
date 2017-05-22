using JobOverview.Entity;
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
        /// <summary>
        /// Rempli la liste passée en paramètre avec la liste des employés sans la liste de taches décrite dans le SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="listEmployee"></param>
        private static void DataReaderToListEmployeeWithoutTasks(SqlDataReader reader, List<Employee> listEmployee)
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
                        CodeTeam = (string)reader["CodeEquipe"],
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
            }
        }

        /// <summary>
        /// Rempli la liste passée en paramètre avec la liste de taches décrite dans le SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="listTask"></param>
        private static void DataReaderToListTask(SqlDataReader reader, List<Entity.Task> listTask)
        {
            while (reader.Read())
            {

                // Si la liste des taches de l'employé courrant est vide ou si la tache est déjà dans la liste ajouter une nouvelle activité
                if (!listTask.Any() || !listTask.Where(t => t.Id == (Guid)reader["IdTache"]).Any())
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
                            Version = new Entity.Version() { Number = (float)reader["NumeroVersion"] },
                            Software = new Software() { Code = (string)reader["CodeLogiciel"] }
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

                    listTask.Add(task);
                }

                // Si il n'y a pas de temps de travail à ajouter, passer à la ligne suivante
                if (reader["DateTravail"] == DBNull.Value) continue;

                Entity.Task currentTask = listTask.Where(t => t.Id == (Guid)reader["IdTache"]).First();

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

        /// <summary>
        /// Rempli la liste passée en paramètre avec la liste des logiciels décrite dans le SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="listSoftware"></param>
        private static void DataReaderToListSoftware(SqlDataReader reader, List<Software> listSoftware)
        {
            while (reader.Read())
            {
                // Si la liste des logiciels est vide ou si le logiciel change créer un nouveau logiciel
                if (!listSoftware.Any() || listSoftware.Last().Code != (string)reader["CodeLogiciel"])
                {
                    Software software = new Software()
                    {
                        Code = (string)reader["CodeLogiciel"],
                        Name = (string)reader["Nom"],
                        ListModule = new List<Module>(),
                        ListVersion = new List<Entity.Version>()
                    };

                    listSoftware.Add(software);
                }

                Software currentSoftware = listSoftware.Last();

                // Si la liste des modules est vide ou si le module n'existe pas déjà créer un nouveau module
                if (!currentSoftware.ListModule.Any() || !currentSoftware.ListModule.Where(m => m.Code == (string)reader["CodeModule"]).Any())
                {
                    Module module = new Module()
                    {
                        Code = (string)reader["CodeModule"],
                        Label = (string)reader["Libelle"],
                        ListSubModule = new List<Module>()
                    };

                    // Si il s'agit d'un module l'ajouter à la liste des modules
                    if (reader["CodeModuleParent"] == DBNull.Value)
                        currentSoftware.ListModule.Add(module);
                    // Sinon, il s'agit d'un sous-module et on l'ajoute la liste des sous-module du module parent si il n'y est pas déjà
                    else
                    {
                        Module moduleParent = currentSoftware.ListModule.Where(m => m.Code == (string)reader["CodeModuleParent"]).First();

                        if (!moduleParent.ListSubModule.Where(sm => sm.Code == module.Code).Any())
                            moduleParent.ListSubModule.Add(module);
                    }
                }

                // Si la liste des versions est vide ou si la version n'existe pas déjà créer une nouvelle version
                if (!currentSoftware.ListVersion.Any() || !currentSoftware.ListVersion.Where(v => v.Number == (float)reader["NumeroVersion"]).Any())
                {
                    Entity.Version version = new Entity.Version()
                    {
                        Number = (float)reader["NumeroVersion"],
                        Year = (short)reader["Millesime"],
                        SettingDate = (DateTime)reader["DateOuverture"],
                        EstimatedReleaseDate = (DateTime)reader["DateSortiePrevue"],
                        ActualReleaseDate = reader["DateSortieReelle"] != DBNull.Value ? (DateTime?)reader["DateSortieReelle"] : null,
                        NumberOfReleases = (int)reader["NbRelease"]
                    };

                    currentSoftware.ListVersion.Add(version);
                }
            }
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Renvoi la liste des employés sans charger leur liste de tâches.
        /// </summary>
        /// <param name="loginManager"></param>
        /// <returns></returns>
        static public List<Employee> GetListEmployeeWithoutTasks()
        {
            List<Employee> listEmployee = new List<Employee>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
            {
                string query = @"select p.Login, p.Nom, p.Prenom, p.Manager, p.TauxProductivite, p.CodeEquipe,
	                                            m.CodeMetier, m.Libelle,
	                                            act1.CodeActivite CodeActiviteMetier, act1.Libelle LibelleActiviteMetier, act1.Annexe AnnexeActiviteMetier
                                from jo.Personne p 
                                inner join jo.Metier m on p.CodeMetier = m.CodeMetier
                                inner join jo.ActiviteMetier am on m.CodeMetier = am.MetierCodeMetier
                                inner join jo.Activite act1 on am.ActiviteCodeActivite = act1.CodeActivite
                                order by p.Login, m.CodeMetier, act1.CodeActivite";

                SqlCommand cmd = new SqlCommand(query, cnx);

                cnx.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderToListEmployeeWithoutTasks(reader, listEmployee);
                }
            }

            return listEmployee;
        }

        /// <summary>
        /// Renvoi la liste des employés dont le login du manager est passé en paramètre sans charger leur liste de taches.
        /// </summary>
        /// <param name="loginManager"></param>
        /// <returns></returns>
        static public List<Employee> GetListEmployeeWithoutTasks(string loginManager)
        {
            List<Employee> listEmployee = new List<Employee>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
            {
                string query = @"select p.Login, p.Nom, p.Prenom, p.Manager, p.TauxProductivite, p.CodeEquipe,
	                                            m.CodeMetier, m.Libelle,
	                                            act1.CodeActivite CodeActiviteMetier, act1.Libelle LibelleActiviteMetier, act1.Annexe AnnexeActiviteMetier
                                from jo.Personne p 
                                inner join jo.Metier m on p.CodeMetier = m.CodeMetier
                                inner join jo.ActiviteMetier am on m.CodeMetier = am.MetierCodeMetier
                                inner join jo.Activite act1 on am.ActiviteCodeActivite = act1.CodeActivite
                                where p.Manager = @LoginManager
                                order by p.Login, m.CodeMetier, act1.CodeActivite";

                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.Add(new SqlParameter("@LoginManager", SqlDbType.VarChar));
                cmd.Parameters["@LoginManager"].Value = loginManager;

                cnx.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderToListEmployeeWithoutTasks(reader, listEmployee);
                }
            }

            return listEmployee;
        }

        /// <summary>
        /// Renvoi la liste des taches de l'employé dont le login est passé en paramètre.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        static public List<Entity.Task> GetListTask(string login)
        {
            List<Entity.Task> listTask = new List<Entity.Task>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
            {
                string query = @"SELECT t.IdTache, t.Description, t.Libelle, t.Annexe, 
                                    act2.CodeActivite CodeActiviteTache, act2.Libelle LibelleActiviteTache, act2.Annexe AnnexeActiviteTache,
                                    tr.Heures, tr.TauxProductivite, tr.DateTravail,
                                    tp.Numero, tp.DureeRestanteEstimee, tp.DureePrevue,
                                    tp.CodeModule, tp.NumeroVersion, tp.CodeLogicielVersion AS CodeLogiciel
                                    FROM jo.Personne p 
                                    INNER JOIN jo.Tache t ON p.Login = t.Login
                                    INNER JOIN jo.Activite act2 ON t.CodeActivite = act2.CodeActivite
                                    LEFT OUTER JOIN  jo.Travail tr ON t.IdTache = tr.IdTache 
                                    LEFT OUTER JOIN jo.TacheProd tp ON t.IdTache = tp.IdTache
                                    WHERE p.Login = @Login
                                    ORDER by t.IdTache, act2.CodeActivite, tr.DateTravail";

                SqlCommand cmd = new SqlCommand(query, cnx);
                cmd.Parameters.Add(new SqlParameter("@Login", SqlDbType.VarChar));
                cmd.Parameters["@Login"].Value = login;

                cnx.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderToListTask(reader, listTask);
                }
            }

            return listTask;
        }

        /// <summary>
        /// Renvoi la liste des logiciels.
        /// </summary>
        /// <returns></returns>
        static public List<Software> GetListSoftware()
        {
            List<Software> listSoftware = new List<Software>();

            using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
            {
                string query = @"select distinct l.CodeLogiciel, l.Nom,
	                                        v.NumeroVersion, v.Millesime, v.DateOuverture, v.DateSortiePrevue, v.DateSortieReelle,
	                                        m1.CodeModule, m1.Libelle, m2.CodeModule CodeModuleParent,
	                                        Count(1) over(partition by l.CodeLogiciel, v.NumeroVersion, m1.CodeModule, m2.CodeModule) NbRelease
                                 from jo.Logiciel l
                                 inner join jo.Version v on l.CodeLogiciel = v.CodeLogiciel
                                 inner join jo.Release r on l.CodeLogiciel = r.CodeLogiciel and v.NumeroVersion = r.NumeroVersion
                                 inner join jo.Module m1 on l.CodeLogiciel = m1.CodeLogiciel
                                 left outer join jo.Module m2 on m1.CodeModuleParent = m2.CodeModule
                                 order by l.CodeLogiciel, v.NumeroVersion, m2.CodeModule";

                SqlCommand cmd = new SqlCommand(query, cnx);

                cnx.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    DataReaderToListSoftware(reader, listSoftware);
                }
            }

            return listSoftware;
        }

        /// <summary>
        /// Exporte la liste donnée en paramètre sous forme xml et la stock au chemin spécifié.
        /// </summary>
        /// <param name="listTask">Liste à sauvegrder</param>
        /// <param name="path">chemin du dossier et nom du fichier avec extension.</param>
        static public void ExportListTaskEmployeeToXML(List<Employee> listEmployee, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>),
                                       new XmlRootAttribute("ListEmployee"));

            using (var sw = new StreamWriter(path))
            {
                serializer.Serialize(sw, listEmployee);
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
               new XmlRootAttribute("ListTask"));

            using (var sr = new StreamReader(path))
            {
                ListTask = (List<Entity.Task>)deserializer.Deserialize(sr);
            }

            return ListTask;
        }
        #endregion

        #region Méthodes pour un besoin ultérieur
        ///// <summary>
        ///// Renvoi l'employé dont le login est passé passé en paramètre.
        ///// </summary>
        ///// <param name="login"></param>
        ///// <returns></returns>
        //static public Employee GetEmployee(string login)
        //{
        //    List<Employee> listEmployee = new List<Employee>();

        //    using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
        //    {
        //        string query = @"select p.Login, p.Nom, p.Prenom, p.Manager, p.TauxProductivite,
        //                                     m.CodeMetier, m.Libelle,
        //                                     act1.CodeActivite CodeActiviteMetier, act1.Libelle LibelleActiviteMetier, act1.Annexe AnnexeActiviteMetier,
        //                                     t.IdTache, t.Description, t.Libelle, t.Annexe, 
        //                                     act2.CodeActivite CodeActiviteTache, act2.Libelle LibelleActiviteTache, act2.Annexe AnnexeActiviteTache,
        //                                     tr.Heures, tr.TauxProductivite, tr.DateTravail,
        //                                     tp.Numero, tp.DureeRestanteEstimee, tp.DureePrevue, tp.CodeModule, tp.NumeroVersion
        //                        from jo.Personne p 
        //                        inner join jo.Metier m on p.CodeMetier = m.CodeMetier
        //                        inner join jo.ActiviteMetier am on m.CodeMetier = am.MetierCodeMetier
        //                        inner join jo.Activite act1 on am.ActiviteCodeActivite = act1.CodeActivite
        //                        inner join jo.Tache t on p.Login = t.Login
        //                        inner join jo.Activite act2 on t.CodeActivite = act2.CodeActivite
        //                        left outer join  jo.Travail tr on t.IdTache = tr.IdTache 
        //                        left outer join jo.TacheProd tp on t.IdTache = tp.IdTache
        //                        where p.Login = @Login
        //                        order by p.Login, m.CodeMetier, act1.CodeActivite, t.IdTache, act2.CodeActivite, tr.DateTravail";

        //        SqlCommand cmd = new SqlCommand(query, cnx);
        //        cmd.Parameters.Add(new SqlParameter("@Login", SqlDbType.VarChar));
        //        cmd.Parameters["@Login"].Value = login;

        //        cnx.Open();

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            DataReaderToListEmployee(reader, listEmployee);
        //        }
        //    }

        //    return listEmployee.FirstOrDefault();
        //}

        ///// <summary>
        ///// Renvoi la liste des employés dont le login du manager est passé en paramètre.
        ///// </summary>
        ///// <param name="loginManager"></param>
        ///// <returns></returns>
        //static public List<Employee> GetListEmployee(string loginManager)
        //{
        //    List<Employee> listEmployee = new List<Employee>();

        //    using (SqlConnection cnx = new SqlConnection(Properties.Settings.Default.JobOverviewConnectionStringDefault))
        //    {
        //        string query = @"select p.Login, p.Nom, p.Prenom, p.Manager, p.TauxProductivite,
        //                                     m.CodeMetier, m.Libelle,
        //                                     act1.CodeActivite CodeActiviteMetier, act1.Libelle LibelleActiviteMetier, act1.Annexe AnnexeActiviteMetier,
        //                                     t.IdTache, t.Description, t.Libelle, t.Annexe, 
        //                                     act2.CodeActivite CodeActiviteTache, act2.Libelle LibelleActiviteTache, act2.Annexe AnnexeActiviteTache,
        //                                     tr.Heures, tr.TauxProductivite, tr.DateTravail,
        //                                     tp.Numero, tp.DureeRestanteEstimee, tp.DureePrevue, tp.CodeModule, tp.NumeroVersion
        //                        from jo.Personne p 
        //                        inner join jo.Metier m on p.CodeMetier = m.CodeMetier
        //                        inner join jo.ActiviteMetier am on m.CodeMetier = am.MetierCodeMetier
        //                        inner join jo.Activite act1 on am.ActiviteCodeActivite = act1.CodeActivite
        //                        left outer join jo.Tache t on p.Login = t.Login
        //                        inner join jo.Activite act2 on t.CodeActivite = act2.CodeActivite
        //                        left outer join  jo.Travail tr on t.IdTache = tr.IdTache 
        //                        left outer join jo.TacheProd tp on t.IdTache = tp.IdTache
        //                        where p.Manager = @LoginManager
        //                        order by p.Login, m.CodeMetier, act1.CodeActivite, t.IdTache, act2.CodeActivite, tr.DateTravail";

        //        SqlCommand cmd = new SqlCommand(query, cnx);
        //        cmd.Parameters.Add(new SqlParameter("@LoginManager", SqlDbType.VarChar));
        //        cmd.Parameters["@LoginManager"].Value = loginManager;

        //        cnx.Open();

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            DataReaderToListEmployee(reader, listEmployee);
        //        }
        //    }

        //    return listEmployee;
        //}

        ///// <summary>
        ///// Rempli la liste passée en paramètre avec la liste des employés décrite dans le SqlDataReader
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <param name="listEmployee"></param>
        //private static void DataReaderToListEmployee(SqlDataReader reader, List<Employee> listEmployee)
        //{
        //    while (reader.Read())
        //    {
        //        // Si la liste d'employé est vide ou si l'employé change ajouter un nouvel employé
        //        if (!listEmployee.Any() || listEmployee.Last().Login != (string)reader["Login"])
        //        {
        //            Employee employee = new Employee()
        //            {
        //                Login = (string)reader["Login"],
        //                LastName = (string)reader["Nom"],
        //                FirstName = (string)reader["Prenom"],
        //                Habilitation = reader["Manager"] == DBNull.Value ? Habilitation.Manager : Habilitation.Employee,
        //                Productivity = (float)reader["TauxProductivite"],
        //                ListTask = new List<Entity.Task>(),
        //                Job = new Job()
        //                {
        //                    Code = (string)reader["CodeMetier"],
        //                    Label = (string)reader["Libelle"],
        //                    ListActivity = new List<Activity>()
        //                }
        //            };

        //            listEmployee.Add(employee);
        //        }


        //        Employee currentEmployee = listEmployee.Last();

        //        // Si la liste des activités de l'employé courrant est vide ou si l'activité change ajouter une nouvelle activité
        //        if (!currentEmployee.Job.ListActivity.Any() || currentEmployee.Job.ListActivity.Last().Code != (string)reader["CodeActiviteMetier"])
        //        {
        //            Activity activity = new Activity()
        //            {
        //                Code = (string)reader["CodeActiviteMetier"],
        //                Label = (string)reader["LibelleActiviteMetier"],
        //                IsAnnex = (bool)reader["AnnexeActiviteMetier"]
        //            };

        //            currentEmployee.Job.ListActivity.Add(activity);
        //        }

        //        // Si il n'y a pas de tache à ajouter, lire la ligne suivante
        //        if (reader["Annexe"] == DBNull.Value) continue;

        //        // Si la liste des taches de l'employé courrant est vide ou si la tache est déjà dans la liste ajouter une nouvelle activité
        //        if (!currentEmployee.ListTask.Any() || !currentEmployee.ListTask.Where(t => t.Id == (Guid)reader["IdTache"]).Any())
        //        {
        //            Entity.Task task;
        //            if ((bool)reader["Annexe"])
        //                task = new Entity.Task();
        //            else
        //            {
        //                task = new TaskProd()
        //                {
        //                    Number = (int)reader["Numero"],
        //                    EstimatedRemainingTime = (float)reader["DureeRestanteEstimee"],
        //                    PredictedTime = (float)reader["DureePrevue"],
        //                    Module = new Module() { Code = (string)reader["CodeModule"] },
        //                    Version = new Entity.Version() { Number = (float)reader["NumeroVersion"] }
        //                };
        //            }

        //            task.Id = (Guid)reader["IdTache"];
        //            if (reader["Description"] != DBNull.Value)
        //                task.Description = (string)reader["Description"];
        //            task.Label = (string)reader["Libelle"];

        //            task.Activity = new Activity()
        //            {
        //                Code = (string)reader["CodeActiviteTache"],
        //                Label = (string)reader["LibelleActiviteTache"],
        //                IsAnnex = (bool)reader["AnnexeActiviteTache"]
        //            };
        //            task.ListWorkTime = new List<WorkTime>();

        //            currentEmployee.ListTask.Add(task);
        //        }

        //        // Si il n'y a pas de temps de travail à ajouter, passer à la ligne suivante
        //        if (reader["DateTravail"] == DBNull.Value) continue;

        //        Entity.Task currentTask = currentEmployee.ListTask.Where(t => t.Id == (Guid)reader["IdTache"]).First();

        //        if (!currentTask.ListWorkTime.Any() ||
        //            currentTask.ListWorkTime.Last().WorkingDate != (DateTime)reader["DateTravail"])
        //        {
        //            WorkTime workTime = new WorkTime()
        //            {
        //                Hours = (float)reader["Heures"],
        //                Productivity = (float)reader["TauxProductivite"],
        //                WorkingDate = (DateTime)reader["DateTravail"]
        //            };

        //            currentTask.ListWorkTime.Add(workTime);
        //        }

        //    }
        //}
        #endregion
    }
}
