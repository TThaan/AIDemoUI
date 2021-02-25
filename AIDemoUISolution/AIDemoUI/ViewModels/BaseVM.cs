using AIDemoUI.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AIDemoUI.ViewModels
{
    public interface IBaseVM
    {
        IRelayCommand UnfocusCommand { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        void OnAllPropertiesChanged();

        void Unfocus(object parameter);

        // debugging
        bool IsPropertyChangedNull();
    }

    public class BaseVM : INotifyPropertyChanged, IBaseVM
    {
        #region fields & ctor

        protected readonly ISimpleMediator _mediator;

        public BaseVM(ISimpleMediator mediator)
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
        public virtual void OnAllPropertiesChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        // debugging
        public bool IsPropertyChangedNull()
        {
            return PropertyChanged == null;
        }

        #endregion
    }
}
