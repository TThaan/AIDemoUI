using AIDemoUI.Commands;
using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.SampleData;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using DeepLearningDataProvider;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.ObjectModel;
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
            // serviceCollection.AddScoped(typeof(ISessionContext), typeof(SessionContext));

            #endregion

            Container = DIContainerManager.Container;//
            // ServiceProvider = new AutofacServiceProvider(Container);

            #region Show

            using (var scope = Container.BeginLifetimeScope())
            {
                var mainWindowVM = scope.Resolve<MainWindowVM>();
                MainWindow = new MainWindow() { DataContext = mainWindowVM };
                MainWindow.Show();
            }

            #endregion

        }
    }
}
