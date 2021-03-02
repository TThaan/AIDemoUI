﻿using AIDemoUI.Commands;
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
        void Any_PropertyChanged(object sender, PropertyChangedEventArgs e);
        void OnAllPropertiesChanged();

        void Unfocus(object parameter);

        // debugging
        bool IsPropertyChangedNull();
    }

    public class BaseVM : INotifyPropertyChanged, IBaseVM
    {
        #region fields & ctor

        protected readonly ISessionContext _sessionContext;
        protected readonly ISimpleMediator _mediator;

        public BaseVM(ISessionContext sessionContext, ISimpleMediator mediator)
        {
            _sessionContext = sessionContext;
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
        public void Any_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
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
