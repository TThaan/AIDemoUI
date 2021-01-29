using AIDemoUI.ViewModels;
using System;

namespace AIDemoUI.SampleData
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        /// <summary>
        /// Ctor used by LayerParametersVMSampleData
        /// </summary>
        public LayerParametersVMSampleData()
            :base(new MainWindowVM(), 1234)
        {

        }
        /// <summary>
        /// Ctor used by MainVMSampleData
        /// </summary>
        public LayerParametersVMSampleData(MainWindowVM mainVM, int id)
            : base(mainVM, id)
        {
            throw new ArgumentException($"In parameterized ctor of {GetType().Name}");
        }
    }
}
