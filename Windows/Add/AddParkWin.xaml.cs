using Rudenko_praktika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class AddParkWin : Window
    {
        private readonly PraktikaContext _context;
        public AddParkWin(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
            OpenTB.Text = "00:00";
            CloseTB.Text = "00:00";
        }
        private void PhoneTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string input = textBox.Text;
                string digitsOnly = new string(input.Where(char.IsDigit).ToArray());

                if (digitsOnly.Length > 0)
                {
                    string formattedNumber = string.Format("({0}){1}-{2}-{3}",
                        GetSafeSubstring(digitsOnly, 0, 3),
                        GetSafeSubstring(digitsOnly, 3, 3),
                        GetSafeSubstring(digitsOnly, 6, 2),
                        GetSafeSubstring(digitsOnly, 8, 2));

                    textBox.TextChanged -= PhoneTB_TextChanged;
                    textBox.Text = formattedNumber;
                    textBox.CaretIndex = textBox.Text.Length;
                    textBox.TextChanged += PhoneTB_TextChanged;
                }
            }
        }
        private string GetSafeSubstring(string str, int startIndex, int length)
        {
            if (str.Length >= startIndex + length)
                return str.Substring(startIndex, length);
            else if (str.Length > startIndex)
                return str.Substring(startIndex);
            else
                return string.Empty;
        }
        private void TimeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ":")
                e.Handled = true;
            else
            {
                TextBox textBox = sender as TextBox;
                string text = textBox.Text.Insert(textBox.CaretIndex, e.Text);
                string[] parts = text.Split(':');

                if (parts.Length > 2 ||
                    (parts.Length == 2 && (parts[0].Length > 2 || parts[1].Length > 2)) ||
                    (parts.Length == 1 && parts[0].Length > 2))
                    e.Handled = true;
            }
        }
        private void IndexTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!char.IsDigit(e.Text, 0) || textBox.Text.Length >= 6)
                e.Handled = true;
        }
        private void Area_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
                e.Handled = true;
            else
            {
                TextBox textBox = sender as TextBox;
                string text = textBox.Text.Insert(textBox.CaretIndex, e.Text);
                string[] parts = text.Split('.');
                if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length > 2) || (parts.Length == 1 && parts[0].Length > 6))
                    e.Handled = true;
            }
        }

        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            TextBox[] obligatoryTBs = { NameTB, AreaTB, CityTB, IndexTB };

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

            if (string.IsNullOrWhiteSpace(CloseTB.Text) != string.IsNullOrWhiteSpace(OpenTB.Text))
            {
                if (string.IsNullOrWhiteSpace(CloseTB.Text))
                {
                    CloseTB.BorderBrush = Brushes.Red;
                    OpenTB.BorderBrush = Brushes.Black;
                }
                else
                {
                    OpenTB.BorderBrush = Brushes.Red;
                    CloseTB.BorderBrush = Brushes.Black;
                }
                isEmpty = true;
            }
            else
            {
                OpenTB.BorderBrush = Brushes.Black;
                CloseTB.BorderBrush = Brushes.Black;
            }
            if(TimeOnly.Parse(CloseTB.Text) <= TimeOnly.Parse(OpenTB.Text)
                && CloseTB.Text != "00:00"
                && OpenTB.Text != "00:00")
            {
                OpenTB.BorderBrush = Brushes.Red;
                CloseTB.BorderBrush = Brushes.Red;
                isEmpty = true;
            }
            else
            {
                OpenTB.BorderBrush = Brushes.Black;
                CloseTB.BorderBrush = Brushes.Black;
            }

            bool isValid = true;
            if (!isEmpty)
            {
                Park newPark = new Park();
                newPark.ParkName = NameTB.Text;
                newPark.PlantingDensity = PlantingDensityCB.Text;
                newPark.ParkArea = decimal.Parse(AreaTB.Text.Replace('.', ','));
                newPark.ParkOpening = TimeOnly.Parse(OpenTB.Text);
                newPark.ParkClosure = TimeOnly.Parse(CloseTB.Text);
                newPark.City = CityTB.Text;
                if (!string.IsNullOrWhiteSpace(StreetTB.Text))
                    newPark.Street = StreetTB.Text;

                if (!string.IsNullOrWhiteSpace(BuildingTB.Text))
                    newPark.Building = BuildingTB.Text;

                Regex regex = new Regex(@"\(\d{3}\)\d{3}-\d{2}-\d{2}");
                if (!regex.IsMatch(PhoneTB.Text) && !string.IsNullOrWhiteSpace(PhoneTB.Text))
                {
                    MessageBox.Show("Номер телефона должен соответствовать формату (999)999-99-99", "Ошибка");
                    isValid = false;
                }
                if (!string.IsNullOrWhiteSpace(PhoneTB.Text))
                    newPark.PhoneNumber = PhoneTB.Text;

                if (!string.IsNullOrWhiteSpace(WebsiteTB.Text))
                    newPark.Website = WebsiteTB.Text;
                if (IndexTB.Text.Length < 6  || int.Parse(IndexTB.Text) < 100000)
                {
                    MessageBox.Show("Введите индекс от 100000 до 999999", "Ошибка");
                    isValid = false;
                }
                newPark.Index = int.Parse(IndexTB.Text);

                _context.Parks.Add(newPark);
                if (isValid)
                {
                    _context.SaveChanges();
                    MessageBox.Show("Парк успешно добавлен");
                    Close();
                }
            }
        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
