using MatrixHelper;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AIDemoUI.Converters
{
    public class MatrixToStringConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new System.ArgumentException($"" +
            //    $"value == null: " +
            //    $"{value == null}");
            IMatrix matrix = value as IMatrix;
            if (matrix == null) return "Matrix is null.";
            else return matrix.ToLog();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
