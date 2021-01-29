using System;

namespace AIDemoUI
{
    public delegate void SubViewModelChangedEventHandler(object s, SubViewModelChangedEventArgs e);
    public class SubViewModelChangedEventArgs : EventArgs
    {
        private string _property;

        public SubViewModelChangedEventArgs(string property)
        {
            _property = property;
        }

        public string Property
        {
            get { return _property; }
        }
    }
    public interface INotifySubViewModelChanged
    {
        event SubViewModelChangedEventHandler SubViewModelChanged;
    }
}