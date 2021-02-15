using AIDemoUI.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AIDemoUI.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        #region fields & ctor

        protected readonly SimpleMediator _mediator;
        IRelayCommand unfocusCommand;

        public BaseVM(SimpleMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Commands

        public IRelayCommand UnfocusCommand
        {
            get
            {
                if (unfocusCommand == null)
                {
                    unfocusCommand = new RelayCommand(UnfocusCommand_Execute, UnfocusCommand_CanExecute);
                }
                return unfocusCommand;
            }
        }
        void UnfocusCommand_Execute(object parameter)
        {
            var element = parameter as UIElement;
            element.Focusable = true;
            element.Focus();
        }
        bool UnfocusCommand_CanExecute(object parameter)
        {
            return true;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnAllPropertiesChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        #endregion
    }
}
