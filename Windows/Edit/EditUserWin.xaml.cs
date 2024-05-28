using Rudenko_praktika.Models;
using Rudenko_praktika.Windows.Add;
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

namespace Rudenko_praktika.Windows.Edit
{
    public partial class EditUserWin : Window
    {
        private readonly PraktikaContext _context;
        private User currentUser;
        bool isNotSaved = true;
        public EditUserWin(PraktikaContext context, User currentUser)
        {
            _context = context;
            this.currentUser = currentUser;
            InitializeComponent();
            this.DataContext = currentUser;
            RoleCB.ItemsSource= _context.Roles.Where(r => r.RoleName != "Администратор").ToList();
            if (currentUser.Role.RoleName == "Администратор")
            {
                RoleCB.ItemsSource = _context.Roles.ToList();
                RoleCB.IsEnabled = false;
                RoleCB.SelectedItem = currentUser.Role;
            }    
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (isNotSaved)
            {
                _context.Entry(currentUser).Reload();
                Close();
            }
        }
        private void ResetPassBTN_CLK(object sender, RoutedEventArgs e)
        {
            ResetPasswordWin resetWin = App.Current.Windows.OfType<ResetPasswordWin>().FirstOrDefault();

            if (resetWin == null || resetWin.Visibility != Visibility.Visible)
            {
                resetWin = new ResetPasswordWin(_context, currentUser);
                resetWin.Show();
            }
        }
        private void SaveBTN_CLK(object sender, RoutedEventArgs e)
        {
            var userList = _context.Users.Where(u => u.UserId!=currentUser.UserId).ToList();
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].UserLogin == LoginTB.Text)
                {
                    MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка", MessageBoxButton.OK);
                    return;
                }
            }
            if (string.IsNullOrEmpty(LoginTB.Text))
            {
                LoginTB.BorderBrush=Brushes.Red;
                return;
            }
            else
                LoginTB.BorderBrush = Brushes.Black;

            _context.SaveChanges();
            isNotSaved = false;
            Close();
        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
