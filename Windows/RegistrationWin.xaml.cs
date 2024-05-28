using DocumentFormat.OpenXml.Drawing.Diagrams;
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

namespace Rudenko_praktika.Windows
{
    public partial class RegistrationWin : Window
    {
        private readonly PraktikaContext _context;
        public RegistrationWin(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            AuthorizationWin authorizationWin = App.Current.Windows.OfType<AuthorizationWin>().FirstOrDefault();
            authorizationWin.Show();
            Close();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            AuthorizationWin authorizationWin = App.Current.Windows.OfType<AuthorizationWin>().FirstOrDefault();
            authorizationWin.Show();
            Close();
        }

        private void SignUpBTN_CLK(object sender, RoutedEventArgs e)
        {
            bool isEmpty = false;
            if (!string.IsNullOrEmpty(LoginTB.Text)
                && !string.IsNullOrEmpty(PasswordB.Password)
                && !string.IsNullOrEmpty(PasswordB2.Password))
            {
                LoginTB.BorderBrush = Brushes.Black;
                PasswordB.BorderBrush = Brushes.Black;
                PasswordB2.BorderBrush = Brushes.Black;
            }
            else
            {
                isEmpty = true;
                if (string.IsNullOrEmpty(LoginTB.Text))
                {
                    LoginTB.BorderBrush = Brushes.Red;
                    isEmpty = false;
                }
                else
                    LoginTB.BorderBrush = Brushes.Black;

                if (string.IsNullOrEmpty(PasswordB.Password))
                {
                    PasswordB.BorderBrush = Brushes.Red;
                    isEmpty = false;
                }
                else
                    PasswordB.BorderBrush = Brushes.Black;

                if (string.IsNullOrEmpty(PasswordB2.Password))
                {
                    PasswordB2.BorderBrush = Brushes.Red;
                    isEmpty = false;
                }
                else
                    PasswordB2.BorderBrush = Brushes.Black;
            }

            if (isEmpty)
                return;

            var userList = _context.Users.ToList();
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].UserLogin == LoginTB.Text)
                {
                    MessageBox.Show("Пользователь с таким именем уже существует.", "Ошибка", MessageBoxButton.OK);
                    return;
                }
            }

            if (PasswordB.Password.Length < 6
                || !PasswordB.Password.Any(char.IsUpper)
                || !PasswordB.Password.Any(char.IsDigit)
                || !PasswordB.Password.Any(c => "!@#$%^".Contains(c))) {
                MessageBox.Show(@"Пароль должен быть не менее 6 символов, содержать минимум 1 прописную и 1 цифру,
                                а также хотябы один символ из набора ! @ # $ % ^",
                                "Пароль не соответствует требованиям", MessageBoxButton.OK);
                return;
            }

            if (PasswordB.Password!= PasswordB2.Password)
            {
                ErrorPassTB.Visibility= Visibility.Visible;
                return;
            }
            else
                ErrorPassTB.Visibility = Visibility.Hidden;

            Models.User newUser = new User();
            newUser.Password = UserProcessings.GetPasswordHash(PasswordB.Password);
            newUser.UserLogin = LoginTB.Text;
            ConfirmationRegWin ConfRegWin = App.Current.Windows.OfType<ConfirmationRegWin>().FirstOrDefault();

            if (ConfRegWin == null || ConfRegWin.Visibility != Visibility.Visible)
            {
                ConfRegWin = new ConfirmationRegWin(_context, newUser);
                ConfRegWin.Show();
                Close();
            }
        }
    }
}
