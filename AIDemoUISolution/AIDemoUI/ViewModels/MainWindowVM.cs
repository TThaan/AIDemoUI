using FourPixCam;
using System;

namespace AIDemoUI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        public MainWindowVM(NetParametersVM netParametersVM)
        {
            NetParametersVM = netParametersVM ?? 
                throw new NullReferenceException($"{GetType().Name}.ctor");
            NetParametersVM.OkBtnPressed += OnOkButtonPressed;
        }

        public NetParametersVM NetParametersVM { get; }
        void OnOkButtonPressed(NetParameters netParameters)
        {
            new Initializer(netParameters).Run();
        }
    }
}
