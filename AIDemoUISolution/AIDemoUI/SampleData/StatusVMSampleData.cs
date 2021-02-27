using AIDemoUI.ViewModels;
using NeuralNetBuilder;
using static AIDemoUI.SampleData.SampleDataInitializer;

namespace AIDemoUI.SampleData
{
    public class StatusVMSampleData : StatusVM
    {
        public StatusVMSampleData()
            : base(SampleSessionContext, SampleMediator)
        {
            SampleSessionContext.Trainer = Initializer.GetRawTrainer();
            SampleSessionContext.Trainer.CurrentEpoch = 3;
            SampleSessionContext.Trainer.CurrentSample = 742;
            SampleSessionContext.Trainer.Epochs = 10;
            SampleSessionContext.Trainer.SamplesTotal = 1000;
            SampleSessionContext.Trainer.CurrentTotalCost = .00037647f;

            SampleSessionContext.Trainer.Status = "Training...";
        }
    }
}
