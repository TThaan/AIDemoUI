using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.SampleDataBootStrapper;

namespace AIDemoUI.SampleData
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        public LayerParametersVMSampleData()
            : base(SampleSessionContext, SampleLayerParameters, SampleLayerParametersVMFactory, null, 1234)
        {

        }
    }
}
