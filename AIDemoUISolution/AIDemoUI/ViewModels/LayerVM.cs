using FourPixCam;
using MatrixHelper;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AIDemoUI.ViewModels
{
    public class LayerVM : BaseVM
    {
        #region ctor & fields

        //int id, n;
        //ActivationType activationType;
        ObservableCollection<float> inputLayer;
        ObservableCollection<float> inputs, outputs;
        // Matrix weights, biases;
        // IRelayCommand changeNCommand;

        public LayerVM(Layer layer)
        {
            Layer = layer ??
                throw new NullReferenceException($"" +
                $"{typeof(Layer).Name} {nameof(layer)} ({GetType().Name}.ctor)");

            SetDefaultValues(layer);
        }

        #region helpers

        void SetDefaultValues(Layer layer)
        {
            N = layer.N == 0 ? 4 : layer.N;
            ActivationType = layer.ActivationType == default ? ActivationType.ReLU : layer.ActivationType;
            Inputs = Enumerable.Range(0, N).Select(x => 0f).ToObservableCollection();
            Outputs = Enumerable.Range(0, N).Select(x => 0f).ToObservableCollection();
        }

        #endregion

        #endregion

        #region public

        public Layer Layer { get; }
        public int Id
        {
            get { return Layer.Id; }
            set
            {
                if (Layer.Id != value)
                {
                    Layer.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public int N
        {
            get { return Layer.N; }
            set
            {
                if (Layer.N != value)
                {
                    Layer.N = value;
                    OnLayerVMPropertyChanged();
                }
            }
        }
        public ActivationType ActivationType
        {
            get { return Layer.ActivationType; }
            set
            {
                if (Layer.ActivationType != value)
                {
                    Layer.ActivationType = value;
                    OnPropertyChanged();
                }
            }
        }
        public Matrix Weights
        {
            get { return Layer.Weights; }
            set
            {
                if (Layer.Weights != value)
                {
                    Layer.Weights = value;
                    OnPropertyChanged();
                }
            }
        }
        public Matrix Biases
        {
            get { return Layer.Biases; }
            set
            {
                if (Layer.Biases != value)
                {
                    Layer.Biases = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<float> InputLayer
        {
            get
            {
                return inputLayer;
            }
            set
            {
                if (inputLayer != value)
                {
                    inputLayer = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Processed

        public ObservableCollection<float> Inputs
        {
            get { return inputs; }
            set
            {
                if (inputs != value)
                {
                    inputs = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<float> Outputs
        {
            get { return outputs; }
            set
            {
                if (outputs != value)
                {
                    outputs = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #endregion

        #region RelayCommand

        //public IRelayCommand ChangeNCommand
        //{
        //    get
        //    {
        //        if (changeNCommand == null)
        //        {
        //            changeNCommand = new RelayCommand(ChangeNCommand_Execute, ChangeNCommand_CanExecute);
        //        }
        //        return changeNCommand;
        //    }
        //}
        //void ChangeNCommand_Execute(object parameter)
        //{
        //    // string text = parameter as string;
        //    // bool parsed = int.TryParse(text, out int newN);
        //    if (true)   //parsed
        //    {
        //        Inputs = Enumerable.Range(0, N)
        //            .Select(x => 0f)
        //            .ToObservableCollection();
        //    }
        //}
        //bool ChangeNCommand_CanExecute(object parameter)
        //{
        //    return true;
        //}

        #endregion

        #region OnLayersChanged

        void OnInputsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
        void OnInputLayerChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region OnLayerVMPropertyChanged

        void OnLayerVMPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(N))
            {
                Inputs = Enumerable.Range(0, N)
                    .Select(x => 0f)
                    .ToObservableCollection();
                Outputs = Enumerable.Range(0, N)
                    .Select(x => 0f)
                    .ToObservableCollection();
            }
            OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
