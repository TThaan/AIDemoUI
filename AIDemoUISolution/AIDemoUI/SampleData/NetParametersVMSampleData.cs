using AIDemoUI.ViewModels;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Collections.ObjectModel;

namespace AIDemoUI.SampleData
{
    public class NetParametersVMSampleData : NetParametersVM
    {
        #region ctor

        public NetParametersVMSampleData()
            :base(new NetParameters(), new TrainerParameters())
        {
            // LayerParameterVMs = new ObservableCollection<LayerParametersVM>();
            // LayerParameterVMs.CollectionChanged += OnLayerVMsChanged;
            // LayerParameterVMs.Add(new LayerParametersVM(0));
            // LayerParameterVMs.Add(new LayerParametersVM(1));
            // LayerParameterVMs.Add(new LayerParametersVM(2));
            // LayerParameterVMs.Add(new LayerParametersVM(3));
            // LayerParameterVMs.Add(new LayerParametersVM(4));
        }

        #endregion
    }
}
