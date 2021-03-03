using System;
using System.Threading.Tasks;

namespace AIDemoUI.Commands.Async
{
    public class AsyncRelayCommand_Raisable : AsyncRelayCommand, IRaisableCommand
    {
        #region ctor

        public AsyncRelayCommand_Raisable(Func<object, Task> execute, Predicate<object> canExecute)
            : base(execute, canExecute) { }

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
