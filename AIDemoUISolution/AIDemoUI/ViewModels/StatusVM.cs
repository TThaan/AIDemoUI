using NeuralNetBuilder;
using System;
using System.Windows;

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
        string Message { get; }
        Visibility DetailsVisibility { get; }
    }

    public class StatusVM : BaseVM, IStatusVM
    {
        #region fields & ctor

        public StatusVM(ISessionContext sessionContext, ISimpleMediator mediator)
            : base(sessionContext, mediator)
        {
            
        }

        #endregion

        #region properties

        public int SamplesTotal => _sessionContext.Trainer.SamplesTotal;
        public int Epochs => _sessionContext.Trainer.Epochs;
        public int CurrentEpoch => _sessionContext.Trainer.CurrentEpoch;
        public int CurrentSample => _sessionContext.Trainer.CurrentSample;
        public float CurrentTotalCost => _sessionContext.Trainer.CurrentTotalCost;
        public float LastEpochsAccuracy => _sessionContext.Trainer.LastEpochsAccuracy;
        public Visibility DetailsVisibility => GetDetailsVisibility();
        public string Message
        {
            get
            {
                OnPropertyChanged(nameof(DetailsVisibility));
                return $"{_sessionContext.Trainer.Message}";
            }
        }

        #region helpers

        private Visibility GetDetailsVisibility()
        {
            if (_sessionContext.SampleSet == null ||
                _sessionContext.Trainer == null || 
                _sessionContext.Trainer.TrainerStatus == TrainerStatus.Undefined || 
                _sessionContext.Trainer.TrainerStatus == TrainerStatus.Initialized)
            {
                return Visibility.Hidden;
            }
            return Visibility.Visible;
        }

        #endregion

        #endregion
    }
}
