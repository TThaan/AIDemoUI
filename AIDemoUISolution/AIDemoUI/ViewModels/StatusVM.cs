using System;
using System.ComponentModel;

namespace AIDemoUI.ViewModels
{
    public interface IStatusVM : IBaseVM
    {
        int CurrentEpoch { get; }
        int CurrentSample { get; }
        float CurrentTotalCost { get; }
        int SamplesTotal { get; }
        int Epochs { get; }
        float LastEpochsAccuracy { get; }
        string Status { get; }
        void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e);
        void SampleSet_PropertyChanged(object sender, PropertyChangedEventArgs e);
    }

    public class StatusVM : BaseSubVM, IStatusVM
    {
        #region fields & ctor

        // int progressBarValue, progressBarMax;
        // string progressBarText;
        private readonly ISessionContext _sessionContext;

        public StatusVM(ISessionContext sessionContext, ISimpleMediator mediator)
            : base(mediator)
        {
            _mediator.Register("Token: MainWindowVM", StatusVMCallback);
            _sessionContext = sessionContext;       
        }

        private void StatusVMCallback(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region properties

        public int SamplesTotal => _sessionContext.Trainer.SamplesTotal;
        public int Epochs => _sessionContext.Trainer.Epochs;
        public int CurrentEpoch => _sessionContext.Trainer.CurrentEpoch;
        public int CurrentSample => _sessionContext.Trainer.CurrentSample;
        public float CurrentTotalCost => _sessionContext.Trainer.CurrentTotalCost;
        public float LastEpochsAccuracy => _sessionContext.Trainer.LastEpochsAccuracy;
        public string Status => $"{_sessionContext.SampleSet?.Status}\n{_sessionContext.Trainer.Status}";   // remove question mark

        #endregion

        #region events

        public void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        public void SampleSet_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #endregion
    }
}
