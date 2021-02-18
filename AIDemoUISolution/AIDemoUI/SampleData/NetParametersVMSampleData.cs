using AIDemoUI.ViewModels;
using System;
using static AIDemoUI.SampleData.SampleDataBootStrapper;

namespace AIDemoUI.SampleData
{
    public class NetParametersVMSampleData : NetParametersVM
    {
        #region ctor

        public NetParametersVMSampleData()
            : base(SampleMediator, SampleNetParameters, SampleTrainerParameters, SampleLayerParametersCollection, SampleLayerParametersFactory)
        {
            //throw new ArgumentException($"Location: {GetType().Name}:\n{nameof(SampleLayerParametersCollection)}.Length: {SampleLayerParametersCollection.Count}");
            AreParametersGlobal = true;
            WeightMin_Global = -1;
            WeightMax_Global = 1;
        }

        #endregion
    }
}
