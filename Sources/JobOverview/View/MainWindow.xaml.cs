﻿using JobOverview.ViewModel;
using System.Configuration;
using System.Windows;

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
			DataContext = new VMMain();

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

            if (!resCnx.Value) Close();
#endif
            // TODO : gérer la fermeture de la fenetre 

            // Affichage d'une fenêtre modale d'identification
            var dlgLog = new ModalWindow(new VMLogin());
			dlgLog.Title = "Identification";
            bool? resLog = dlgLog.ShowDialog();

            // Si l'utilisateur annule, on ferme l'application
            if (!resLog.Value) Close();

        }
    }
}
