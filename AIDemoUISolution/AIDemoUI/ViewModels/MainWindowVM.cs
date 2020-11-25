using FourPixCam;
using System;
using System.Threading.Tasks;

namespace AIDemoUI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        public MainWindowVM(NetParametersVM netParametersVM)
        {
            NetParametersVM = netParametersVM ?? 
                throw new NullReferenceException($"{GetType().Name}.ctor");
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
