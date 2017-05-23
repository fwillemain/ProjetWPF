using JobOverview.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JobOverview.View
{
    /// <summary>
    /// Interaction logic for UCAddTask.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private ViewModelBase _vm;

        /// <summary>
        /// Crée une fenêtre modale qui affichera la vue associée
        /// à la vue-modèle passée en paramètre
        /// </summary>
        /// <param name="vm"></param>
        public AddTaskWindow(ViewModelBase vm)
        {
            InitializeComponent();

            _vm = vm;
            ccPrinc.Content = vm;
            btnOK.Click += BtnOK_Click;
        }
    }
}
