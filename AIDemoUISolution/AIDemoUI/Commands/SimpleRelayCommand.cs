using System;
using System.Diagnostics;
using System.Windows.Input;

namespace AIDemoUI.Commands
{
    public class SimpleRelayCommand : ICommand
    {
        #region fields & ctor

        protected readonly Action<object> _execute = null;

        public SimpleRelayCommand(Action<object> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        #endregion

        #region ICommand

        public virtual void Execute(object parameter) => _execute(parameter);
        [DebuggerStepThrough]
        public virtual bool CanExecute(object parameter) => true;
        public virtual event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion
    }
}
