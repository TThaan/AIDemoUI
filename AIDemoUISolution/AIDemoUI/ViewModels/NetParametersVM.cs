using AIDemoUI.Commands;
using FourPixCam;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AIDemoUI.ViewModels
{
    public class NetParametersVM : BaseVM
    {
        #region ctor & fields

        NetParameters netParameters;
        RelayCommand addCommand, deleteCommand, moveUpCommand, okCommand, unfocusCommand;
        IEnumerable<ActivationType> activationTypes;
        IEnumerable<CostType> costTypes;
        IEnumerable<WeightInitType> weightInitTypes;

        public NetParametersVM(NetParameters netParameters)
        {
            this.netParameters = netParameters ?? 
                throw new NullReferenceException($"{GetType().Name}.ctor");    
        }

        #endregion

        #region public

        public ObservableCollection<Layer> Layers
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

        public IEnumerable<ActivationType> ActivationTypes => activationTypes ??
            (activationTypes = Enum.GetValues(typeof(ActivationType)).ToList<ActivationType>().Skip(1));
        public IEnumerable<CostType> CostTypes => costTypes ?? 
            (costTypes = Enum.GetValues(typeof(CostType)).ToList<CostType>().Skip(1));
        public IEnumerable<WeightInitType> WeightInitTypes => weightInitTypes ??
            (weightInitTypes = Enum.GetValues(typeof(WeightInitType)).ToList<WeightInitType>().Skip(1));

        #endregion

        #region RelayCommand

        public RelayCommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(Layers, AddCommand_Execute, AddCommand_CanExecute);
                }
                return addCommand;
            }
        }
        void AddCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            Layer layer = cp.Content as Layer;
            Layers.Add(new Layer { N = 0, ActivationType = ActivationType.ReLU });
        }
        bool AddCommand_CanExecute(object parameter)
        {
            return true;
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(Layers, DeleteCommand_Execute, DeleteCommand_CanExecute);
                }
                return deleteCommand;
            }
        }
        void DeleteCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            Layer layer = cp.Content as Layer;
            Layers.Remove(layer);
        }
        bool DeleteCommand_CanExecute(object parameter)
        {
            return true;
        }
        public RelayCommand MoveUpCommand
        {
            get
            {
                if (moveUpCommand == null)
                {
                    moveUpCommand = new RelayCommand(Layers, MoveUpCommand_Execute, MoveUpCommand_CanExecute);
                }
                return moveUpCommand;
            }
        }
        void MoveUpCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            Layer layer = cp.Content as Layer;
            int currentIndex = Layers.IndexOf(layer);
            Layers.Move(currentIndex, currentIndex > 0 ? currentIndex - 1 : 0);
        }
        bool MoveUpCommand_CanExecute(object parameter)
        {
            return true;
        }
        public RelayCommand UnfocusCommand
        {
            get
            {
                if (unfocusCommand == null)
                {
                    unfocusCommand = new RelayCommand(Layers, UnfocusCommand_Execute, UnfocusCommand_CanExecute);
                }
                return unfocusCommand;
            }
        }
        void UnfocusCommand_Execute(object parameter)
        {            
            var netParametersView = parameter as UserControl;
            netParametersView.FocusVisualStyle = null;
            netParametersView.Focusable = true;
            netParametersView.Focus();
        }
        bool UnfocusCommand_CanExecute(object parameter)
        {
            return true;
        }
        public RelayCommand OkCommand
        {
            get
            {
                if (okCommand == null)
                {
                    okCommand = new RelayCommand(Layers, OkCommand_Execute, OkCommand_CanExecute);
                }
                return okCommand;
            }
        }
        void OkCommand_Execute(object parameter)
        {
            MessageBox.Show("Not implemented yet.");
        }
        bool OkCommand_CanExecute(object parameter)
        {
            return true;
        }

        #endregion

        #region Dedicated Commands



        #endregion
    }
}
