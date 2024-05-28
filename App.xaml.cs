using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using Rudenko_praktika.Models;
using Rudenko_praktika.Windows;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Rudenko_praktika
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public static User logInUser = null;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PraktikaContext>(options =>
            options.UseSqlServer("Data Source=KEMMED\\SQLEXPRESS;" +
                                    "Initial Catalog=paktika;" +
                                    "Integrated Security=True;" +
                                    "Connect Timeout=30;" +
                                    "Encrypt=False;"));

            services.AddSingleton<AuthorizationWin>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _serviceProvider.GetRequiredService<AuthorizationWin>().Show();
        }
    }
}
