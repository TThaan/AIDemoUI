using System;

namespace AIDemoUI.Commands
{
    public class SimpleRelayCommand_Raisable : SimpleRelayCommand, IRaisableCommand
    {
        #region ctor

        public SimpleRelayCommand_Raisable(Action<object> execute)
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
