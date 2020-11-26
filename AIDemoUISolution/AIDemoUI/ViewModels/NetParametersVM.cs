using AIDemoUI.Commands;
using FourPixCam;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AIDemoUI.ViewModels
{
    public delegate Task OkBtnEventHandler(NetParameters netParameters);

    public class NetParametersVM : BaseVM
    {
        #region ctor & fields

        NetParameters _netParameters;
        IRelayCommand addCommand, deleteCommand, moveLeftCommand, moveRightCommand, unfocusCommand;
        IAsyncCommand okCommandAsync;
        IEnumerable<ActivationType> activationTypes;
        IEnumerable<CostType> costTypes;
        IEnumerable<WeightInitType> weightInitTypes;
        ObservableCollection<LayerVM> layerVMs;

        public NetParametersVM()
        {
            _netParameters = new NetParameters();
            SetDefaultValues();
            LayerVMs.CollectionChanged += OnLayerVMsChanged;
        }

        #region helpers

        void SetDefaultValues()
        {
            IsWithBias = false;
            WeightMin = -1;
            WeightMax = 1;
            BiasMin = -1;
            BiasMax = 1;
            CostType = CostType.SquaredMeanError;
            WeightInitType = WeightInitType.Xavier;
            LayerVMs = new ObservableCollection<LayerVM>
            {
                new LayerVM(new Layer(){ Id = 0}), 
                new LayerVM(new Layer(){ Id = 1}), 
                new LayerVM(new Layer(){ Id = 2})
            };
        }

        #endregion

        #endregion

        #region public

        public ObservableCollection<LayerVM> LayerVMs
        {
            get { return layerVMs; }
            set
            {
                if (layerVMs != value)
                {
                    layerVMs = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsWithBias
        {
            get { return _netParameters.IsWithBias; }
            set
            {
                if (_netParameters.IsWithBias != value)
                {
                    _netParameters.IsWithBias = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMin
        {
            get { return _netParameters.WeightMin; }
            set
            {
                if (_netParameters.WeightMin != value)
                {
                    _netParameters.WeightMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMax
        {
            get { return _netParameters.WeightMax; }
            set
            {
                if (_netParameters.WeightMax != value)
                {
                    _netParameters.WeightMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMin
        {
            get { return _netParameters.BiasMin; }
            set
            {
                if (_netParameters.BiasMin != value)
                {
                    _netParameters.BiasMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMax
        {
            get { return _netParameters.BiasMax; }
            set
            {
                if (_netParameters.BiasMax != value)
                {
                    _netParameters.BiasMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public CostType CostType
        {
            get { return _netParameters.CostType; }
            set
            {
                if (_netParameters.CostType != value)
                {
                    _netParameters.CostType = value;
                    OnPropertyChanged();
                }
            }
        }
        public WeightInitType WeightInitType
        {
            get { return _netParameters.WeightInitType; }
            set
            {
                if (_netParameters.WeightInitType != value)
                {
                    _netParameters.WeightInitType = value;
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
            LayerVM layerVM = cp.Content as LayerVM;

            LayerVM newLayerVM = new LayerVM(new Layer() { ActivationType = ActivationType.ReLU });
            int newIndex = (LayerVMs.IndexOf(layerVM));
            LayerVMs.Insert(newIndex+1, newLayerVM);
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
            LayerVM layerVM = cp.Content as LayerVM;
            LayerVMs.Remove(layerVM);
        }
        bool DeleteCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IRelayCommand MoveLeftCommand
        {
            get
            {
                if (moveLeftCommand == null)
                {
                    moveLeftCommand = new RelayCommand(MoveLeftCommand_Execute, MoveLeftCommand_CanExecute);
                }
                return moveLeftCommand;
            }
        }
        void MoveLeftCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            LayerVM layerVM = cp.Content as LayerVM;
            int currentIndex = LayerVMs.IndexOf(layerVM);
            LayerVMs.Move(currentIndex, currentIndex > 0 ? currentIndex - 1 : 0);
        }
        bool MoveLeftCommand_CanExecute(object parameter)
        {
            return true;
        }
        public IRelayCommand MoveRightCommand
        {
            get
            {
                if (moveRightCommand == null)
                {
                    moveRightCommand = new RelayCommand(MoveRightCommand_Execute, MoveRightCommand_CanExecute);
                }
                return moveRightCommand;
            }
        }
        void MoveRightCommand_Execute(object parameter)
        {
            ContentPresenter cp = parameter as ContentPresenter;
            LayerVM layerVM = cp.Content as LayerVM;
            int currentIndex = LayerVMs.IndexOf(layerVM);
            LayerVMs.Move(currentIndex, currentIndex < LayerVMs.Count - 1 ? currentIndex + 1 : 0);
        }
        bool MoveRightCommand_CanExecute(object parameter)
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

        #region OnLayersChanged

        void OnLayerVMsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                case NotifyCollectionChangedAction.Move:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                case NotifyCollectionChangedAction.Reset:
                    // Check if input layer has changed.
                    // If 'yes': Notify 'InputValues'.
                    break;
                default:
                    break;
            }
            UpdateIndeces();
        }

        #region helpers

        void UpdateIndeces()
        {
            for (int i = 0; i < LayerVMs.Count; i++)
            {
                LayerVMs.ElementAt(i).Id = i;
                for (int k = 0; k < LayerVMs.ElementAt(i).Inputs.Count; k++)
                {
                    LayerVMs.ElementAt(i).Inputs[k] = 0f;
                }
            }            
        }

        #endregion

        #endregion

        #region OkBtnPressed

        public event OkBtnEventHandler OkBtnPressed;
        async Task OnOkBtnPressedAsync()
        {
            _netParameters.Layers = LayerVMs
                .Select(x => x.Layer)
                .ToArray();
            await OkBtnPressed?.Invoke(_netParameters);
        }

        #endregion
    }
}
