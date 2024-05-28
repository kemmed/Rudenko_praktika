using Microsoft.EntityFrameworkCore;
using Rudenko_praktika.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Rudenko_praktika.Windows.Edit
{
    public partial class EditPlantingParkWin : Window
    {
        private readonly PraktikaContext _context;
        Park currentPark;
        List<Planting> currentParkPlantings;
        List<PlantingPark> tmpParkPlantings;
        List<Planting> allAvailablePlantings;
        List<TextBox> countTBs;
        public EditPlantingParkWin(PraktikaContext context, Park currentPark)
        {
            InitializeComponent();
            _context = context;
            this.currentPark = currentPark;
            countTBs = new List<TextBox>();
        }
        private void Refresh()
        {
            foreach (TextBox tb in countTBs)
            {
                if (!CountTextBoxes.Children.Contains(tb))
                    CountTextBoxes.Children.Add(tb);
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();
            ICollectionView view = CollectionViewSource.GetDefaultView(AllPlantings.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
                view.Filter = item => ((item as Planting).PlantingName.ToLower().Contains(searchText));
            else
                view.Filter = null;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tmpParkPlantings = _context.PlantingParks
                .Include(x => x.Planting)
                .Where(x => x.ParkId == currentPark.ParkId)
                .ToList();

            currentParkPlantings = tmpParkPlantings
                .Select(x => x.Planting)
                .ToList();

            CurrParkPlantings.ItemsSource = currentParkPlantings;
            allAvailablePlantings = _context.Plantings.ToList().Except(currentParkPlantings).ToList();
            AllPlantings.ItemsSource = allAvailablePlantings;

            foreach (Planting planting in currentParkPlantings)
            {
                TextBox textBoxCount = new TextBox();
                textBoxCount.Text = tmpParkPlantings.FirstOrDefault(x => x.PlantingId == planting.PlantingId)
                    .PlantingsNumber.ToString();
                textBoxCount.Height = 20;

                countTBs.Add(textBoxCount);
            }
            Refresh();
        }

        private void CountTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
                e.Handled = true;
        }
        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            if (AllPlantings.SelectedItem != null)
            {
                currentParkPlantings.Add(AllPlantings.SelectedItem as Planting);
                allAvailablePlantings.Remove(AllPlantings.SelectedItem as Planting);

                CurrParkPlantings.Items.Refresh();
                AllPlantings.Items.Refresh();

                TextBox textBoxCount = new TextBox();
                textBoxCount.Text = "1";
                textBoxCount.Height = 20;
                textBoxCount.PreviewTextInput += CountTB_PreviewTextInput;

                countTBs.Add(textBoxCount);

                Refresh();
            }
        }

        private void RemoveBTN_CLK(object sender, RoutedEventArgs e)
        {
            int indexToRemove = CurrParkPlantings.SelectedIndex;
            if (CurrParkPlantings.SelectedItem != null)
            {
                Planting selectedPlanting = CurrParkPlantings.SelectedItem as Planting;

                currentParkPlantings.Remove(selectedPlanting);
                allAvailablePlantings.Add(selectedPlanting);

                CurrParkPlantings.Items.Refresh();
                AllPlantings.Items.Refresh();

                
                CountTextBoxes.Children.RemoveAt(indexToRemove);
                countTBs.RemoveAt(indexToRemove);
            }
        }

        private void SaveBTN_CLK(object sender, RoutedEventArgs e)
        {
            _context.PlantingParks.RemoveRange(_context.PlantingParks.Where(x => x.ParkId == currentPark.ParkId));

            foreach (TextBox v in countTBs)
            {
                if (v.Text == "0" || string.IsNullOrEmpty(v.Text))
                {
                    MessageBox.Show("Количество должно быть больше 0.", "Ошибка", MessageBoxButton.OK);
                    return;
                }
            }
            foreach (Planting plantings in currentParkPlantings)
            {
                PlantingPark parkPlantings = new PlantingPark();
                parkPlantings.ParkId = currentPark.ParkId;
                parkPlantings.PlantingId = plantings.PlantingId;
                parkPlantings.PlantingsNumber = int.Parse(countTBs[currentParkPlantings.IndexOf(plantings)].Text);
                _context.PlantingParks.Add(parkPlantings);
            }
            _context.SaveChanges();
            MessageBox.Show("Все изменения сохранены успешно.", "", MessageBoxButton.OK);
        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
