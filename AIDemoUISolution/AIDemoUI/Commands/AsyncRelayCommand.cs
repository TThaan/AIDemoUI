using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AIDemoUI.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }

    public class AsyncRelayCommand : IAsyncCommand
    {
        #region fields& ctor

        private Func<object, Task> _execute;
        protected Predicate<object> _canExecute;
        protected bool _isExecuting, _isConcurrentExecutionAllowed;


        public AsyncRelayCommand(Func<object, Task> execute, Predicate<object> canExecute = default)
        {
            _execute = execute ?? throw new NullReferenceException(
                $"{execute.GetType().Name} {nameof(execute)} ({GetType()}.ctor)");
            _canExecute = canExecute;
        }

        #endregion

        #region IAsyncCommand

        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute())
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

            OnCanExecuteChanged();
        }

        #region ICommand

        public void Execute(object parameter)
        {
            ExecuteAsync(parameter).FireAndForgetSafeAsync();
        }
        [DebuggerStepThrough]
        public virtual bool CanExecute(object parameter = null)
        {
            return
                (_isConcurrentExecutionAllowed || !_isExecuting) &&
                (_canExecute?.Invoke(parameter) ?? true);
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
            // CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #endregion
    }
}
