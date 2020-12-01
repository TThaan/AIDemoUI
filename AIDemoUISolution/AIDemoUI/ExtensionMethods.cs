using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AIDemoUI
{
    public static class ExtensionMethods
    {
        internal static List<T> ToList<T>(this Array arr)
        {
            var result = new List<T>();

            for (int i = 0; i < arr.Length; i++)
            {
                result.Add((T)arr.GetValue(i));
            }

            return result;
        }
        internal static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
        /// <summary>
        /// Source: https://johnthiriet.com/removing-async-void/#
        /// </summary>
        /// <param name="task"></param>
        /// <param name="handler"></param>
        internal static async void FireAndForgetSafeAsync(this Task task, IExceptionHandler handler = null)
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
