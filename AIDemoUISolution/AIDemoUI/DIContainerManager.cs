using AIDemoUI.Commands;
using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.SampleData;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AIDemoUI
{
    /// <summary>
    /// Use file (json?) with default values??
    /// </summary>
    public class DIContainerManager
    {
        static IContainer container;
        public static IContainer Container => container ?? (container = GetContainer());

        private static IContainer GetContainer()
        {
            #region Autofac

            var builder = new ContainerBuilder();
            // builder.Populate(serviceCollection);    // After registering all services!  // Later??

            #region Parameters

            // List<Parameter> allParameters = new List<Parameter>();
            // allParameters.Add(new NamedParameter("setName_FourPixelCamera", SetName.FourPixelCamera));

            #endregion

            #region Libraries' Containers/ServiceProviders

            builder.RegisterType<NetParameters>().As<INetParameters>().SingleInstance();
            builder.RegisterType<LayerParameters>().As<ILayerParameters>();
            builder.RegisterType<TrainerParameters>().As<ITrainerParameters>().SingleInstance();    // Single?

            // Or SampleSetFactory?
            // builder.RegisterType<SampleSet>();  // .AsSelf()?
            builder.Register(x => new SamplesSteward(x.Resolve<IMainWindowVM>().SampleSet_StatusChanged))
                .As<ISamplesSteward>();
            builder.RegisterType<SampleSetParameters>()
                .As<ISampleSetParameters>();

            #endregion

            #region SessionContext

            // builder.RegisterType<SessionContext>().As<ISessionContext>().SingleInstance();

            builder.RegisterType<LayerParametersFactory>().As<ILayerParametersFactory>();

            #endregion

            #region BaseVM

            builder.RegisterType<BaseVM>()
                .OnActivated(x =>
                {
                    x.Instance.UnfocusCommand = new RelayCommand(x.Instance.Unfocus, y => true);
                });

            #endregion

            #region MainWindowVM

            builder.RegisterType<MainWindowVM>()
                .SingleInstance()
                .As<IMainWindowVM>()
                .OnActivated(x =>
                {
                    x.Instance.ExitCommand = new RelayCommand(x.Instance.Exit, y => true);
                    x.Instance.LoadParametersCommand = new AsyncRelayCommand(x.Instance.LoadParametersAsync, y => true);
                    x.Instance.SaveParametersCommand = new AsyncRelayCommand(x.Instance.SaveParametersAsync, y => true);
                    x.Instance.LoadInitializedNetCommand = new AsyncRelayCommand(x.Instance.LoadInitializedNetAsync, y => true);
                    x.Instance.SaveInitializedNetCommand = new AsyncRelayCommand(x.Instance.SaveInitializedNetAsync, y => true);
                    x.Instance.EnterLogNameCommand = new AsyncRelayCommand(x.Instance.EnterLogNameAsync, y => true);
                });

            #endregion

            #region NetParametersVM

            builder.RegisterType<NetParametersVM>()
                .SingleInstance()
                .As<INetParametersVM>()
                .OnActivating(x =>
                {
                    x.Instance.AreParametersGlobal = true;
                    x.Instance.WeightMin_Global = -1;
                    x.Instance.WeightMax_Global = 1;
                    x.Instance.BiasMin_Global = 0;
                    x.Instance.BiasMax_Global = 0;
                    x.Instance.CostType = CostType.SquaredMeanError;
                    x.Instance.WeightInitType = WeightInitType.Xavier;
                    x.Instance.LearningRate = .1f;
                    x.Instance.LearningRateChange = .9f;
                    x.Instance.EpochCount = 10;
                })
                .OnActivated(x =>
                {
                    x.Instance.AddCommand = new RelayCommand(x.Instance.Add, y => true);
                    x.Instance.DeleteCommand = new RelayCommand(x.Instance.Delete, y => true);
                    x.Instance.MoveLeftCommand = new RelayCommand(x.Instance.MoveLeft, y => true);
                    x.Instance.MoveRightCommand = new RelayCommand(x.Instance.MoveRight, y => true);
                });

            builder.Register(x =>
            {
                var result = new ObservableCollection<ILayerParameters>()
                {
                    new LayerParameters(){Id = 0, NeuronsPerLayer = 4, ActivationType=ActivationType.NullActivator, BiasMin = 0, BiasMax = 0, WeightMin = -1, WeightMax = 1},
                    new LayerParameters(){Id = 1, NeuronsPerLayer = 8, ActivationType=ActivationType.Tanh, BiasMin = 0, BiasMax = 0, WeightMin = -1, WeightMax = 1},
                    new LayerParameters(){Id = 2, NeuronsPerLayer = 4, ActivationType=ActivationType.LeakyReLU, BiasMin = 0, BiasMax = 0, WeightMin = -1, WeightMax = 1}
                };
                return result;
            })
                .SingleInstance()
                .OnActivated(x => x.Instance.CollectionChanged += x.Context.Resolve<IMainWindowVM>().LayerParametersCollection_CollectionChanged);

            #endregion

            #region StartStopVM

            builder.RegisterType<StartStopVM>()
                .SingleInstance()
                .As<IStartStopVM>()
                .OnActivated(x =>
                {
                    x.Instance.InitializeNetCommand = new AsyncRelayCommand(x.Instance.InitializeNetAsync, y => true);
                    x.Instance.ShowSampleImportWindowCommand = new RelayCommand(x.Instance.ShowSampleImportWindow, y => true);
                    x.Instance.TrainCommand = new AsyncRelayCommand(x.Instance.TrainAsync, x.Context.Resolve<IStartStopVM>().TrainAsync_CanExecute);
                    x.Instance.IsStarted = false;
                    x.Instance.IsLogged = false;
                    x.Instance.LogName = Path.GetTempPath() + "AIDemoUI.txt";
                });

            #endregion

            #region StatusVM

            builder.RegisterType<StatusVM>()
                .As<IStatusVM>()
                .SingleInstance()
                .OnActivated(x =>
                {
                    x.Instance.ProgressBarValue = 0;
                    x.Instance.ProgressBarMax = 100;
                    x.Instance.ProgressBarText = "Wpf AI Demo";
                });//

            #endregion

            #region Sample Import (View and VM)

            builder.RegisterType<SampleImportWindowVM>()
                .SingleInstance()
                .As<ISampleImportWindowVM>()
                .OnActivated(x =>
                {
                    x.Instance.SetSamplesLocationCommand = new AsyncRelayCommand(x.Instance.SetSamplesLocationAsync, x.Context.Resolve<ISampleImportWindowVM>().SetSamplesLocationAsync_CanExecute);
                    x.Instance.OkCommand = new AsyncRelayCommand(x.Instance.OkAsync, x.Context.Resolve<ISampleImportWindowVM>().OkAsync_CanExecute);
                    x.Instance.SelectedTemplate = x.Instance.Templates.Values.First();
                });
            builder.Register(x => new SampleImportWindow() { DataContext = x.Resolve<ISampleImportWindowVM>() }).SingleInstance();   //hm..

            #endregion

            #region Factories and Stewards

            builder.RegisterType<LayerParametersFactory>()
                .As<ILayerParametersFactory>();
            builder.RegisterType<SamplesSteward>()
                .As<ISamplesSteward>();
            builder.RegisterType<SampleSetParametersSteward>()
                .As<ISampleSetParametersSteward>();

            #endregion

            #region Sample Data

            builder.RegisterType<SampleDataBootStrapper>();

            #endregion

            #region Mediator

            builder.RegisterType<SimpleMediator>()
                .As<ISimpleMediator>();

            #endregion

            #region Event Handlers

            // Single?
            // builder.Register(x => new DeepLearningDataProvider.StatusChangedEventHandler(x.Resolve<IMainWindowVM>().SampleSet_StatusChanged));//.Named<StatusChangedEventHandler>("sampleSet_StatusChanged")
            // builder.Register(x => new NotifyCollectionChangedEventHandler(x.Resolve<IMainWindowVM>().LayerParametersVMCollection_CollectionChanged));//.Named<NotifyCollectionChangedEventHandler>("layerParametersVMCollection_StatusChanged")

            #endregion

            #endregion

            return builder.Build();
        }
    }
}
