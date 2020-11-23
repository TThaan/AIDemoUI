using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AIDemoUI.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVisible = (bool)value;
            var hiddenOrCollapsed = (string)parameter;

            if (isVisible)
            {
                return Visibility.Visible;
            }

            if (hiddenOrCollapsed == "hidden")
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
