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
            DataContext = _vm;
            tbxPredictedTime.TextChanged += TbxPredictedTime_TextChanged;
        }

        private void TbxPredictedTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            float testFloat;
            if (tbxPredictedTime.Text.ToString().Length >5) //TODO taille des variables
            {
                MessageBox.Show("Veuillez entrer un temps à 5 chiffres.");
                tbxPredictedTime.Text = tbxPredictedTime.Text.Remove(5, 1);
            }
            if ( !string.IsNullOrEmpty(tbxPredictedTime.Text.ToString()) && !float.TryParse(tbxPredictedTime.Text, out testFloat))
            {
                MessageBox.Show("Veuillez entrer un nombre.");
                tbxPredictedTime.Clear();
            }
        }

        private void cbTypeTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ccFilling.Visibility = (bool)cbTypeTask.SelectedValue ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
