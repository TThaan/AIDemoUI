using FourPixCam;
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

        async Task OnOkButtonPressedAsync(NetParameters netParameters, bool TurnBased, 
            string Url_TrainingLabels, string Url_TrainingImages, string Url_TestingLabels, string Url_TestingImages)
        {
            await Task.Run(() =>
            {
                if (TurnBased)
                {
                    new Initializer(netParameters).RunTurnBased(Url_TrainingLabels, Url_TrainingImages, Url_TestingLabels, Url_TestingImages);
                }
                else
                { 
                    new Initializer(netParameters).Run(Url_TrainingLabels, Url_TrainingImages, Url_TestingLabels, Url_TestingImages); 
                }
            });
        }
    }
}
