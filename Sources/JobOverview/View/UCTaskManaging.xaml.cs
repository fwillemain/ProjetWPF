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
    /// Interaction logic for UCTaskCreation.xaml
    /// </summary>
    public partial class UCTaskCreation : UserControl
    {
        public UCTaskCreation()
        {
            InitializeComponent();
        }

        #region Obsolète
        ///// <summary>
        ///// Filtre les taches en fonction du logiciel choisis.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void FilterTaskWithSoftware(object sender, SelectionChangedEventArgs e)
        //{
        //        ICollectionView view = CollectionViewSource.GetDefaultView(((ViewModel.VMTaskManaging)DataContext).ListTaskProd);
        //    if (view != null)
        //        view.Filter = FilterBySoftware;
        //}
        ///// <summary>
        ///// Filtre les taches par rapport à un logiciel.
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //private bool FilterBySoftware(object obj)
        //{
        //    return (((TaskProd)obj).Software.Code == ((Software)cbFilterPerSoftware.SelectedItem).Code);
        //}
        ///// <summary>
        ///// Filtre les taches en fonction de la version choisis.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void FilterTaskWithVersion(object sender, SelectionChangedEventArgs e)
        //{
        //    ICollectionView view = CollectionViewSource.GetDefaultView(((ViewModel.VMTaskManaging)DataContext).ListTaskProd);
        //    if (view != null)
        //        view.Filter = FilterByVersion;
        //}
        ///// <summary>
        ///// Filtre les taches par rapport à une version.
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //private bool FilterByVersion(object obj)
        //{
        //    return (((TaskProd)obj).Version.Number == ((Entity.Version)cbFilterPerVersion.SelectedItem).Number);
        //} 

        //private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (tabTaskAnnex.IsSelected)
        //    {
        //        ICollectionView view = CollectionViewSource.GetDefaultView(((ViewModel.VMTaskManaging)DataContext).SelectedEmployee.ListTask);
        //        if (view != null)
        //        {
        //            view.Filter = FilterBySoftware;
        //            view.Filter += FilterByVersion;
        //            view.Filter += FilterByTaskAnnex;
        //        }
        //    }
        //    else if (tabTaskProd.IsSelected)
        //    {
        //        ICollectionView view = CollectionViewSource.GetDefaultView(((ViewModel.VMTaskManaging)DataContext).SelectedEmployee.ListTask);
        //        if (view != null)
        //        {
        //            view.Filter = FilterBySoftware;
        //            view.Filter += FilterByVersion;
        //            view.Filter += FilterByTaskProd;
        //        }
        //    }
        ////}
        ///// <summary>
        ///// Filtre les taches annexes.
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //private bool FilterByTaskAnnex(object obj)
        //{
        //    return (((Entity.Task)obj).Activity.IsAnnex);
        //}
        ///// <summary>
        ///// Filtre les taches de productions.
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //private bool FilterByTaskProd(object obj)
        //{
        //    return !(((Entity.Task)obj).Activity.IsAnnex);
        //}

        //private void lvListEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    lvListTaskAnnex.DataContext = ListTaskAnnex;
        //    lvListTaskProd.DataContext = ListTaskProd;
        //}
        #endregion
    }
}
