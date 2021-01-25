using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AIDemoUI.Commands
{
    public class AsyncRelayCommand : IAsyncCommand
    {
        #region IAsyncCommand fields

        Func<object, Task> _execute;
        Predicate<object> _canExecute;
        bool _isExecuting;

        #endregion

        #region IAsyncCommand ctor

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
                //_isExecuting = true;
                //await _execute(parameter);
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                catch(Exception e)
                {
                    throw;
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            OnCanExecuteChanged();
        }

        #region ICommand

        [DebuggerStepThrough]
        public bool CanExecute(object parameter = null)
        {
            return
                !_isExecuting &&
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
        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                ExecuteAsync(parameter).FireAndForgetSafeAsync();
            }
            else { _execute(parameter); }
        }
        public void OnCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion

        #endregion
    }
}
