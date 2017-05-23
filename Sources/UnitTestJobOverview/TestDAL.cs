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
        public void TestUpdateDatabaseWorkTimeOfTaskList()
        {
            List<Task> listTask = DAL.GetListTask("RBEAUMONT");
            int nbInitialWorkTime = listTask.Sum(t => t.ListWorkTime.Count);
            var listTaskToModify = FillTaskList(listTask);

            DAL.UpdateDatabaseWorkTimeOfTaskList(listTaskToModify);

            listTask = DAL.GetListTask("RBEAUMONT");
            int nbAfterModifyWorkTime = listTask.Sum(t => t.ListWorkTime.Count);

            Assert.AreEqual(nbAfterModifyWorkTime - nbInitialWorkTime, 12);
        }

        /// <summary>
        /// Ajoute du temps de travail sur les deux premières taches annexes et taches prod
        /// </summary>
        /// <param name="listTask"></param>
        /// <returns></returns>
        private List<Task> FillTaskList(List<Task> listTask)
        {
            List<Task> listTaskRes = new List<Task>();

            Task task1 = listTask.Where(t => t.Activity.IsAnnex == true).ToList().First();

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

            Task taskProd1 = listTask.OfType<TaskProd>().FirstOrDefault();

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
    }
}
