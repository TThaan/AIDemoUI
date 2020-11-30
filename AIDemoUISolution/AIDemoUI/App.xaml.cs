using AIDemoUI.Views;
using System.Windows;

namespace AIDemoUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            #region Viewmodels and MainView

            #region test:

            // Sample[] samples_4Pix = NNet_InputProvider.FourPixCam.DataFactory.GetTrainingSamples(20, .2f, .3f);
            // string json_4Pix = samples_4Pix.ToJson();
            // bool b_4Pix = samples_4Pix.Save(@"C:\temp\FourPixCamTrainingData_distorted.txt", StorageFormat.Json);

            // Sample[] samples_Mnist = NNet_InputProvider.MNIST.DataFactory.GetTrainingSamples(0, 0);
            // string string_Mnist = samples_Mnist.First().ToJson();
            // bool b_Mnist = samples_Mnist.Save(@"C:\temp\MNISTTrainingData_distorted.txt", StorageFormat.Json);

            #endregion

            MainWindow = new MainWindow();
            MainWindow.Show();

            #endregion
        }
    }
}
