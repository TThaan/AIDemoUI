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
        async Task OnOkButtonPressedAsync(NetParameters netParameters)
        {
            await Task.Run(() =>
            {
                new Initializer(netParameters).Run();
            });
        }
    }
}
