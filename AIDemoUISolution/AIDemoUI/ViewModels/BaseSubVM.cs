using System;
using System.Runtime.CompilerServices;

namespace AIDemoUI.ViewModels
{
    public class BaseSubVM : BaseVM, INotifySubViewModelChanged
    {
        #region fields & ctor

        // protected MainWindowVM _mainVM;

        public BaseSubVM()
        {
            // _mainVM = mainVM ?? throw new NullReferenceException($"{GetType().Name}.ctor");
        }

        #endregion

        #region public

        //public MainWindowVM MainVM 
        //{
        //    get { return _mainVM; }
        //    set { _mainVM = value; }
        //}

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
