using DeepLearningDataProvider;
using System;
using System.Collections.Generic;
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

            var t0 = values.GetType().Name;
            // var t1 = values[1].GetType().Name;
            // var t2 = values[2].GetType().Name;
            // var t3 = values[3].GetType().Name;
            // var t4 = values[4].GetType().Name;

            // redundant.. (incl multi converter/binding)
            // Dictionary<SetName, SampleSetParameters> templates = values[1] as Dictionary<SetName, SampleSetParameters>;

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
            //SetName templateName = (SetName)value;

            //try
            //{
            //    return Creator.Templates[templateName];
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        #endregion
    }
}
