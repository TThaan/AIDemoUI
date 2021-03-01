using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using System;
using System.Windows;

namespace AIDemoUI
{
    public partial class App : Application
    {
        internal static IContainer Container { get; set; }
        // IServiceProvider ServiceProvider { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            #region ServiceProvider

            // var serviceCollection = new ServiceCollection();

            #endregion

            Container = new DIManager().Container;//
            // ServiceProvider = new AutofacServiceProvider(Container);

            #region Show

            using (var scope = Container.BeginLifetimeScope())
            {
                // var mainWindowVM = scope.Resolve<DefaultValuesInitializer>().DefaultMainWindowVM;
                var mainWindowVM = scope.Resolve<IMainWindowVM>();
                new DefaultValues().Set(mainWindowVM, scope.Resolve<ISessionContext>());
                MainWindow = new MainWindow() { DataContext = mainWindowVM };
                MainWindow.Show();
            }

            #endregion

        }
    }
}
