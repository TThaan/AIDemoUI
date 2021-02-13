using AIDemoUI.Factories;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Collections.ObjectModel;

namespace AIDemoUI
{
    public interface ISessionContext
    {
        // MainWindowVM MainWindowVM { get; }
        StartStopVM StartStopVM { get; }
        StatusVM StatusVM { get; }
        NetParametersVM NetParametersVM { get; }
        ObservableCollection<LayerParametersVM> LayerParametersVMs { get; }
        ILayerParameters LayerParameters { get; }
        ILayerParametersVMFactory LayerParametersVMFactory { get; }
        SampleImportWindow SampleImportWindow { get; }  // ?
    }

    /// <summary>
    /// Contains shared data of different view models.
    /// </summary>
    public class SessionContext : ISessionContext
    {
        #region ctor

        public SessionContext(MainWindowVM mainWindowVM, StartStopVM startStopVM, StatusVM statusVM, NetParametersVM netParametersVM,
            ILayerParameters layerParameters, ILayerParametersVMFactory layerParametersVMFactory, ObservableCollection<LayerParametersVM> layerParametersVMs, //ILayerParametersVM
            SampleImportWindow sampleImportWindow)
        {
            MainWindowVM = mainWindowVM;
            StartStopVM = startStopVM;
            StatusVM = statusVM;
            NetParametersVM = netParametersVM;
            LayerParametersVMs = layerParametersVMs;
            LayerParameters = layerParameters;
            LayerParametersVMFactory = layerParametersVMFactory;
            SampleImportWindow = sampleImportWindow;
        }

        #endregion

        public MainWindowVM MainWindowVM { get; }
        public StartStopVM StartStopVM { get; }
        public StatusVM StatusVM { get; }
        public ObservableCollection<LayerParametersVM> LayerParametersVMs { get; } // Later: Use only ObservableCollection<LayerParameter> in SessionContext!
        public ILayerParameters LayerParameters { get; }
        public ILayerParametersVMFactory LayerParametersVMFactory { get; }
        public ILayerParametersVMCollectionFactory LayerParametersVMsFactory { get; }
        public NetParametersVM NetParametersVM { get; }
        public SampleImportWindow SampleImportWindow { get; }
    }
}
