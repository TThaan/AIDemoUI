using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataBootStrapper;

namespace AIDemoUI.SampleData
{
    public class NetParametersVMSampleData : NetParametersVM
    {
        #region ctor

        public NetParametersVMSampleData()
            : base(SampleSessionContext, SampleNetParameters, SampleTrainerParameters, SampleLayerParametersVMFactory) { }

        #endregion
    }
}
