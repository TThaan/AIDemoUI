using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AIDemoUI
{
    public static class ExtensionMethods
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
        /// <summary>
        /// Source: https://johnthiriet.com/removing-async-void/#
        /// </summary>
        /// <param name="task"></param>
        /// <param name="handler"></param>
        public static async void FireAndForgetSafeAsync(this Task task, IExceptionHandler handler = null)
        {
            try
            {
                await task;
            }
            catch (Exception exception)
            {
                handler?.HandleException(exception);
            }
        }
    }
}
