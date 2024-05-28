using Rudenko_praktika.Models;
using Rudenko_praktika.Pages;
using Rudenko_praktika.Windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rudenko_praktika
{
    public partial class MainWin : Window
    {
        private readonly PraktikaContext _context;
        bool closeFlag = true;
        public MainWin(PraktikaContext context)
        {
            InitializeComponent();
            _context = context;
            if (App.logInUser.Role.RoleName == "Ландшафтный дизайнер")
            {
                FountainTI.Visibility = Visibility.Collapsed;
                PavilionTI.Visibility = Visibility.Collapsed;
                UserTI.Visibility = Visibility.Collapsed;
            }
            else if (App.logInUser.Role.RoleName == "Инженер по водоснабжению") 
            { 
                PavilionTI.Visibility = Visibility.Collapsed;
                PlantingTI.Visibility = Visibility.Collapsed;
                UserTI.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(closeFlag)
                Environment.Exit(0);
        }
        private void ParksFrame_Loaded(object sender, RoutedEventArgs e)
        {
            ParksFrame.Navigate(new ParkPage(_context));
        }
        private void FountainsFrame_Loaded(object sender, RoutedEventArgs e)
        {
            FountainsFrame.Navigate(new FountainPage(_context));
        }
        private void PavilionsFrame_Loaded(object sender, RoutedEventArgs e)
        {
            PavilionsFrame.Navigate(new PavilionPage(_context));
        }
        private void PlantingsFrame_Loaded(object sender, RoutedEventArgs e)
        {
            PlantinsFrame.Navigate(new PlantingPage(_context));
        }
        private void UsersFrame_Loaded(object sender, RoutedEventArgs e)
        {
            UsersFrame.Navigate(new UserPage(_context));
        }

        private void CloseBTN_CLK(object sender, RoutedEventArgs e)
        {
            AuthorizationWin authorizationWin = App.Current.Windows.OfType<AuthorizationWin>().FirstOrDefault();
            authorizationWin.Show();
            closeFlag = false;
            Close();
        }       
    }
}