using JobOverview.ViewModel;
using System.Configuration;
using System.Windows;
using System;

namespace JobOverview.View
{
    /// <summary>
    /// Fenêtre principale de l'application
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        // Après chargement de la fenêtre
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            // Affichage d'une fenêtre modale de choix de chaine de connexion (seulement en mode debugg)
            var dlgCnx = new ModalWindow(new VMConnection());
            dlgCnx.Title = "Connection";
            bool? resCnx = dlgCnx.ShowDialog();

            // Quitte le main si la fenêtre est fermée
            if (!resCnx.Value)
            {
                Close();
                return;
            }
#endif

            try
            {
                DataContext = new VMMain();
            }
            catch (ArgumentException)
            {
                // La création de la VMMain renvoi une exception de type ArgumentException si la chaine de connexion n'est pas valide
                MessageBox.Show("Une chaine de connexion valide doit être renseignée dans pour \"JobOverviewConnectionStringDefault\" du fichier de configuration de l'application.", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                Close();
                return;
            }

            //Affichage d'une fenêtre modale d'identification
            var dlgLog = new ModalWindow(new VMLogin());
            dlgLog.Title = "Identification";
            bool? resLog = dlgLog.ShowDialog();

            // Si l'utilisateur annule, on ferme l'application
            if (!resLog.Value) Close();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }
    }
}
