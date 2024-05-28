using Rudenko_praktika.Helper;
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
    public partial class AddUserWin : Window
    {
        private readonly PraktikaContext _context;
        public AddUserWin(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
            RoleCB.ItemsSource = _context.Roles.Where(r => r.RoleName != "Администратор").ToList();
        }

        private void AddBTN_CLK(object sender, RoutedEventArgs e)
        {
            var userList = _context.Users.ToList();
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].UserLogin == LoginTB.Text)
                {
                    MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка", MessageBoxButton.OK);
                    return;
                }
            }
            bool isEmpty = false;
            if (string.IsNullOrEmpty(LoginTB.Text))
            {
                LoginTB.BorderBrush = Brushes.Red;
                isEmpty = true;
            }
            else
                LoginTB.BorderBrush = Brushes.Black;
            
            if (!string.IsNullOrEmpty(PasswordB.Password)
                && !string.IsNullOrEmpty(PasswordB2.Password))
            {
                PasswordB.BorderBrush = Brushes.Black;
                PasswordB2.BorderBrush = Brushes.Black;
            }
            else
            {
                isEmpty = true;

                if (string.IsNullOrEmpty(PasswordB.Password))
                    PasswordB.BorderBrush = Brushes.Red;
                else
                    PasswordB.BorderBrush = Brushes.Black;

                if (string.IsNullOrEmpty(PasswordB2.Password))
                    PasswordB2.BorderBrush = Brushes.Red;
                else
                    PasswordB2.BorderBrush = Brushes.Black;
            }

            if (isEmpty)
                return;

            if (PasswordB.Password.Length < 6
                || !PasswordB.Password.Any(char.IsUpper)
                || !PasswordB.Password.Any(char.IsDigit)
                || !PasswordB.Password.Any(c => "!@#$%^".Contains(c)))
            {
                MessageBox.Show(@"Пароль должен быть не менее 6 символов, содержать минимум 1 прописную и 1 цифру,
                                  а также хотябы один символ из набора ! @ # $ % ^",
                                  "Пароль не соответствует требованиям", MessageBoxButton.OK);
                return;
            }

            if (PasswordB.Password != PasswordB2.Password)
            {
                ErrorPassTB.Visibility = Visibility.Visible;
                return;
            }
            else
                ErrorPassTB.Visibility = Visibility.Hidden;
            User newUser = new User();
            newUser.Password = UserProcessings.GetPasswordHash(PasswordB.Password);
            newUser.UserLogin = LoginTB.Text;
            newUser.RoleId = (RoleCB.SelectedItem as Role).RoleId;
            _context.Add(newUser);
            _context.SaveChanges();
            Close();
        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
