using AIDemoUI.Views;
using Autofac;
using System.Windows;

namespace AIDemoUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IContainer Container = new DIManager().Container;
            using (var scope = Container.BeginLifetimeScope())
            {
                MainWindow = scope.Resolve<MainWindow>();
                MainWindow.Show();
                scope.Resolve<SampleImportWindow>().Owner = MainWindow;
            }
        }
    }
}
