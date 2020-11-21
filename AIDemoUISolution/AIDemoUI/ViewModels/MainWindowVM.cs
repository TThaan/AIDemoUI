using System;

namespace AIDemoUI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        public MainWindowVM(NetParametersVM netParametersVM)
        {
            NetParametersVM = netParametersVM ?? 
                throw new NullReferenceException($"{GetType().Name}.ctor");
        }

        public NetParametersVM NetParametersVM { get; }
    }
}
