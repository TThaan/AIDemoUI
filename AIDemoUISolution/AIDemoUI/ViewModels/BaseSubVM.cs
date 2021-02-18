using System;
using System.Runtime.CompilerServices;

namespace AIDemoUI.ViewModels
{
    /// <summary>
    /// (Make) Redundant?
    /// </summary>
    public class BaseSubVM : BaseVM, INotifySubViewModelChanged
    {
        #region fields & ctor

        //protected readonly ISessionContext _sessionContext;

        public BaseSubVM(ISimpleMediator mediator)//ISessionContext sessionContext, 
            : base(mediator)
        {
            //_sessionContext = sessionContext;
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
