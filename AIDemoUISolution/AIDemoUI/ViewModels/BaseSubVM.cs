using System;
using System.Runtime.CompilerServices;

namespace AIDemoUI.ViewModels
{
    public class BaseSubVM : BaseVM, INotifySubViewModelChanged
    {
        #region fields & ctor

        protected readonly MainWindowVM _mainVM;

        public BaseSubVM(MainWindowVM mainVM)
        {
            _mainVM = mainVM ?? throw new NullReferenceException($"{GetType().Name}.ctor");
        }

        #endregion

        #region INotifySubViewModelChanged

        public event SubViewModelChangedEventHandler SubViewModelChanged;
        public void OnSubViewModelChanged([CallerMemberName] string propertyName = null)
        {
            SubViewModelChanged?.Invoke(this, new SubViewModelChangedEventArgs(propertyName));
        }

        #endregion
    }
}
