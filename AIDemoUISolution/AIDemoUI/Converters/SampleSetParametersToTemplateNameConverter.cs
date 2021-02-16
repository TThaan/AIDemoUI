using DeepLearningDataProvider;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AIDemoUI.Converters
{
    public class SampleSetParametersToTemplateNameConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            SampleSetParameters setParameters = values as SampleSetParameters;

            try
            {
                return setParameters.Name;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
