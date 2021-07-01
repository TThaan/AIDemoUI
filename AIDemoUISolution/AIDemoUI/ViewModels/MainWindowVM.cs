using AIDemoUI.Commands;
using AIDemoUI.Commands.Async;
using Microsoft.Win32;
using NeuralNetBuilder;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Linq;

namespace AIDemoUI.ViewModels
{
    public interface IMainWindowVM : IBaseVM
    {
        INetParametersVM NetParametersVM { get; set; }
        IStartStopVM StartStopVM { get; set; }
        IStatusVM StatusVM { get; set; }

        IAsyncCommand EnterLogNameCommand { get; }
        IAsyncCommand LoadParametersCommand { get; }
        IAsyncCommand SaveParametersCommand { get; }
        IAsyncCommand LoadInitializedNetCommand { get; }
        IAsyncCommand SaveInitializedNetCommand { get; }
        ICommand ExitCommand { get; }
    }

    public class MainWindowVM : BaseVM, IMainWindowVM
    {
        #region fields & ctor

        private INetParametersVM netParametersVM;
        private IStatusVM statusVM;
        private IStartStopVM startStopVM;

        public MainWindowVM(ISessionContext sessionContext, INetParametersVM netParametersVM, IStartStopVM startStopVM, 
            IStatusVM statusVM, ISimpleMediator mediator)
            : base(sessionContext, mediator)
        {
            NetParametersVM = netParametersVM;
            StartStopVM = startStopVM;
            StatusVM = statusVM;

            DefineCommands();
        }

        #region helpers

        private void DefineCommands()
        {
            ExitCommand = new SimpleRelayCommand(Exit);
            LoadParametersCommand = new SimpleAsyncRelayCommand(LoadParametersAsync);
            SaveParametersCommand = new SimpleAsyncRelayCommand(SaveParametersAsync);
            LoadInitializedNetCommand = new SimpleAsyncRelayCommand(LoadInitializedNetAsync);
            SaveInitializedNetCommand = new SimpleAsyncRelayCommand(SaveInitializedNetAsync);
            EnterLogNameCommand = new SimpleAsyncRelayCommand(EnterLogNameAsync);
        }

        #endregion

        #endregion

        #region properties

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

        public IAsyncCommand EnterLogNameCommand { get; private set; }
        public IAsyncCommand LoadParametersCommand { get; private set; }
        public IAsyncCommand SaveParametersCommand { get; private set; }
        public IAsyncCommand LoadInitializedNetCommand { get; private set; }
        public IAsyncCommand SaveInitializedNetCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        #region Executes

        private async Task EnterLogNameAsync(object parameter)
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
        private async Task LoadParametersAsync(object parameter)
        {
            await Task.Run(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Load Parameters";
                openFileDialog.Filter = "Parameters|*.par";

                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        ISerializedParameters sp = GetSerializedParameters(openFileDialog.OpenFile());
                        SetLoadedValues(sp);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"That didn't work.\n({e.Message})");
                        return;
                    }
                }
            });
        }
        private async Task SaveParametersAsync(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Parameters";
            saveFileDialog.Filter = "Parameters| *.par";
            saveFileDialog.DefaultExt = ".par";

            if (saveFileDialog.ShowDialog() == true)
            {
                ISerializedParameters sp = new SerializedParameters()
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

                    // Json Alternative:
                    var check = saveFileDialog.SafeFileName;
                    var jsonString = JsonConvert.SerializeObject(sp, Formatting.Indented);
                    await File.WriteAllTextAsync(saveFileDialog.FileName.Split('.').First() + ".txt", jsonString);
                }
            }
        }
        private async Task LoadInitializedNetAsync(object parameter)
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
                        _sessionContext.Net = DeSerialize<INet>(openFileDialog.FileName);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"That didn't work.\n({e.Message})");
                        return;
                    }
                });
            }
        }
        private async Task SaveInitializedNetAsync(object parameter)
        {
            if (_sessionContext.Net == null)
            {
                MessageBox.Show("No net initialized yet!");
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
                        Serialize(_sessionContext.Net, NetParametersVM.FileName);
                    });
                }

                await SerializeAsJsonASync(_sessionContext.Net, NetParametersVM.FileName);
            }
        }
        private void Exit(object parameter)
        {
            Application.Current.Shutdown();
        }

        #region helpers

        private ISerializedParameters GetSerializedParameters(Stream stream)
        {
            BinaryFormatter b = new BinaryFormatter();
            ISerializedParameters sp = (ISerializedParameters)b.Deserialize(stream);
            return sp;
        }
        private void SetLoadedValues(ISerializedParameters sp)
        {
            try
            {
                _sessionContext.NetParameters = sp.NetParameters;
                _sessionContext.TrainerParameters = sp.TrainerParameters;

                NetParametersVM.AreParametersGlobal = sp.UseGlobalParameters;
                NetParametersVM.WeightMin_Global = sp.WeightMin_Global;
                NetParametersVM.WeightMax_Global = sp.WeightMax_Global;
                NetParametersVM.BiasMin_Global = sp.BiasMin_Global;
                NetParametersVM.BiasMax_Global = sp.BiasMax_Global;

                netParametersVM.OnAllPropertiesChanged();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Net- and Trainer-Parameters could not be loaded.\n({e.Message})");
                return;
            }
        }
        private void Serialize<T>(T target, string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))//
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, target);
            }
        }
        private async Task SerializeAsJsonASync(object obj, string fileName)
        {
            var jsonString = JsonConvert.SerializeObject(obj);
            var tmpPath = Path.GetTempPath();
            var path = Path.Combine(tmpPath, fileName);
            await File.WriteAllTextAsync(path, jsonString);
        }
        private T DeSerialize<T>(string name)
        {
            using (Stream stream = File.Open(name, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(stream);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
