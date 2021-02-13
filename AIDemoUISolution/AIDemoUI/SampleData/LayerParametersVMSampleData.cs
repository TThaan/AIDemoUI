using AIDemoUI.ViewModels;
using System;

namespace AIDemoUI.SampleData
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        public LayerParametersVMSampleData()
            : base(SampleDataBootStrapper.SampleSessionContext, 1234)
        {

        }
    }
}
