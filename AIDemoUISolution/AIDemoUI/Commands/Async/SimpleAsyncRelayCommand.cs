using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AIDemoUI.Commands.Async
{
    public class SimpleAsyncRelayCommand : IAsyncRelayCommand
    {
        #region fields& ctor

        private Func<object, Task> _execute;
        protected bool _isExecuting;

        public SimpleAsyncRelayCommand(Func<object, Task> execute)
        {
            _execute = execute ?? throw new NullReferenceException(nameof(execute));
        }

        #endregion

        #region IAsyncCommand

        public virtual async Task ExecuteAsync(object parameter)
        {
            try
            {
                _isExecuting = true;
                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
            }
        }
        public bool IsConcurrentExecutionAllowed { get; set; }

        #region ICommand

        public void Execute(object parameter) => ExecuteAsync(parameter).FireAndForgetSafeAsync();
        [DebuggerStepThrough]
        public virtual bool CanExecute(object parameter) => !_isExecuting || IsConcurrentExecutionAllowed;
        public virtual event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #endregion
    }
}
