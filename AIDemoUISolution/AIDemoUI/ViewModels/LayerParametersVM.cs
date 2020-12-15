using AIDemoUI.Commands;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.ViewModels
{
    public class LayerParametersVM : BaseVM
    {
        #region ctor & fields

        IRelayCommand addCommand, deleteCommand, moveLeftCommand, moveRightCommand;

        public LayerParametersVM(int id)
        {
            LayerParameters = new LayerParameters();
            Id = id;
            SetDefaultValues();
        }
        public LayerParametersVM(ILayerParameters layerParameters)
        {
            LayerParameters = layerParameters;
        }

        #region helpers

        void SetDefaultValues()
        {
            N = 4;
            IsWithBias = false;
            WeightMin = -1;
            WeightMax = 1;
            BiasMin = -1;
            BiasMax = 1;
            ActivationType = ActivationType.ReLU;

            // Inputs = Enumerable.Range(0, N).Select(x => 0f).ToObservableCollection();
            // Outputs = Enumerable.Range(0, N).Select(x => 0f).ToObservableCollection();
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
        public bool IsWithBias
        {
            get { return LayerParameters.IsWithBias; }
            set
            {
                if (LayerParameters.IsWithBias != value)
                {
                    LayerParameters.IsWithBias = value;
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
            OnPropertyChanged();
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
            OnPropertyChanged();
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
            OnPropertyChanged();
            
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
            OnPropertyChanged();
        }
        bool MoveRightCommand_CanExecute(object parameter)
        {
            return true;
        }

        #endregion
    }
}
