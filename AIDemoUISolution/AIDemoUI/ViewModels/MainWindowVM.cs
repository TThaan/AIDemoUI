using AIDemoUI.Commands;
using AIDemoUI.FactoriesAndStewards;
using Microsoft.Win32;
using NeuralNetBuilder;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AIDemoUI.ViewModels
{
    public interface IMainWindowVM : IBaseVM
    {
        INetParametersVM NetParametersVM { get; set; }
        IStartStopVM StartStopVM { get; set; }
        IStatusVM StatusVM { get; set; }

        IAsyncCommand EnterLogNameCommand { get; set; }
        IAsyncCommand LoadParametersCommand { get; set; }
        IAsyncCommand SaveParametersCommand { get; set; }
        IAsyncCommand LoadInitializedNetCommand { get; set; }
        IAsyncCommand SaveInitializedNetCommand { get; set; }
        IRelayCommand ExitCommand { get; set; }
        Task EnterLogNameAsync(object parameter);
        void LayerParametersCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
        Task LoadParametersAsync(object parameter);
        Task SaveParametersAsync(object parameter);
        Task LoadInitializedNetAsync(object parameter);
        Task SaveInitializedNetAsync(object parameter);
        void Exit(object parameter);
        void SampleSet_StatusChanged(object sender, DeepLearningDataProvider.StatusChangedEventArgs e);
        void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e);
        void Trainer_StatusChanged(object sender, StatusChangedEventArgs e);
    }

    public class MainWindowVM : BaseVM, IMainWindowVM
    {
        #region fields & ctor

        private readonly ISessionContext _sessionContext;
        // private ILayerParametersVMFactory _layerParametersFactory;
        private INetParametersVM netParametersVM;
        private IStatusVM statusVM;
        private IStartStopVM startStopVM;

        public MainWindowVM(ISessionContext sessionContext, INetParametersVM netParametersVM, IStartStopVM startStopVM, IStatusVM statusVM,
            ISimpleMediator mediator)//ILayerParametersVMFactory layerParametersVMFactory, 
            : base(mediator)
        {
            _sessionContext = sessionContext;
            NetParametersVM = netParametersVM;
            StartStopVM = startStopVM;
            StatusVM = statusVM;
            // _layerParametersFactory = layerParametersVMFactory;

            _mediator.Register("Token: MainWindowVM", MainWindowVMCallback);

            //throw new ArgumentException($"{NetParametersVM.LayerParametersVMCollection == null}");
        }
        void MainWindowVMCallback(object obj)
        {

        }


        #endregion

        #region public

        public INetParametersVM NetParametersVM
        {
            get { return netParametersVM; }
            set
            {
                if (netParametersVM != value)
                {
                    netParametersVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public IStatusVM StatusVM
        {
            get { return statusVM; }
            set
            {
                if (statusVM != value)
                {
                    statusVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public IStartStopVM StartStopVM
        {
            get { return startStopVM; }
            set
            {
                if (startStopVM != value)
                {
                    startStopVM = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Commands

        public IAsyncCommand EnterLogNameCommand { get; set; }
        public IAsyncCommand LoadParametersCommand { get; set; }
        public IAsyncCommand SaveParametersCommand { get; set; }
        public IAsyncCommand LoadInitializedNetCommand { get; set; }
        public IAsyncCommand SaveInitializedNetCommand { get; set; }
        public IRelayCommand ExitCommand { get; set; }

        #region Executes

        public async Task EnterLogNameAsync(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Enter LogName";
            saveFileDialog.Filter = "Text| *.txt";
            saveFileDialog.DefaultExt = ".txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    await Task.Run(() =>
                    {
                        StartStopVM.LogName = saveFileDialog.FileName;
                    });
                }
            }
        }
        public async Task LoadParametersAsync(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load Parameters";
            openFileDialog.Filter = "Parameters|*.par";

            if (openFileDialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        SerializedParameters sp = GetSerializedParameters(openFileDialog);
                        SetLoadedValues(sp);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"That didn't work.\n({e.Message})");
                        return;
                    }
                });
            }
        }
        public async Task SaveParametersAsync(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Parameters";
            saveFileDialog.Filter = "Parameters| *.par";
            saveFileDialog.DefaultExt = ".par";

            if (saveFileDialog.ShowDialog() == true)
            {
                SerializedParameters sp = new SerializedParameters()
                {
                    NetParameters = _sessionContext.NetParameters,
                    TrainerParameters = _sessionContext.TrainerParameters,
                    UseGlobalParameters = netParametersVM.AreParametersGlobal,
                    BiasMin_Global = netParametersVM.BiasMin_Global,
                    BiasMax_Global = netParametersVM.BiasMax_Global,
                    WeightMin_Global = netParametersVM.WeightMin_Global,
                    WeightMax_Global = netParametersVM.WeightMax_Global
                };

                if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    await Task.Run(() =>
                    {
                        Stream stream = saveFileDialog.OpenFile();
                        BinaryFormatter b = new BinaryFormatter();
                        b.Serialize(stream, sp);
                        stream.Close();
                    });
                }
            }
        }
        public async Task LoadInitializedNetAsync(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load Initialized Net";
            openFileDialog.Filter = "Initialized Net|*.net";

            if (openFileDialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        NetParametersVM.FileName = openFileDialog.FileName;
                        StartStopVM.Net = DeSerialize<INet>(openFileDialog.FileName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"That didn't work.\n({e.Message})");
                        return;
                    }
                });
            }
        }
        public async Task SaveInitializedNetAsync(object parameter)
        {
            if (StartStopVM.Net == null)
            {
                MessageBox.Show("No net created yet!");
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save Initialized Net";
                saveFileDialog.Filter = "Net| *.net";
                saveFileDialog.DefaultExt = ".net";

                if (saveFileDialog.ShowDialog() == true)
                {
                    await Task.Run(() =>
                    {
                        NetParametersVM.FileName = saveFileDialog.FileName;
                        Serialize(StartStopVM.Net, NetParametersVM.FileName);
                    });
                }
            }
        }
        public void Exit(object parameter)
        {
            Application.Current.Shutdown();
        }

        #region helpers

        private static SerializedParameters GetSerializedParameters(OpenFileDialog openFileDialog)
        {
            Stream stream = openFileDialog.OpenFile();
            BinaryFormatter b = new BinaryFormatter();
            SerializedParameters sp = (SerializedParameters)b.Deserialize(stream);
            return sp;
        }
        void SetLoadedValues(SerializedParameters sp)
        {
            _sessionContext.NetParameters = sp.NetParameters;

            // Refactor!:
            //NetParametersVM.LayerParametersVMCollection.Clear();
            //foreach (var layerParameters in sp.NetParameters.LayerParametersCollection)  // ta naming: layerParameters != LayersParameters
            //{
            //    // NetParametersVM.LayerParametersCollection.Add(_layerParametersFactory.CreateLayerParameters(NetParametersVM.LayerParametersCollection?.Last()));
            //    // NetParametersVM.LayerParametersCollection.Last().Id = layerParameters.Id;
            //}

            _sessionContext.TrainerParameters = sp.TrainerParameters;

            NetParametersVM.AreParametersGlobal = sp.UseGlobalParameters;
            NetParametersVM.WeightMin_Global = sp.WeightMin_Global;
            NetParametersVM.WeightMax_Global = sp.WeightMax_Global;
            NetParametersVM.BiasMin_Global = sp.BiasMin_Global;
            NetParametersVM.BiasMax_Global = sp.BiasMax_Global;

            // Notify the UI (via properties in NetParametersVM  and LayerParametersVMs)
            // about changed values in NetParameters and LayerParameters.
            netParametersVM.OnAllPropertiesChanged();//LayerParametersCollection-chg notified??
        }
        static void Serialize<T>(T target, string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))//
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, target);
            }
        }
        static T DeSerialize<T>(string name)
        {
            using (Stream stream = File.Open(name, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(stream);
            }
        }
        private void UpdateLocalParameters()
        {
            // NetParametersVM.LayerParametersCollection.Clear();
            //foreach (var layerParameters in LayerParametersCollection)  // ta naming: layerParameters != LayersParameters
            //{
            //    NetParametersVM.LayerParametersCollection.Add(_layerParametersFactory.CreateLayerParameters(NetParametersVM.LayerParametersCollection?.Last()));
            //    NetParametersVM.LayerParametersCollection.Last().Id = layerParameters.Id;
            //}
        }

        #endregion

        #endregion

        #endregion

        #region events

        public void SampleSet_StatusChanged(object sender, DeepLearningDataProvider.StatusChangedEventArgs e)
        {
            StatusVM.ProgressBarText = e.Info;
            Thread.Sleep(200);
        }
        public void Trainer_StatusChanged(object sender, NeuralNetBuilder.StatusChangedEventArgs e)
        {
            StatusVM.ProgressBarText = e.Info;
        }
        public void Trainer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ITrainer trainer = sender as ITrainer;

            if (trainer != null)
            {
                switch (e.PropertyName)
                {
                    case nameof(trainer.CurrentSample):
                        StatusVM.CurrentSample = trainer.CurrentSample;
                        break;
                    case nameof(trainer.CurrentTotalCost):
                        StatusVM.CurrentTotalCost = trainer.CurrentTotalCost;
                        break;
                    case nameof(trainer.LastEpochsAccuracy):
                        StatusVM.LastEpochsAccuracy = trainer.LastEpochsAccuracy;
                        StatusVM.ProgressBarText = $"Training...\n(Last Epoch's Accuracy: {trainer.LastEpochsAccuracy})";
                        break;
                    case nameof(trainer.CurrentEpoch):
                        StatusVM.CurrentEpoch = trainer.CurrentEpoch;
                        StatusVM.ProgressBarValue = StatusVM.CurrentEpoch;
                        break;
                    case nameof(trainer.LearningRate):
                        break;
                    case nameof(trainer.Epochs):
                        StatusVM.Epochs = trainer.Epochs;
                        StatusVM.ProgressBarMax = StatusVM.Epochs;
                        break;
                    case nameof(trainer.IsStarted):
                        StatusVM.ProgressBarMax = trainer.Epochs;
                        StatusVM.ProgressBarText = $"Training...\nLast Epoch's Accuracy: {StatusVM.LastEpochsAccuracy}";
                        break;
                    case nameof(trainer.IsPaused):
                        StatusVM.ProgressBarText = $"Training paused...\nLast Epoch's Accuracy: {StatusVM.LastEpochsAccuracy}";
                        break;
                    case nameof(trainer.IsFinished):
                        StatusVM.ProgressBarText = $"Training finished.\nLast Epoch's Accuracy: {StatusVM.LastEpochsAccuracy}";
                        break;
                    default:
                        break;
                }
            }
        }
        // Redundant?
        public void LayerParametersCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //NetParametersVM.NetParameters.LayersParameters = 
            //    NetParametersVM.LayerParametersCollection.Select(x => x.LayerParameters).ToArray();
        }

        #endregion
    }
}
