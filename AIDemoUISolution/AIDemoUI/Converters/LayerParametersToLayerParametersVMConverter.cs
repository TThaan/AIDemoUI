using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.ViewModels;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AIDemoUI.Converters
{
    public class LayerParametersToLayerParametersVMConverter : IMultiValueConverter
    {
        #region IValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ILayerParameters lp = values[0] as ILayerParameters;
            ILayerParametersVMFactory lpvmFactory = values[1] as ILayerParametersVMFactory;
            if (lp == null || lpvmFactory == null) return null;

            ILayerParametersVM result = lpvmFactory.CreateLayerParametersVM(lp);
            return result;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
