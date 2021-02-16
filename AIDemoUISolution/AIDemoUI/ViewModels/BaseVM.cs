using AIDemoUI.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AIDemoUI.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        #region fields & ctor

        protected readonly SimpleMediator _mediator;

        public BaseVM(SimpleMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Commands

        public IRelayCommand UnfocusCommand { get; set; }

        #region Executes and CanExecutes

        public void Unfocus(object parameter)
        {
            var element = parameter as UIElement;
            element.Focusable = true;
            element.Focus();
        }

        #endregion

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
