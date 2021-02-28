using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.RawData;
using static AIDemoUI.SampleData.MockData;

namespace AIDemoUI.SampleDataViewModel
{
    public class NetParametersVMSampleData : NetParametersVM
    {
        #region ctor

        public NetParametersVMSampleData()
            : base(MockSessionContext, SampleMediator, SampleLayerParametersVMFactory, SampleLayerParametersFactory)
        {
            AreParametersGlobal = false;
            WeightMin_Global = -1;
            WeightMax_Global = 1;
            BiasMin_Global = 0;
            BiasMax_Global = 0;
        }

        #endregion
    }
}
