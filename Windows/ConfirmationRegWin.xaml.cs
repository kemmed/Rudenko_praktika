using Microsoft.VisualBasic.ApplicationServices;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rudenko_praktika.Windows
{
    public partial class ConfirmationRegWin : Window
    {
        private readonly PraktikaContext _context;
        Models.User newUser;
        public ConfirmationRegWin(PraktikaContext context, Models.User newUser)
        {
            InitializeComponent();
            _context = context;
            this.newUser= newUser;
            RoleCB.ItemsSource = _context.Roles.Where(r => r.RoleName != "Администратор").ToList();
        }

        private void SaveBTN_CLK(object sender, RoutedEventArgs e)
        {
            string role = "Администратор";
            string pass = UserProcessings.GetPasswordHash(PasswordB.Password);

            var user = _context.Users.FirstOrDefault(u => u.Role.RoleName == role && u.Password.Equals(pass));

            if (user != null)
            {
                newUser.RoleId = (RoleCB.SelectedItem as Role).RoleId;
                ErrorPassTB.Visibility = Visibility.Hidden;
                _context.Users.Add(newUser);
                _context.SaveChanges();
                AuthorizationWin authorizationWin = App.Current.Windows.OfType<AuthorizationWin>().FirstOrDefault();
                authorizationWin.Show();
                Close();

            }
            else
                ErrorPassTB.Visibility = Visibility.Visible;
        }
    }
}
