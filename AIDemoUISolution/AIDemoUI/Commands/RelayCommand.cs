using System;
using System.Diagnostics;
using System.Windows.Input;

namespace AIDemoUI.Commands
{
    public class RelayCommand : IRelayCommand
    {
        #region Fields

        readonly Action<object> execute = null;
        readonly Predicate<object> canExecute = null;

        #endregion

        #region Constructors

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = default)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion
    }
}
