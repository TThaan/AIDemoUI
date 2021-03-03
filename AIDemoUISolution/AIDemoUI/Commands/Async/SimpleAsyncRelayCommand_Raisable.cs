using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AIDemoUI.Commands.Async
{
    public class SimpleAsyncRelayCommand_Raisable : SimpleAsyncRelayCommand, IRaisableCommand
    {
        #region ctor

        public SimpleAsyncRelayCommand_Raisable(Func<object, Task> execute)
            : base(execute) { }

        #endregion

        #region ICommand

        public override event EventHandler CanExecuteChanged;

        #endregion

        #region IRelayCommand_Raisable

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
