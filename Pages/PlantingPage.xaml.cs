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
    /// Логика взаимодействия для PlantingPage.xaml
    /// </summary>
    public partial class PlantingPage : Page
    {
        private readonly PraktikaContext _context;
        public PlantingPage(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PlantingList.ItemsSource = _context.Plantings.ToList();
        }
        private void EditMI_CLK(object sender, RoutedEventArgs e)
        {
            EditPlantingWin editWin = App.Current.Windows.OfType<EditPlantingWin>().FirstOrDefault();

            if (editWin == null || editWin.Visibility != Visibility.Visible)
            {
                editWin = new EditPlantingWin(_context, (Planting)PlantingList.SelectedItem);
                editWin.ShowDialog();
                PlantingList.ItemsSource = _context.Plantings.ToList();
            }
        }
        private void DeleteMI_CLK(object sender, RoutedEventArgs e)
        {
            var deletePlanting = (Planting)PlantingList.SelectedItem;
            var result = MessageBox.Show("Вы уверены, что хотите безвозвратно удалить насаждение?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (deletePlanting != null)
                {
                    if (!_context.PlantingParks.Any(entity => entity.PlantingId == deletePlanting.PlantingId))
                    {
                        _context.Plantings.Remove(deletePlanting);
                        _context.SaveChanges();
                    }
                    else
                        MessageBox.Show("Нельзя удалить это насаждение, так как на него ссылаются другие объекты.");
                }
            }
            PlantingList.ItemsSource = _context.Plantings.ToList();
        }
        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            AddPlantingWin addWin = App.Current.Windows.OfType<AddPlantingWin>().FirstOrDefault();

            if (addWin == null || addWin.Visibility != Visibility.Visible)
            {
                addWin = new AddPlantingWin(_context);
                addWin.ShowDialog();
                PlantingList.ItemsSource = _context.Plantings.ToList();
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();
            ICollectionView view = CollectionViewSource.GetDefaultView(PlantingList.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
                view.Filter = item => ((item as Planting).PlantingName.ToLower().Contains(searchText));
            else
                view.Filter = null;
        }
        private void PlantingList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var totalAvailableWidth = PlantingList.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var columns = (PlantingList.View as GridView).Columns;

            columns[0].Width = totalAvailableWidth * 0.4;
            columns[1].Width = totalAvailableWidth * 0.4;
            columns[2].Width = totalAvailableWidth * 0.2;
        }
    }
}
