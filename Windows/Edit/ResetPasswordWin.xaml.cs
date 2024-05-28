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
    public partial class ResetPasswordWin : Window
    {
        private readonly PraktikaContext _context;
        private User currentUser;
        public ResetPasswordWin(PraktikaContext context, User currentUser)
        {
            InitializeComponent();
            _context = context;
            this.currentUser= currentUser;
        }
        
        private void SaveBTN_CLK(object sender, RoutedEventArgs e)
        {
            bool isEmpty = false;
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
            currentUser.Password = UserProcessings.GetPasswordHash(PasswordB.Password);
            _context.SaveChanges();
            Close();
        }
        private void CancelBTN_CLK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
