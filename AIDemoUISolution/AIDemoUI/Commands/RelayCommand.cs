using System;
using System.Diagnostics;

namespace AIDemoUI.Commands
{
    public class RelayCommand : SimpleRelayCommand
    {
        #region fields & ctor

        protected readonly Predicate<object> _canExecute = null;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            :base(execute)
        {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        #endregion

        #region ICommand

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                base.Execute(parameter);
            }
        }
        [DebuggerStepThrough]
        public override bool CanExecute(object parameter) => _canExecute(parameter);

        #endregion
    }
}
