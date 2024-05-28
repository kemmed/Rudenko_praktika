using Rudenko_praktika.Models;
using System;
using System.Collections.Generic;
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

namespace Rudenko_praktika.Windows.Add
{
    public partial class AddPlantingWin : Window
    {
        private readonly PraktikaContext _context;
        public AddPlantingWin(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
        }
        private void Life_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                e.Handled = true;
            else
            {
                TextBox textBox = sender as TextBox;
                string text = textBox.Text.Insert(textBox.CaretIndex, e.Text);
                string[] parts = text.Split('.');
                if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length > 1) || (parts.Length == 1 && parts[0].Length > 4))
                    e.Handled = true;
            }

        }
        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            TextBox[] obligatoryTBs = { NameTB, TypeTB, LifeTB };

            bool isEmpty = false;

            foreach (var textBox in obligatoryTBs)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.BorderBrush = Brushes.Red;
                    isEmpty = true;
                }
                else
                    textBox.BorderBrush = Brushes.Black;
            }
            if (!isEmpty)
            {
                Planting newPlanting = new Planting();
                newPlanting.PlantingName = NameTB.Text;
                newPlanting.CultureType = TypeTB.Text;
                newPlanting.AverageLifeExpectancy = decimal.Parse(LifeTB.Text.Replace('.',','));
                _context.Add(newPlanting);
                _context.SaveChanges();
                MessageBox.Show("Насаждение успешно добавлено");
                Close();
            }
        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
