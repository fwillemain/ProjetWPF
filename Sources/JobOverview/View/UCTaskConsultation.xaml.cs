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
        private void FilterWorkTimeWithDateMinMax(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(lvWorkTime.DataContext);
            view.Filter = FilterByDate;
        }

        private bool FilterByDate(object obj)
        {
            return (((WorkTime)obj).WorkingDate < dpDateMax.SelectedDate && ((WorkTime)obj).WorkingDate > dpDateMin.SelectedDate);
        }

        /// <summary>
        /// Filtre les taches de production par rapport au combobox Software, Version, Module, Activité
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterTaskWithSoftwareVersionModuleActivity(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(((ViewModel.VMTaskConsultation)DataContext).CurrentEmployeeListTaskProd);
            view.Filter = FilterBySoftwareVersionModuleActivity;
        }

        private bool FilterBySoftwareVersionModuleActivity(object obj)
        {
            return (((TaskProd)obj).Activity==(Activity)cbSortPerActivity.SelectedItem &&
                ((TaskProd)obj).Version == (Entity.Version)cbSortPerVersion.SelectedItem &&
                ((TaskProd)obj).Software == (Software)cbSortPerSoftware.SelectedItem &&
                ((TaskProd)obj).Module == (Module)cbSortPerModule.SelectedItem
                );
        }

    }
}
