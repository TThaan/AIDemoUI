using AIDemoUI.Commands;
using FourPixCam;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AIDemoUI.ViewModels
{
    public delegate Task OkBtnEventHandler(NetParameters netParameters);

    public class NetParametersVM : BaseVM
    {
        #region ctor & fields

        NetParameters netParameters;
        IRelayCommand addCommand, deleteCommand, moveUpCommand, unfocusCommand;
        IAsyncCommand okCommandAsync;
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

        public IRelayCommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(AddCommand_Execute, AddCommand_CanExecute);
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
        public IRelayCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(DeleteCommand_Execute, DeleteCommand_CanExecute);
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
        public IRelayCommand MoveUpCommand
        {
            get
            {
                if (moveUpCommand == null)
                {
                    moveUpCommand = new RelayCommand(MoveUpCommand_Execute, MoveUpCommand_CanExecute);
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
        public IRelayCommand UnfocusCommand
        {
            get
            {
                if (unfocusCommand == null)
                {
                    unfocusCommand = new RelayCommand(UnfocusCommand_Execute, UnfocusCommand_CanExecute);
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
        public IAsyncCommand OkCommandAsync
        {
            get
            {
                if (okCommandAsync == null)
                {
                    okCommandAsync = new AsyncRelayCommand(OkCommand_Execute, OkCommand_CanExecute);
                }
                return okCommandAsync;
            }
        }
        async Task OkCommand_Execute(object parameter)
        {
            var netParametersView = parameter as UserControl;
            if (netParametersView != null)
            {
                await OnOkBtnPressedAsync();
            }
        }
        bool OkCommand_CanExecute(object parameter)
        {
            return true;
        }

        #endregion

        #region OkBtnPressed

        public event OkBtnEventHandler OkBtnPressed;
        async Task OnOkBtnPressedAsync()
        {
            await OkBtnPressed?.Invoke(netParameters);
        }

        #endregion
    }
}
