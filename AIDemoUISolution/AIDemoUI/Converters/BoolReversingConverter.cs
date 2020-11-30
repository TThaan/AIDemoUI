using System;
using System.Globalization;
using System.Windows.Data;

namespace AIDemoUI.Converters
{
    public class BoolReversingConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (bool)value == false ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
