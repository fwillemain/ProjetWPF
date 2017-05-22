using JobOverview.Entity;
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
    /// Interaction logic for UCTaskConsultation.xaml
    /// </summary>
    public partial class UCTaskConsultation : UserControl
    {
        public UCTaskConsultation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Filtre les journées de travail en fonction des DatePickers DPDateMin et DPDateMax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterTaskWithDateMinMax(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(((ViewModel.VMTaskConsultation)this.DataContext).CurrentEmployeeListTaskProd);
            view.Filter = FilterByDate;
        }

        private bool FilterByDate(object obj)
        {
            return (((WorkTime)obj).WorkingDate < DPDateMax.SelectedDate && ((WorkTime)obj).WorkingDate > DPDateMin.SelectedDate);
        }

    }
}
