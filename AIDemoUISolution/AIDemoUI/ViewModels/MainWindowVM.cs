using AIDemoUI.Commands;
using AIDemoUI.Views;
using DeepLearningDataProvider;
using Microsoft.Win32;
using NeuralNetBuilder;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;

namespace AIDemoUI.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        #region ctor & fields

        IRelayCommand exitCommand;
        IAsyncCommand loadParametersCommandAsync, saveInitializedNetCommandAsync, saveParametersCommandAsync, loadInitializedNetCommandAsync;

        public MainWindowVM()
        {
            NetParametersVM = new NetParametersVM(this);
            StartStopVM = new StartStopVM(this);
            StatusVM = new StatusVM(this);
            SampleImportWindowVM = new SampleImportWindowVM(this);
            SampleImportWindow = new SampleImportWindow()
            { DataContext = SampleImportWindowVM };  // ...s..

            NetParametersVM.SubViewModelChanged += StatusVM.NetParametersVM_SubViewModelChanged;    // Consider a central SubViewModelChanged handling method in MainVM?
            StartStopVM.SubViewModelChanged += StatusVM.StartStopVM_SubViewModelChanged;            // Consider a central SubViewModelChanged handling method in MainVM?

            SetDefaultValues();
        }

        #region helpers

        private void SetDefaultValues()
        {
            
        }

        #endregion

        #endregion

        #region public

        public NetParametersVM NetParametersVM { get; set; }
        public StatusVM StatusVM { get; set; }
        public StartStopVM StartStopVM { get; set; }
        public SampleImportWindowVM SampleImportWindowVM { get; set; }
        public SampleImportWindow SampleImportWindow { get; set; }
        public SampleSetParameters SampleSetParameters { get; set; }

        #endregion

        #region Commands

        public IRelayCommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand(x => Application.Current.Shutdown(), x => true);
                }
                return exitCommand;
            }
        }
        public IAsyncCommand LoadParametersCommandAsync
        {
            get
            {
                if (loadParametersCommandAsync == null)
                {
                    loadParametersCommandAsync = new AsyncRelayCommand(LoadParametersCommand_ExecuteAsync, x => true);
                }
                return loadParametersCommandAsync;
            }
        }
        async Task LoadParametersCommand_ExecuteAsync(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    SerializedParameters sp = GetSerializedParameters(openFileDialog);
                    SetLoadedValues(sp);
                });
            }
        }
        public IAsyncCommand SaveParametersCommandAsync
        {
            get
            {
                if (saveParametersCommandAsync == null)
                {
                    saveParametersCommandAsync = new AsyncRelayCommand(SaveParameters_ExecuteAsync, x => true);
                }
                return saveParametersCommandAsync;
            }
        }
        async Task SaveParameters_ExecuteAsync(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save this Template";
            saveFileDialog.DefaultExt = ".txt";
            // saveFileDialog.Filter = "Text| *.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                SerializedParameters sp = new SerializedParameters()
                {
                    NetParameters = NetParametersVM.NetParameters,
                    TrainerParameters = NetParametersVM.TrainerParameters
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
        public IAsyncCommand LoadInitializedNetCommandAsync
        {
            get
            {
                if (loadInitializedNetCommandAsync == null)
                {
                    loadInitializedNetCommandAsync = new AsyncRelayCommand(LoadInitializedNetCommand_ExecuteAsync, x => true);
                }
                return loadInitializedNetCommandAsync;
            }
        }
        async Task LoadInitializedNetCommand_ExecuteAsync(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                await Task.Run(() =>
                {
                    NetParametersVM.FileName = openFileDialog.FileName;
                    StartStopVM.Net = DeSerialize<INet>(openFileDialog.FileName);
                });
            }
        }
        public IAsyncCommand SaveInitializedNetCommandAsync
        {
            get
            {
                if (saveInitializedNetCommandAsync == null)
                {
                    saveInitializedNetCommandAsync = new AsyncRelayCommand(SaveInitializedNetCommand_ExecuteAsync, x => true);
                }
                return saveInitializedNetCommandAsync;
            }
        }
        async Task SaveInitializedNetCommand_ExecuteAsync(object parameter)
        {
            if (StartStopVM.Net == null)
            {
                MessageBox.Show("No net created yet!");
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Choose the location to save your newly created net at.";
                saveFileDialog.DefaultExt = ".dat";
                // saveFileDialog.Filter = "Text| *.txt";

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
            NetParametersVM.NetParameters = sp.NetParameters;
            NetParametersVM.LayerParameterVMs = new ObservableCollection<LayerParametersVM>(
                sp.NetParameters.LayersParameters.Select(x => new LayerParametersVM(this, x)));
            NetParametersVM.TrainerParameters = sp.TrainerParameters;

            // After loading serialized parameters each local property gets set to it's stored value.
            // So the value of AreParametersGlobal can be set to false here by default
            // and the global properties can be set to the parameters of the first layer.
            // This way they are correct in the case of AreParametersGlobal being true
            // whereas they get ignored otherwise.
            NetParametersVM.AreParametersGlobal = false;
            NetParametersVM.WeightMin_Global = sp.NetParameters.LayersParameters[0].WeightMin;
            NetParametersVM.WeightMax_Global = sp.NetParameters.LayersParameters[0].WeightMax;
            NetParametersVM.BiasMin_Global = sp.NetParameters.LayersParameters[0].BiasMin;
            NetParametersVM.BiasMax_Global = sp.NetParameters.LayersParameters[0].BiasMax;

            // Notify the UI (via properties in NetParametersVM  and LayerParametersVMs)
            // about changed values in NetParameters and LayerParameters.
            OnAllPropertiesChanged();
        }
        static void Serialize<T>(T target, string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))// + ".dat"
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

        #endregion

        #endregion
    }
}
