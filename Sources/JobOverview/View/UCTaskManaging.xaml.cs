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

        /// <summary>
        /// Filtre les taches en fonction du logiciel choisis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterTaskWithDateMinMax(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(((ViewModel.VMTaskManaging)this.DataContext).ListEmployee.Select(emp => emp.ListTask));
            view.Filter = FilterBySoftware;
        }

        private bool FilterBySoftware(object obj)
        {
            return (((Entity.Software)obj).Code == ((Entity.Software)cbFilterPerSoftware.SelectedItem).Code);
        }

    }
}
