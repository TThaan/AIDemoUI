using FourPixCam;
using NNet_InputProvider;
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

        async Task OnOkButtonPressedAsync(NetParameters netParameters, bool TurnBased, SampleSetParameters sampleSetParameters)
        {
            await Task.Run(() =>
            {
                if (TurnBased)
                {
                    Initializer.RunTurnBased(netParameters, sampleSetParameters);
                }
                else
                { 
                    Initializer.Run(netParameters, sampleSetParameters); 
                }
            });
        }
    }
}
