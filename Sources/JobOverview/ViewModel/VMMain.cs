using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JobOverview.ViewModel
{
    public enum Habilitation
    {
        Manager, Employee
    }
	public class VMMain : ViewModelBase
	{
        
        private Habilitation _habilitation;
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

        public VMMain()
        {
            //List<Employee> listEmployee = DAL.GetListEmployee("JROUSSET");
        }

		#region Commandes
		private ICommand _cmdLogin;
		public ICommand CmdLogin
		{
			get
			{
				if (_cmdLogin == null)
					_cmdLogin = new RelayCommand(() => VMCourante = new VMLogin());
				return _cmdLogin;
			}
		}

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
                if (_cmdVMTaskConsultation == null)
                    _cmdVMTaskConsultation = new RelayCommand(() => { });
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
                    _cmdVMTaskManaging = new RelayCommand(() => VMCourante = new VMTaskManaging(), ActiverEmployee);
                return _cmdVMTaskManaging;
            }
        }

        private ICommand _cmdExportToXML;
        public ICommand CmdExportToXML
        {
            get
            {
                if (_cmdExportToXML == null)
                    _cmdExportToXML = new RelayCommand(() => { }, ActiverEmployee);
                return _cmdExportToXML;
            }
        }

        private bool ActiverEmployee()
        {
            return _habilitation != Habilitation.Employee;
        }

        #endregion
    }
}
