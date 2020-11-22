using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AIDemoUI
{
    public static class ExtensionMethods
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}
