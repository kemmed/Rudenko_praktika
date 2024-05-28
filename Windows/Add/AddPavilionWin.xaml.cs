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
    public partial class AddPavilionWin : Window
    {
        private readonly PraktikaContext _context;
        public AddPavilionWin(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
            ParkCB.ItemsSource = _context.Parks.ToList();
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
        private void AddTN_CLK(object sender, RoutedEventArgs e)
        {
            TextBox[] obligatoryTBs = { NameTB, AreaTB};

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
                Pavilion newPavilion = new Pavilion();
                newPavilion.PavilionName = NameTB.Text;
                newPavilion.PavilionArea = decimal.Parse(AreaTB.Text.Replace('.',','));
                newPavilion.PavilionType = TypeCB.Text;
                newPavilion.ParkId = (ParkCB.SelectedItem as Park).ParkId;
                _context.Add(newPavilion);
                _context.SaveChanges();
                MessageBox.Show("Павильон успешно добавлен");
                Close();
            }

        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
