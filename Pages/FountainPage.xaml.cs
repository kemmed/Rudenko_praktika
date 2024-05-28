using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.EntityFrameworkCore;
using Rudenko_praktika.Models;
using Rudenko_praktika.Windows;
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
    /// Логика взаимодействия для FountainPage.xaml
    /// </summary>
    public partial class FountainPage : Page
    {
        private readonly PraktikaContext _context;
        public FountainPage(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FountainList.ItemsSource = _context.Fountains.Include(x => x.Park).ToList();
        }

        private void EditMI_CLK(object sender, RoutedEventArgs e)
        {
            EditFountainWin editWin = App.Current.Windows.OfType<EditFountainWin>().FirstOrDefault();

            if (editWin == null || editWin.Visibility != Visibility.Visible)
            {
                editWin = new EditFountainWin(_context, (Fountain)FountainList.SelectedItem);
                editWin.ShowDialog();
                FountainList.ItemsSource = _context.Fountains.ToList();
            }
        }
        private void DeleteMI_CLK(object sender, RoutedEventArgs e)
        {
            var deleteFountain = (Fountain)FountainList.SelectedItem;
            var result = MessageBox.Show("Вы уверены, что хотите безвозвратно удалить фонтан?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result==MessageBoxResult.Yes)
            {
                _context.Fountains.Remove(deleteFountain);
                _context.SaveChanges();
            }
            FountainList.ItemsSource = _context.Fountains.ToList();
        }
        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            AddFountainWin addWin = App.Current.Windows.OfType<AddFountainWin>().FirstOrDefault();

            if (addWin == null || addWin.Visibility != Visibility.Visible)
            {
                addWin = new AddFountainWin(_context);
                addWin.ShowDialog();
                FountainList.ItemsSource = _context.Fountains.ToList();
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();
            ICollectionView view = CollectionViewSource.GetDefaultView(FountainList.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
                view.Filter = item => ((item as Fountain).Park.ParkName.ToLower().Contains(searchText));
            else
                view.Filter = null;
        }
        private void FountainList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var totalAvailableWidth = FountainList.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var columns = (FountainList.View as GridView).Columns;

            columns[0].Width = totalAvailableWidth * 0.16;
            columns[1].Width = totalAvailableWidth * 0.1;
            columns[2].Width = totalAvailableWidth * 0.16;
            columns[3].Width = totalAvailableWidth * 0.16;
            columns[4].Width = totalAvailableWidth * 0.16;
            columns[5].Width = totalAvailableWidth * 0.22;
        }
    }
}
