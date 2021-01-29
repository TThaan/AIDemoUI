using AIDemoUI.Commands;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.ViewModels
{
    public class LayerParametersVM : BaseSubVM
    {
        #region fields & ctor

        IRelayCommand addCommand, deleteCommand, moveLeftCommand, moveRightCommand;

        public LayerParametersVM(MainWindowVM mainVM, int id)
            : base(mainVM)
        {
            LayerParameters = new LayerParameters();
            Id = id;
            SetDefaultValues();
        }
        public LayerParametersVM(MainWindowVM mainVM, ILayerParameters layerParameters)
            : base(mainVM)
        {
            LayerParameters = layerParameters;
        }

        #region helpers

        void SetDefaultValues()
        {
            N = 4;
            WeightMin = -1;
            WeightMax = 1;
            BiasMin = 0;
            BiasMax = 0;
            ActivationType = ActivationType.ReLU;
        }

        #endregion

        #endregion

        #region public

        public ILayerParameters LayerParameters { get; }
        public int Id
        {
            get { return LayerParameters.Id; }
            set
            {
                if (LayerParameters.Id != value)
                {
                    LayerParameters.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public int N
        {
            get { return LayerParameters.NeuronsPerLayer; }
            set
            {
                if (LayerParameters.NeuronsPerLayer != value)
                {
                    LayerParameters.NeuronsPerLayer = value;                    
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMin
        {
            get { return LayerParameters.WeightMin; }
            set
            {
                if (LayerParameters.WeightMin != value)
                {
                    LayerParameters.WeightMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float WeightMax
        {
            get { return LayerParameters.WeightMax; }
            set
            {
                if (LayerParameters.WeightMax != value)
                {
                    LayerParameters.WeightMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMin
        {
            get { return LayerParameters.BiasMin; }
            set
            {
                if (LayerParameters.BiasMin != value)
                {
                    LayerParameters.BiasMin = value;
                    OnPropertyChanged();
                }
            }
        }
        public float BiasMax
        {
            get { return LayerParameters.BiasMax; }
            set
            {
                if (LayerParameters.BiasMax != value)
                {
                    LayerParameters.BiasMax = value;
                    OnPropertyChanged();
                }
            }
        }
        public ActivationType ActivationType
        {
            get { return LayerParameters.ActivationType; }
            set
            {
                if (LayerParameters.ActivationType != value)
                {
                    LayerParameters.ActivationType = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region LayerDetails Commands

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
            // OnPropertyChanged();
            int newIndex = Id + 1;
            LayerParametersVM newLayerParametersVM = new LayerParametersVM(_mainVM, newIndex);
            // newLayerParametersVM.PropertyChanged += LayerParametersVM_PropertyChanged;
            _mainVM.NetParametersVM.LayerParameterVMs.Insert(newIndex, newLayerParametersVM);
            AdjustIdsToNewPositions();
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
            // wa LayerParameters & ..VMs as IDisposable?
            // OnPropertyChanged();
            _mainVM.NetParametersVM.LayerParameterVMs.Remove(this);
            AdjustIdsToNewPositions();
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
            // OnPropertyChanged();
            int currentIndex = Id;
            _mainVM.NetParametersVM.LayerParameterVMs.Move(
                currentIndex, currentIndex > 0 ? currentIndex - 1 : 0);
            AdjustIdsToNewPositions();
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
            // OnPropertyChanged();
            int currentIndex = Id;
            _mainVM.NetParametersVM.LayerParameterVMs.Move(
                currentIndex, currentIndex < _mainVM.NetParametersVM.LayerParameterVMs.Count - 1 ? currentIndex + 1 : 0);
            AdjustIdsToNewPositions();
        }
        bool MoveRightCommand_CanExecute(object parameter)
        {
            return true;
        }

        #region helpers

        private void AdjustIdsToNewPositions()
        {
            for (int i = 0; i < _mainVM.NetParametersVM.LayerParameterVMs.Count; i++)
            {
                _mainVM.NetParametersVM.LayerParameterVMs[i].Id = i;
            }
        }

        #endregion
        
        #endregion
    }
}
