using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using FourPixCam;
using System.Windows;

namespace AIDemoUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            #region Viewmodels and MainView

            NetParametersVM netParametersVM = new NetParametersVM(new NetParameters());
            MainWindowVM mainWindowVM = new MainWindowVM(netParametersVM);

            MainWindow = new MainWindow() { DataContext = mainWindowVM };
            MainWindow.Show();

            #endregion
        }
    }
}
