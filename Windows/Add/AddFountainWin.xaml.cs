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
    public partial class AddFountainWin : Window
    {
        private readonly PraktikaContext _context;
        public AddFountainWin(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
            ParkCB.ItemsSource=_context.Parks.ToList();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                e.Handled = true;
            else
            {
                TextBox textBox = sender as TextBox;
                string text = textBox.Text.Insert(textBox.CaretIndex, e.Text);
                string[] parts = text.Split('.');
                if (parts.Length > 2 
                    || (parts.Length == 2 && parts[1].Length > 2) 
                    || (parts.Length == 1 && parts[0].Length > 6))
                    e.Handled = true;
            }
        }
        private void CipherTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!char.IsDigit(e.Text, 0) || textBox.Text.Length >= 8)
                e.Handled = true;
        }
        private void Date_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                e.Handled = true;
        }

        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            Fountain newFountain = new Fountain();
            TextBox[] obligatoryTBs = { CipherTB, AreaTB, NormTB, MaxTB };

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
            if (_context.Fountains.Any(x => x.FountainCipher == int.Parse(CipherTB.Text)))
            {
                MessageBox.Show("Фонтан с таким шифром уже существует.", "Ошибка", MessageBoxButton.OK);
                isEmpty = true;
            }

            else
                datePicker.BorderBrush = Brushes.Black;

            if (!isEmpty)
            {
                if (string.IsNullOrWhiteSpace(datePicker.Text))
                    newFountain.ConstructionDate = null;
                else if (datePicker.SelectedDate.Value > DateTime.Now)
                {
                    datePicker.BorderBrush = Brushes.Red;
                    return;
                }
                else
                    newFountain.ConstructionDate = datePicker.SelectedDate.Value;

                newFountain.FountainCipher = int.Parse(CipherTB.Text);
                newFountain.FountainArea = decimal.Parse(AreaTB.Text.Replace('.',','));
                newFountain.NormalWaterConsumption = decimal.Parse(NormTB.Text.Replace('.',','));
                newFountain.MaximumWaterConsumption = decimal.Parse(MaxTB.Text.Replace('.',','));
                newFountain.ParkId = (ParkCB.SelectedItem as Park).ParkId;
                _context.Add(newFountain);
                _context.SaveChanges();
                MessageBox.Show("Фонтан успешно добавлен");
                Close();
            }
        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

