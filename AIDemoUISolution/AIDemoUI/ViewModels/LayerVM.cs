using FourPixCam;
using MatrixHelper;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AIDemoUI.ViewModels
{
    public class LayerVM : BaseVM
    {
        #region ctor & fields

        int n;
        ObservableCollection<float> inputs, outputs;

        public LayerVM(int id, int n, ActivationType activationType)//Layer layer
        {
            //Layer = layer ??
            //    throw new NullReferenceException($"" +
            //    $"{typeof(Layer).Name} {nameof(layer)} ({GetType().Name}.ctor)");

            Layer = new Layer(id, n, activationType);
            SetDefaultValues(Layer);
        }

        #region helpers

        void SetDefaultValues(Layer layer)
        {
            ActivationType = layer.ActivationType == default ? ActivationType.ReLU : layer.ActivationType;
            Inputs = Enumerable.Range(0, N).Select(x => 0f).ToObservableCollection();
            Outputs = Enumerable.Range(0, N).Select(x => 0f).ToObservableCollection();
            N = layer.N == 0 ? 4 : layer.N;
        }

        #endregion

        #endregion

        #region public

        public Layer Layer { get; }
        public int Id { get; set; }
        public int N
        {
            get { return n; }
            set
            {
                if (n != value)
                {
                    int diff = value - n;
                    n = value;

                    if (diff > 0)
                    {
                        for (int i = 0; i < diff; i++)
                        {
                            Inputs.Add(0);
                            Outputs.Add(0);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < diff; i++)
                        {
                            Inputs.RemoveAt(Inputs.Count - 1);
                            Outputs.RemoveAt(Inputs.Count - 1);
                        }
                    }
                    
                    OnPropertyChanged();
                }
            }
        }
        public ActivationType ActivationType { get; set; }
        public Matrix Weights { get; set; }
        public Matrix Biases { get; set; }

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

        #region OnLayersChanged (redundant?)

        public void OnLayerUpdate()
        {
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                for (int j = 0; j < Layer.Processed.Input.m; j++)
                {
                    float a = Layer.Processed.Input[j];
                    float b = Layer.Processed.Output[j];

                    Inputs[j] = a;
                    Outputs[j] = b;
                }
                Biases?.ForEach(Layer.Biases, x => x);
                Weights?.ForEach(Layer.Weights, x => x);
            });
        }
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
