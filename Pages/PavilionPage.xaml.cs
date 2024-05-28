using DocumentFormat.OpenXml.Drawing;
using Microsoft.EntityFrameworkCore;
using Rudenko_praktika.Models;
using Rudenko_praktika.Windows.Add;
using Rudenko_praktika.Windows.Edit;
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

namespace Rudenko_praktika.Pages
{
    /// <summary>
    /// Логика взаимодействия для PavilionPage.xaml
    /// </summary>
    public partial class PavilionPage : Page
    {
        private readonly PraktikaContext _context;
        public PavilionPage(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PavilionList.ItemsSource = _context.Pavilions.Include(x => x.Park).ToList();
        }

        private void EditMI_CLK(object sender, RoutedEventArgs e)
        {
            EditPavilionWin editWin = App.Current.Windows.OfType<EditPavilionWin>().FirstOrDefault();

            if (editWin == null || editWin.Visibility != Visibility.Visible)
            {
                editWin = new EditPavilionWin(_context,(Pavilion)PavilionList.SelectedItem);
                editWin.ShowDialog();
                PavilionList.ItemsSource = _context.Pavilions.Include(x => x.Park).ToList();
            }
        }
        private void DeleteMI_CLK(object sender, RoutedEventArgs e)
        {
            var deletePavilion = (Pavilion)PavilionList.SelectedItem;
            var result = MessageBox.Show("Вы уверены, что хотите безвозвратно удалить павильон?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _context.Pavilions.Remove(deletePavilion);
                _context.SaveChanges();
            }
            PavilionList.ItemsSource = _context.Pavilions.ToList();
        }
        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            AddPavilionWin addWin = App.Current.Windows.OfType<AddPavilionWin>().FirstOrDefault();

            if (addWin == null || addWin.Visibility != Visibility.Visible)
            {
                addWin = new AddPavilionWin(_context);
                addWin.ShowDialog();
                PavilionList.ItemsSource = _context.Pavilions.Include(x => x.Park).ToList();
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();
            ICollectionView view = CollectionViewSource.GetDefaultView(PavilionList.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
                view.Filter = item => ((item as Pavilion).PavilionName.ToLower().Contains(searchText));
            else
                view.Filter = null;
        }
        private void PavilionList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var totalAvailableWidth = PavilionList.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var columns = (PavilionList.View as GridView).Columns;

            columns[0].Width = totalAvailableWidth * 0.25;
            columns[1].Width = totalAvailableWidth * 0.25;
            columns[2].Width = totalAvailableWidth * 0.25;
            columns[3].Width = totalAvailableWidth * 0.25;
        }
    }
}
