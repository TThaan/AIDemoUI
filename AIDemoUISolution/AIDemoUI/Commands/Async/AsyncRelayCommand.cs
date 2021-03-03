using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AIDemoUI.Commands.Async
{
    public class AsyncRelayCommand : SimpleAsyncRelayCommand
    {
        #region fields& ctor

        protected Predicate<object> _canExecute;

        public AsyncRelayCommand(Func<object, Task> execute, Predicate<object> canExecute)
            :base(execute)
        {
            _canExecute = canExecute;
        }

        #endregion

        #region IAsyncCommand

        public override async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                await base.ExecuteAsync(parameter);
            }
        }

        #endregion

        #region ICommand

        [DebuggerStepThrough]
        public override bool CanExecute(object parameter)
        {
            return
                base.CanExecute(parameter) &&
                (_canExecute?.Invoke(parameter) ?? true);
        }

        #endregion
    }
}
