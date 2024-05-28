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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private readonly PraktikaContext _context;
        public UserPage(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           UserList.ItemsSource = _context.Users.Include(x => x.Role).ToList();
        }
        private void EditMI_CLK(object sender, RoutedEventArgs e)
        {
            EditUserWin editWin = App.Current.Windows.OfType<EditUserWin>().FirstOrDefault();

            if (editWin == null || editWin.Visibility != Visibility.Visible)
            {
                editWin = new EditUserWin(_context, (User)UserList.SelectedItem);
                editWin.ShowDialog();
                UserList.ItemsSource = _context.Users.ToList();
            }
        }
        private void DeleteMI_CLK(object sender, RoutedEventArgs e)
        {
            var deleteUser = (User)UserList.SelectedItem;
            var result = MessageBox.Show("Вы уверены, что хотите безвозвратно удалить пользователя?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (deleteUser != null)
                {
                    if (deleteUser.Role.RoleName!="Администратор")
                    {
                        _context.Users.Remove(deleteUser);
                        _context.SaveChanges();
                    }
                    else
                        MessageBox.Show("Невозможно удалить администратора.");
                }
            }
            UserList.ItemsSource = _context.Users.ToList();
        }
        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            AddUserWin addWin = App.Current.Windows.OfType<AddUserWin>().FirstOrDefault();

            if (addWin == null || addWin.Visibility != Visibility.Visible)
            {
                addWin = new AddUserWin(_context);
                addWin.ShowDialog();
                UserList.ItemsSource = _context.Users.ToList();
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();
            ICollectionView view = CollectionViewSource.GetDefaultView(UserList.ItemsSource);

            if (!string.IsNullOrEmpty(searchText))
                view.Filter = item => ((item as User).UserLogin.ToLower().Contains(searchText));
            else
                view.Filter = null;
        }
        private void UserList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var totalAvailableWidth = UserList.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            var columns = (UserList.View as GridView).Columns;

            columns[0].Width = totalAvailableWidth * 0.5;
            columns[1].Width = totalAvailableWidth * 0.5;
        }
    }
}
