using AIDemoUI.ViewModels;
using AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModels
{
    public class NetParametersVMSampleData : NetParametersVM
    {
        #region ctor

        public NetParametersVMSampleData()
            : base(new MockSessionContext(), RawMediator, RawLayerParametersVMFactory, RawLayerParametersFactory)
        {
            // LoadDefault?
            AreParametersGlobal = false;
            WeightMin_Global = -1;
            WeightMax_Global = 1;
            BiasMin_Global = 0;
            BiasMax_Global = 0;
        }

        #endregion
    }
}
