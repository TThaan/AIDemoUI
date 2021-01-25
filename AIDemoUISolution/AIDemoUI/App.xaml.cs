using AIDemoUI.Views;
using System.Windows;

namespace AIDemoUI
{
    public partial class App : Application
    {        
        // redundant
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}
