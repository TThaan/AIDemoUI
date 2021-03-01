using DeepLearningDataProvider;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AIDemoUI.Converters
{
    public class SampleSetParametersToTemplateNameConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ISampleSetParameters setParameters = value as ISampleSetParameters;

            try
            {
                return setParameters.Name;
            }
            catch (Exception)
            {
                // throw;
                return "Couldn't resolve SetParameter's name.";
            }
        }
        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
