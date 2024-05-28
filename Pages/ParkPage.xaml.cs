using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.VisualBasic.ApplicationServices;
using OfficeOpenXml;
using Rudenko_praktika.Models;
using Rudenko_praktika.Windows.Add;
using Rudenko_praktika.Windows.Edit;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.MessageBox;
using Page = System.Windows.Controls.Page;
using TextBox = System.Windows.Controls.TextBox;

namespace Rudenko_praktika.Pages
{
    /// <summary>
    /// Логика взаимодействия для ParkPage.xaml
    /// </summary>
    public partial class ParkPage : Page
    {
        private readonly PraktikaContext _context;
        public ParkPage(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
            if (App.logInUser.Role.RoleName == "Ландшафтный дизайнер")
            {
                AddBTN.Visibility = Visibility.Collapsed;
                ExcelBTN.Visibility = Visibility.Collapsed;
                EditMI.Visibility = Visibility.Collapsed;
                DeleteMI.Visibility = Visibility.Collapsed;
            }
            else if (App.logInUser.Role.RoleName == "Инженер по водоснабжению")
            {
                AddBTN.Visibility = Visibility.Collapsed;
                ExcelBTN.Visibility = Visibility.Collapsed;
                contextMenu.Visibility = Visibility.Hidden;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ParkList.ItemsSource = _context.Parks.ToList();
        }
        private void Page_Activated(object sender, RoutedEventArgs e)
        {
            ParkList.ItemsSource = _context.Parks.ToList();
        }
        private void EditMI_CLK(object sender, RoutedEventArgs e)
        {
            EditParkWin editWin = App.Current.Windows.OfType<EditParkWin>().FirstOrDefault();

            if (editWin == null || editWin.Visibility != Visibility.Visible)
            {
                editWin = new EditParkWin(_context, (Park)ParkList.SelectedItem);
                editWin.ShowDialog();
                ParkList.ItemsSource = _context.Parks.ToList();
            }
        }
        private void MoreMI_CLK(object sender, RoutedEventArgs e)
        {
            EditPlantingParkWin plantingParkWin = App.Current.Windows.OfType<EditPlantingParkWin>().FirstOrDefault();

            if (plantingParkWin == null || plantingParkWin.Visibility != Visibility.Visible)
            {
                plantingParkWin = new EditPlantingParkWin(_context, (Park)ParkList.SelectedItem);
                plantingParkWin.ShowDialog();
                ParkList.ItemsSource = _context.Parks.ToList();
            }
        }
        private void DeleteMI_CLK(object sender, RoutedEventArgs e)
        {
            var deletePark = (Park)ParkList.SelectedItem;
            var result = MessageBox.Show("Вы уверены, что хотите безвозвратно удалить парк?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (deletePark != null)
                {
                    if (!_context.Fountains.Any(entity => entity.ParkId == deletePark.ParkId)
                        && !_context.PlantingParks.Any(entity => entity.ParkId == deletePark.ParkId)
                        && !_context.Pavilions.Any(entity => entity.ParkId == deletePark.ParkId))
                    {
                        _context.Parks.Remove(deletePark);
                        _context.SaveChanges();
                    }
                    else
                        MessageBox.Show("Нельзя удалить этот парк, так как на него ссылаются другие объекты.");
                }
            }
            ParkList.ItemsSource = _context.Parks.ToList();
        }
        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            AddParkWin addWin = App.Current.Windows.OfType<AddParkWin>().FirstOrDefault();

            if (addWin == null || addWin.Visibility != Visibility.Visible)
            {
                addWin = new AddParkWin(_context);
                addWin.ShowDialog();
                ParkList.ItemsSource = _context.Parks.ToList();
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();
            ICollectionView view = CollectionViewSource.GetDefaultView(ParkList.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
                view.Filter = item => ((item as Park).ParkName.ToLower().Contains(searchText));
            else
                view.Filter = null;
        }
        private void ExcelBTN_CLK(object sender, RoutedEventArgs e)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                for (int i = 0; i < (ParkList.View as GridView).Columns.Count; i++)
                    worksheet.Cells[1, i + 1].Value = (ParkList.View as GridView).Columns[i].Header.ToString();

                for (int i = 0; i < ParkList.Items.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = (ParkList.Items[i] as Park).ParkName;
                    worksheet.Cells[i + 2, 2].Value = (ParkList.Items[i] as Park).ParkArea;
                    worksheet.Cells[i + 2, 3].Value = (ParkList.Items[i] as Park).PlantingDensity;
                    worksheet.Cells[i + 2, 4].Value = (ParkList.Items[i] as Park).Schedule;
                    worksheet.Cells[i + 2, 5].Value = (ParkList.Items[i] as Park).Address;
                    worksheet.Cells[i + 2, 6].Value = (ParkList.Items[i] as Park).PhoneNumber;
                    worksheet.Cells[i + 2, 7].Value = (ParkList.Items[i] as Park).Website;
                }

                string directory = AppDomain.CurrentDomain.BaseDirectory;
                for (int i = 0; i < 4; i++)
                    directory = Directory.GetParent(directory).FullName;

                string fileName = "ParkReport.xls";
                string filePath = Path.Combine(directory, "Reports", fileName);

                FileInfo excelFile = new FileInfo(filePath);
                excelPackage.SaveAs(excelFile);

                var result = MessageBox.Show($"Данные успешно экспортированы в {filePath}");
            }
        }

        private void ParkList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var totalAvailableWidth = ParkList.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var columns = (ParkList.View as GridView).Columns;

            columns[0].Width = totalAvailableWidth * 0.14;
            columns[1].Width = totalAvailableWidth * 0.07;
            columns[2].Width = totalAvailableWidth * 0.14;
            columns[3].Width = totalAvailableWidth * 0.14;
            columns[4].Width = totalAvailableWidth * 0.23;
            columns[5].Width = totalAvailableWidth * 0.14;
            columns[6].Width = totalAvailableWidth * 0.14;
        }
    }
}
