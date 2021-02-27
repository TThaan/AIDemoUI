using AIDemoUI.ViewModels;
using static AIDemoUI.SampleData.MockData;

namespace AIDemoUI.SampleData
{
    public class StatusVMSampleData : StatusVM
    {
        public StatusVMSampleData()
            : base(MockSessionContext, MockMediator)
        {
            //throw new System.ArgumentException($"MockSessionContext == null: {MockSessionContext == null}");
        }
    }
}
