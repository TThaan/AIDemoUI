using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.MockData;
using static AIDemoUI.SampleData.RawData;

namespace AIDemoUI.SampleDataViewModel
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        static int counter = 0;

        public LayerParametersVMSampleData()
            : base(MockSessionContext, SampleMediator)
        {
            // Vary sample layer views

            switch (counter)
            {
                case 0:
                    LayerParameters = MockLayerParameters01;
                    break;
                case 1:
                    LayerParameters = MockLayerParameters02;
                    break;
                case 2:
                    LayerParameters = MockLayerParameters03;
                    break;
            }
            counter++;
        }
    }
}
