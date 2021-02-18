using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using System.Windows;

namespace AIDemoUI
{
    public partial class App : Application
    {
        private static IContainer Container { get; set; }
        // IServiceProvider ServiceProvider { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            #region ServiceProvider

            // var serviceCollection = new ServiceCollection();

            #endregion

            Container = DIContainerManager.Container;//
            // ServiceProvider = new AutofacServiceProvider(Container);

            #region Show

            using (var scope = Container.BeginLifetimeScope())
            {
                var mainWindowVM = scope.Resolve<IMainWindowVM>();
                MainWindow = new MainWindow() { DataContext = mainWindowVM };
                MainWindow.Show();
            }

            #endregion

        }
    }
}
