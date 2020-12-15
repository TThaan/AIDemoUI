using AIDemoUI.ViewModels;
using System;

namespace AIDemoUI.SampleData
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        public LayerParametersVMSampleData()
            :base(1234)
        {

        }
        public LayerParametersVMSampleData(int id)
            : base(id)
        {
            throw new ArgumentException($"In parameterized ctor of {GetType().Name}");
        }
    }
}
