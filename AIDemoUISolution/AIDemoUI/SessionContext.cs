using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.ViewModels;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Collections.ObjectModel;

namespace AIDemoUI
{
    public interface ISessionContext
    {
        INetParameters NetParameters { get; }
        ILayerParameters LayerParameters { get; }
        ITrainerParameters TrainerParameters { get; }
        ObservableCollection<LayerParametersVM> LayerParametersVMCollection { get; }
        //ILayerParametersVMFactory LayerParametersVMFactory { get; }
        //SampleImportWindow SampleImportWindow { get; }  // ?
        ISampleSetParametersSteward SampleSetParametersSteward { get; set; }
        // StatusChangedEventHandler SampleSet_StatusChanged { get; set; }
    }

    /// <summary>
    /// Contains shared data of different view models.
    /// </summary>
    public class SessionContext : ISessionContext
    {
        #region ctor

        public SessionContext(INetParameters netParameters,ILayerParameters layerParameters, ITrainerParameters trainerParameters, ObservableCollection<LayerParametersVM> layerParametersVMCollection, // ILayerParametersVM, ILayerParametersVMFactory layerParametersVMFactory
                                                                                                                                                                                                        //SampleImportWindow sampleImportWindow,
            ISampleSetParametersSteward sampleSetParametersSteward)//StatusChangedEventHandler sampleSet_StatusChanged
        {
            NetParameters = netParameters;
            LayerParameters = layerParameters;
            TrainerParameters = trainerParameters;
            LayerParametersVMCollection = layerParametersVMCollection;
            //LayerParametersVMFactory = layerParametersVMFactory;
            //SampleImportWindow = sampleImportWindow;

            SampleSetParametersSteward = sampleSetParametersSteward;
            //SampleSet_StatusChanged = sampleSet_StatusChanged;
        }

        #endregion

        public INetParameters NetParameters { get; }
        public ILayerParameters LayerParameters { get; }
        public ITrainerParameters TrainerParameters { get; }
        public ObservableCollection<LayerParametersVM> LayerParametersVMCollection { get; } // Later: Use only ObservableCollection<LayerParameter> in SessionContext!
        //public ILayerParametersVMFactory LayerParametersVMFactory { get; }
        public ILayerParametersVMCollectionFactory LayerParametersVMsFactory { get; }
        //public SampleImportWindow SampleImportWindow { get; }

        public ISampleSetParametersSteward SampleSetParametersSteward { get; set; }
        //public StatusChangedEventHandler SampleSet_StatusChanged { get; set; }
    }
}
