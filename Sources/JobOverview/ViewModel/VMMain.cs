using JobOverview.Entity;
using JobOverview.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;

namespace JobOverview.ViewModel
{

	public class VMMain : ViewModelBase
	{
        //TODO: Current employee non static
        public static Employee CurrentEmployee { get; set; }
        public static List<Employee> ListEmployee { get; set; }
                                           // Vue-modèle courante sur laquelle est liées le ContentControl
                                           // de la zone principale
        private ViewModelBase _VMCourante;
		public ViewModelBase VMCourante
		{
			get { return _VMCourante; }
			private set
			{
				SetProperty(ref _VMCourante, value);
			}
		}

        public string XmlPath { get; set; }

        public VMMain()
        {
            CurrentEmployee = new Employee();
            ListEmployee = DAL.GetListEmployeeWithoutTasks();
        }

		#region Commandes
		//private ICommand _cmdLogin;
		//public ICommand CmdLogin
		//{
		//	get
		//	{
		//		if (_cmdLogin == null)
		//			_cmdLogin = new RelayCommand(() => VMCourante = new VMLogin());
		//		return _cmdLogin;
		//	}
		//}

        private ICommand _cmdVMAbout;
        public ICommand CmdVMABout
        {
            get
            {
                if (_cmdVMAbout == null)
                    _cmdVMAbout = new RelayCommand(() => { });
                return _cmdVMAbout;
            }
        }

        private ICommand _cmdVMTaskConsultation;
        public ICommand CmdVMTaskConsultation
        {
            get
            {
                // TODO VMMain::CmdVMTaskConsultation : utiliser l'employé courrant pour la création de chaque VM (à discuter)
                if (_cmdVMTaskConsultation == null)
                    _cmdVMTaskConsultation = new RelayCommand(() =>  VMCourante = new VMTaskConsultation(CurrentEmployee));
                return _cmdVMTaskConsultation;
            }
        }

        private ICommand _cmdVMVersion;
        public ICommand CmdVMVersion
        {
            get
            {
                if (_cmdVMVersion == null)
                    _cmdVMVersion = new RelayCommand(() => { });
                return _cmdVMVersion;
            }
        }

        private ICommand _cmdVMTaskManaging;
        public ICommand CmdVMTaskManaging
        {
            get
            {
                if (_cmdVMTaskManaging == null)
                    _cmdVMTaskManaging = new RelayCommand(() => VMCourante = new VMTaskManaging(ListEmployee), ActiverEmployee);
                return _cmdVMTaskManaging;
            }
        }

        private ICommand _cmdExportToXML;
        public ICommand CmdExportToXML
        {
            get
            {
                if (_cmdExportToXML == null)
                    _cmdExportToXML = new RelayCommand(ExportToXML, ActiverEmployee);
                return _cmdExportToXML;
            }
        }

        private void ExportToXML()
        {
            // Chargement de la liste des taches pour tous les employés de la liste pour lesquels ce n'est pas déjà fait
            foreach (var e in ListEmployee)
                if (e.ListTask == null)
                    e.ListTask = new System.Collections.ObjectModel.ObservableCollection<Entity.Task>(DAL.GetListTask(e.Login));

            // Récupération du dossier où les taches seront exportées au format .xml
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DAL.ExportListTaskEmployeeToXML(ListEmployee, dlg.SelectedPath);
                    System.Windows.MessageBox.Show("L'exportation s'est terminée sans erreur.");
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Une erreur s'est produite, l'exportation a échoué.", "Erreur", MessageBoxButton.OKCancel , System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private bool ActiverEmployee()
        {
            return CurrentEmployee.Habilitation != Habilitation.Employee;
        }

        #endregion
    }
}
