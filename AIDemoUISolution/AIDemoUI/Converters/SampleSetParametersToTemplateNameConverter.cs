using NNet_InputProvider;
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
            SampleSetParameters setParameters = value as SampleSetParameters;

            try
            {
                return setParameters.Name;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SetName templateName = (SetName)value;

            try
            {
                return SampleSetParameters.Templates[templateName];
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
