using NeuralNetBuilder;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AIDemoUI.Converters
{
    public class VisibilityToVisibilityConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //throw new ArgumentException($"{value == null}");
                TrainerStatus status = (TrainerStatus)value;
                return status == TrainerStatus.Undefined || status == TrainerStatus.Initialized 
                    ? Visibility.Hidden
                    : Visibility.Visible;
            }
            catch (Exception)
            {
                return Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
