using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using JobOverview.Entity;
using System.Collections.ObjectModel;
using JobOverview.Model;
using System.Linq;

namespace UnitTestJobOverview
{
    [TestClass]
    public class TestDAL
    {
        [TestMethod]
        public void TestAddTaskList()
        {
            // Récupère la liste des taches de RBEAUMONT
            List<Task> listTask = DAL.GetListTask("RBEAUMONT");

            // Stock le nombre de ligne de temps de travail sur l'ensemble des taches
            int nbInitialWorkTime = listTask.Sum(t => t.ListWorkTime.Count);
            var listTaskToModify = GetModifiedTaskList(listTask);

            DAL.UpdateDatabaseWorkTimeOfTaskList(listTaskToModify);

            // Stock le nombre de ligne de temps de travail sur l'ensemble des taches après modification de la liste
            listTask = DAL.GetListTask("RBEAUMONT");
            int nbAfterAddingWorkTime = listTask.Sum(t => t.ListWorkTime.Count);

            // Vérifie la différence entre le nombre de ligne de temps de travail avant et après la modification
            Assert.AreEqual(12, nbAfterAddingWorkTime - nbInitialWorkTime);
        }

        [TestMethod]
        public void TestModifyTaskList()
        {
            List<Task> listTask = DAL.GetListTask("RBEAUMONT");

            // Stock le nombre de ligne de temps de travail sur l'ensemble des taches
            int nbInitialWorkTime = listTask.Sum(t => t.ListWorkTime.Count);
            var listTaskToModify = GetModifiedTaskList(listTask);

            DAL.UpdateDatabaseWorkTimeOfTaskList(listTaskToModify);

            // Stock le nombre de ligne de temps de travail sur l'ensemble des taches après suppression des 
            // enregistrements précédents
            listTask = DAL.GetListTask("RBEAUMONT");
            int nbAfterModifyWorkTime = listTask.Sum(t => t.ListWorkTime.Count);

            // Vérifie la différence entre le nombre de ligne de temps de travail avant et après l'ajout puis 
            // suppression des temps de travail
            Assert.AreEqual(0, nbAfterModifyWorkTime - nbInitialWorkTime);
        }

        [TestMethod]
        public void TestRemoveTaskList()
        {
            List<Task> listTask = DAL.GetListTask("RBEAUMONT");

            // Stock le nombre de ligne de temps de travail sur l'ensemble des taches
            int nbInitialWorkTime = listTask.Sum(t => t.ListWorkTime.Count);
            var listTaskToModify = GetModifiedTaskList(listTask);

            // Modifie la liste pour que les éléments soient supprimés
            ModifyTaskListToDelete(listTaskToModify);

            DAL.UpdateDatabaseWorkTimeOfTaskList(listTaskToModify);

            // Stock le nombre de ligne de temps de travail sur l'ensemble des taches après suppression des 
            // enregistrements précédents
            listTask = DAL.GetListTask("RBEAUMONT");
            int nbAfterDeleteWorkTime = listTask.Sum(t => t.ListWorkTime.Count);

            // Vérifie la différence entre le nombre de ligne de temps de travail avant et après l'ajout puis 
            // suppression des temps de travail
            Assert.AreEqual(-12, nbAfterDeleteWorkTime - nbInitialWorkTime);
        }

        [TestMethod]
        public void TestUpdateDatabaseTaskListOfEmployee()
        {
            // Récupère une liste d'employé contenant seulement RBEAUMONT avec sa liste de tache
            List<Employee> listEmployee = new List<Employee>();
            Employee employee = DAL.GetListEmployeeWithoutTasks().Where(e => e.Login == "RBEAUMONT").First();
            employee.ListTask = new ObservableCollection<Task>(DAL.GetListTask("RBEAUMONT"));

            // Stock le nombre de ligne de tache avant modification
            int nbInitialTask = employee.ListTask.Count;

            // Ajoute les taches à ajouter dans la BDD
            employee.ListTask = new ObservableCollection<Task>();
            AddTaskToEmployee(employee);
            listEmployee.Add(employee);

            // Stock les Id des taches ajoutées pour les supprimer ensuite
            List<Guid> listIdTask = employee.ListTask.Select(t => t.Id).ToList();

            // Ajoute les taches
            DAL.UpdateDatabaseTaskListOfEmployee(listEmployee, new List<Guid>());

            // Stock le nombre de ligne de tache après ajout
            employee.ListTask = new ObservableCollection<Task>(DAL.GetListTask("RBEAUMONT"));
            int nbAfterModifyTask = employee.ListTask.Count;

            // Vérifie la différence entre le nombre de tache avant et après la modification
            Assert.AreEqual(2, nbAfterModifyTask - nbInitialTask);

            // Efface les taches précédemment ajoutées
            DAL.UpdateDatabaseTaskListOfEmployee(new List<Employee>(), listIdTask);

            // Stock le nombre de ligne de tache après suppression
            employee.ListTask = new ObservableCollection<Task>(DAL.GetListTask("RBEAUMONT"));
            int nbAfterDeleteTask = employee.ListTask.Count;

            // Vérifie la différence entre le nombre de tache au début et après suppression des taches ajoutées
            Assert.AreEqual(0, nbAfterDeleteTask - nbInitialTask);
        }

