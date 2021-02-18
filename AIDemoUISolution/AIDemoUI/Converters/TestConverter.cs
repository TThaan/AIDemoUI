using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace AIDemoUI.Converters
{
    public class TestConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null) 
            { 
                var v = value.GetType().Name; 
            }
            ILayerParameters lp = value as LayerParameters;
            // throw new ArgumentException($"value = null: {value == null}\n");
            // return ((value as ILayerParameters) == null).ToString();
            if (value == null) return "value is null";
            return "value type: " + value.GetType().Name;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
