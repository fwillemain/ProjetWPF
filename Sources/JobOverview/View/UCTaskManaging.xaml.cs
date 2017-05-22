using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UCTaskCreation.xaml
    /// </summary>
    public partial class UCTaskCreation : UserControl
    {
        public UCTaskCreation()
        {
            InitializeComponent();
        }
        // TODO : faire gaffe quand on change de logiciel l'état de la liste de version de la combobox
        /// <summary>
        /// Filtre les taches en fonction du logiciel choisis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterTaskWithSoftware(object sender, SelectionChangedEventArgs e)
        {
                ICollectionView view = CollectionViewSource.GetDefaultView(lvListTaskProd.DataContext);
            if (view != null)
                view.Filter = FilterBySoftware; 
        }
        /// <summary>
        /// Filtre les taches par rapport à un logiciel.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FilterBySoftware(object obj)
        {
            return (((Entity.Software)obj).Code == ((Entity.Software)cbFilterPerSoftware.SelectedItem).Code);
        }
        /// <summary>
        /// Filtre les taches en fonction de la version choisis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterTaskWithVersion(object sender, SelectionChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(lvListTaskProd.DataContext);
            if (view != null)
                view.Filter = FilterByVersion;
        }
        /// <summary>
        /// Filtre les taches par rapport à une version.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FilterByVersion(object obj)
        {
            return (((Entity.Version)obj).Number == ((Entity.Version)cbFilterPerSoftware.SelectedItem).Number);
        }
    }
}
