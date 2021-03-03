using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AIDemoUI.Commands
{
    public class ConcurrentAsyncRelayCommand : AsyncRelayCommand
    {
        #region fields & ctor

        public ConcurrentAsyncRelayCommand(Func<object, Task> execute, Predicate<object> canExecute = default)
            :base(execute, canExecute)
        {
            _isConcurrentExecutionAllowed = true;
        }

        #endregion

        #region overrides

        [DebuggerStepThrough]
        public override bool CanExecute(object parameter = null)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        #endregion
    }
}
