using AIDemoUI.ViewModels;
using AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModels
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        static int changingId = 0;

        public LayerParametersVMSampleData()
            : base(new MockSessionContext(), RawMediator)
        {
            LayerParameters = new MockLayerParameters(changingId);
            changingId++;
        }
    }
}
