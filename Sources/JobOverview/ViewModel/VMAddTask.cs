using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{
    public class VMAddTask :ViewModelBase
    {
        #region Champs privés
        private ObservableCollection<Activity> _listActivity;
        private Software _selectedSoftware;
        private Entity.Version _selectedVersion;
        private Module _selectedModule;
        #endregion
        #region Propriétées publiques
        public List<Software> ListSoftware { get; set; }
        public Software SelectedSoftware
        {
            get
            { return _selectedSoftware != null ? _selectedSoftware : ListSoftware.FirstOrDefault(); }
            set
            { SetProperty(ref _selectedSoftware, value); }
        }
        public Entity.Version SelectedVersion
        {
            get
            { return _selectedVersion != null ? _selectedVersion : ListSoftware.Where(s => s.Code == SelectedSoftware.Code).FirstOrDefault().ListVersion.FirstOrDefault(); }
            set
            { SetProperty(ref _selectedVersion, value); }
        }
        public Module SelectedModule
        {
            get
            { return _selectedModule != null ? _selectedModule : ListSoftware.Where(s => s.Code == SelectedSoftware.Code).FirstOrDefault().ListModule.FirstOrDefault(); }
            set
            { SetProperty(ref _selectedModule, value); }
        }
        public Employee CurrentEmployee { get; set; }
        public ObservableCollection<Activity> ListActivity
        {
            get { return _listActivity; }
            set {SetProperty(ref _listActivity, value); }
        } 
        #endregion
        public VMAddTask(Employee selectedEmployee)
        {
            ListSoftware = DAL.GetListSoftware();
            CurrentEmployee = selectedEmployee;
            var tempList = CurrentEmployee.Job.ListActivity.Where(a => a.IsAnnex == false).ToList();
            foreach (Activity activity in CurrentEmployee.Job.ListActivity.Where(a => a.IsAnnex == true).ToList())
            {
                foreach (Activity empActivity in CurrentEmployee.ListTask.Select(t => t.Activity).Where(a => a.IsAnnex == true).ToList())
                {
                    if (empActivity != activity)
                    {
                        // Récupération des activités annexes qui n'ont pas encore de tâches associées.
                        tempList.Add(activity);
                    }
                }
            }
            ListActivity = new ObservableCollection<Activity>(tempList.Distinct().ToList());
        }

    }
}
