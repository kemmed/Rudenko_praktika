using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.Logging;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rudenko_praktika.Windows
{
    public partial class AuthorizationWin : Window
    {
        private readonly PraktikaContext _context;
        public string log;
        public string pass;
        public AuthorizationWin(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void АuthorizationBTN_CLK(object sender, RoutedEventArgs e)
        {
            log = LoginTB.Text;
            pass = UserProcessings.GetPasswordHash(PasswordTB.Password);

            var user = _context.Users.Include(x=>x.Role).FirstOrDefault(u => u.UserLogin == log && u.Password.Equals(pass));

            if (user != null)
            {
                App.logInUser = user;
                errorMessageTB.Visibility = Visibility.Hidden;
                Hide();
                MainWin mainWin = new MainWin(_context);
                mainWin.Show();

            }
            else
                errorMessageTB.Visibility = Visibility.Visible;
        }
        private void RregistrationBTN_CLK(object sender, RoutedEventArgs e)
        {
            RegistrationWin RegWin = App.Current.Windows.OfType<RegistrationWin>().FirstOrDefault();

            if (RegWin == null || RegWin.Visibility != Visibility.Visible)
            {
                RegWin = new RegistrationWin(_context);
                Hide();
                RegWin.Show();
            }
        }
    }
}
