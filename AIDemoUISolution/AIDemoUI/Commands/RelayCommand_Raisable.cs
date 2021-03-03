using System;

namespace AIDemoUI.Commands
{
    public class RelayCommand_Raisable : RelayCommand, IRaisableCommand
    {
        #region ctor

        public RelayCommand_Raisable(Action<object> execute, Predicate<object> canExecute)
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