        #region Méthodes privées
        /// <summary>
        /// Ajoute du temps de travail sur les deux premières taches annexes et taches prod et les renvoie
        /// sous forme de liste
        /// </summary>
        /// <param name="listTask"></param>
        /// <returns></returns>
        private List<Task> GetModifiedTaskList(List<Task> listTask)
        {
            List<Task> listTaskRes = new List<Task>();

            Task task1 = listTask.Where(t => t.Activity.IsAnnex == true).ToList().First();
            task1.ListWorkTime = new ObservableCollection<WorkTime>();

            #region Ajout des temps de travail
            task1.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today,
                Hours = 8,
                Productivity = 1
            });

            task1.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today.AddDays(-1),
                Hours = 8,
                Productivity = 1
            });

            task1.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today.AddDays(-2),
                Hours = 8,
                Productivity = 1
            });
            #endregion

            Task task2 = listTask.Where(t => t.Activity.IsAnnex == true).ToList()[1];
            task2.ListWorkTime = new ObservableCollection<WorkTime>();

            #region Ajout des temps de travail
            task2.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today,
                Hours = 8,
                Productivity = 1
            });

            task2.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today.AddDays(-1),
                Hours = 8,
                Productivity = 1
            });

            task2.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today.AddDays(-2),
                Hours = 8,
                Productivity = 1
            });
            #endregion

            Task taskProd1 = listTask.OfType<TaskProd>().First();
            taskProd1.ListWorkTime = new ObservableCollection<WorkTime>();

            #region Ajout des temps de travail

            taskProd1.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today,
                Hours = 8,
                Productivity = 1
            });

            taskProd1.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today.AddDays(-1),
                Hours = 8,
                Productivity = 1
            });

            taskProd1.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today.AddDays(-2),
                Hours = 8,
                Productivity = 1
            });
            #endregion

            Task taskProd2 = listTask.OfType<TaskProd>().ToList()[1];
            taskProd2.ListWorkTime = new ObservableCollection<WorkTime>();

            #region Ajout des temps de travail
            taskProd2.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today,
                Hours = 8,
                Productivity = 1
            });

            taskProd2.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today.AddDays(-1),
                Hours = 8,
                Productivity = 1
            });

            taskProd2.ListWorkTime.Add(new WorkTime()
            {
                WorkingDate = DateTime.Today.AddDays(-2),
                Hours = 8,
                Productivity = 1
            });
            #endregion

            listTaskRes.Add(task1);
            listTaskRes.Add(task2);
            listTaskRes.Add(taskProd1);
            listTaskRes.Add(taskProd2);

            return listTaskRes;
        }

        /// <summary>
        /// Modifie la productivité de tous les temps de travail à 0 pour informer la DAL qu'ils doivent être supprimé
        /// </summary>
        /// <param name="listTask"></param>
        private void ModifyTaskListToDelete(List<Task> listTask)
        {
            foreach (var t in listTask)
                foreach (var wt in t.ListWorkTime)
                    wt.Productivity = 0;
        }

        private void AddTaskToEmployee(Employee employee)
        {
            TaskProd taskProd = new TaskProd()
            {
                Id = Guid.NewGuid(),
                Activity = new Activity() { Code = "ANF" },
                EstimatedRemainingTime = 0,
                Label = "Tache prod 1",
                Module = new Module() { Code = "SEPARATION" },
                PredictedTime = 10,
                Software = new Software() { Code = "GENOMICA" },
                Version = new JobOverview.Entity.Version() { Number = 1.0F },
                ListWorkTime = new ObservableCollection<WorkTime>()
            };

            Task taskAnx = new Task()
            {
                Id = Guid.NewGuid(),
                Activity = new Activity() { Code = "APPUI_AUTRE_SERV", IsAnnex = true },
                Label = "Tache anx 1",
                ListWorkTime = new ObservableCollection<WorkTime>(),
            };

            employee.ListTask.Add(taskProd);
            employee.ListTask.Add(taskAnx);
        } 
        #endregion
    }
}
