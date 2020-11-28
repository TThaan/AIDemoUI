using FourPixCam;
using System.IO;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        public MainWindowVM()
        {
            NetParametersVM = new NetParametersVM();
            NetParametersVM.OkBtnPressed += OnOkButtonPressedAsync;
        }

        public NetParametersVM NetParametersVM { get; }
        async Task OnOkButtonPressedAsync(NetParameters netParameters, Stream trainingData, Stream testingData)
        {
            await Task.Run(() =>
            {
                new Initializer(netParameters).Run(trainingData, testingData);
            });
        }
    }
}
