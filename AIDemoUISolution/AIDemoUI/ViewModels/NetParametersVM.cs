using AIDemoUI.Commands;
using FourPixCam;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AIDemoUI.ViewModels
{
    public class NetParametersVM : BaseVM
    {
        #region ctor & fields

        NetParameters netParameters;
        RelayCommand netParametersCommand;

        public NetParametersVM(NetParameters netParameters)
        {
            this.netParameters = netParameters ?? 
                throw new NullReferenceException($"{GetType().Name}.ctor");
        }

        #endregion

        #region public

        public List<(int N, ActivationType activationType)> Layers
        {
            get { return netParameters.Layers; }
            set
            {
                if (netParameters.Layers != value)
                {
                    netParameters.Layers = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsWithBias
        {
            get { return netParameters.IsWithBias; }
            set
            {
                if (netParameters.IsWithBias != value)
                {
                    netParameters.IsWithBias = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMin
        {
            get { return netParameters.WeightMin; }
            set
            {
                if (netParameters.WeightMin != value)
                {
                    netParameters.WeightMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMax
        {
            get { return netParameters.WeightMax; }
            set
            {
                if (netParameters.WeightMax != value)
                {
                    netParameters.WeightMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMin
        {
            get { return netParameters.BiasMin; }
            set
            {
                if (netParameters.BiasMin != value)
                {
                    netParameters.BiasMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMax
        {
            get { return netParameters.BiasMax; }
            set
            {
                if (netParameters.BiasMax != value)
                {
                    netParameters.BiasMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public CostType CostType
        {
            get { return netParameters.CostType; }
            set
            {
                if (netParameters.CostType != value)
                {
                    netParameters.CostType = value;
                    OnPropertyChanged();
                }
            }
        }
        public WeightInitType WeightInitType
        {
            get { return netParameters.WeightInitType; }
            set
            {
                if (netParameters.WeightInitType != value)
                {
                    netParameters.WeightInitType = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region SettingsCommand

        public RelayCommand NetParametersCommand
        {
            get
            {
                if (netParametersCommand == null)
                {
                    netParametersCommand = new RelayCommand(NetParametersCommand_Execute, NetParametersCommand_CanExecute);
                }
                return netParametersCommand;
            }
        }
        void NetParametersCommand_Execute(object parameter)
        {
            MessageBox.Show("Not implemented yet.");
        }
        bool NetParametersCommand_CanExecute(object parameter)
        {
            return true;
        }

        #endregion
    }
}
