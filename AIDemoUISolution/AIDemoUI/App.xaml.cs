using AIDemoUI.Commands;
using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.SampleData;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DeepLearningDataProvider;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace AIDemoUI
{
    public partial class App : Application
    {
        private static IContainer Container { get; set; }
        IServiceProvider ServiceProvider { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            #region ServiceProvider

            // var serviceCollection = new ServiceCollection();
            // serviceCollection.AddScoped(typeof(ISessionContext), typeof(SessionContext));

            #endregion

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
            builder.Register(x => new SamplesSteward(x.Resolve<MainWindowVM>().SampleSet_StatusChanged))
                .As<ISamplesSteward>();
            builder.RegisterType<SampleSetParameters>();

            #endregion

            #region SessionContext

            builder.RegisterType<SessionContext>().As<ISessionContext>().SingleInstance();

            builder.RegisterType<LayerParametersVMFactory>().As<ILayerParametersVMFactory>();

            #endregion

            #region BaseVM

            builder.RegisterType<BaseVM>()
                .OnActivated(x =>
                {
                    x.Instance.UnfocusCommand = new RelayCommand(x.Context.Resolve<BaseVM>().Unfocus, y => true);
                });

            #endregion

            #region MainWindowVM

            builder.RegisterType<MainWindowVM>().SingleInstance()
                .OnActivated(x =>
                {
                    x.Instance.ExitCommand = new RelayCommand(x.Context.Resolve<MainWindowVM>().Exit, y => true);
                    x.Instance.LoadParametersCommand = new AsyncRelayCommand(x.Context.Resolve<MainWindowVM>().LoadParametersAsync, y => true);
                    x.Instance.SaveParametersCommand = new AsyncRelayCommand(x.Context.Resolve<MainWindowVM>().SaveParametersAsync, y => true);
                    x.Instance.LoadInitializedNetCommand = new AsyncRelayCommand(x.Context.Resolve<MainWindowVM>().LoadInitializedNetAsync, y => true);
                    x.Instance.SaveInitializedNetCommand = new AsyncRelayCommand(x.Context.Resolve<MainWindowVM>().SaveInitializedNetAsync, y => true);
                    x.Instance.EnterLogNameCommand = new AsyncRelayCommand(x.Context.Resolve<MainWindowVM>().EnterLogNameAsync, y => true);
                });

            #endregion

            #region NetParametersVM

            builder.RegisterType<NetParametersVM>().SingleInstance();

            builder.Register(x => 
            {
                var result = new ObservableCollection<LayerParametersVM>();//ILayerParametersVM // Use Factory..?
                return result;
            })
                .SingleInstance()
                .OnActivated(x => x.Instance.CollectionChanged += x.Context.Resolve<MainWindowVM>().LayerParametersVMCollection_CollectionChanged);

            #endregion

            #region LayerParametersVM

            builder.RegisterType<LayerParametersVM>()//.As<ILayerParametersVM>()
                .OnActivated(x =>
                {
                    x.Instance.AddCommand = new RelayCommand(x.Instance.Add, y => true);
                    x.Instance.DeleteCommand = new RelayCommand(x.Instance.Delete, y => true);
                    x.Instance.MoveLeftCommand = new RelayCommand(x.Instance.MoveLeft, y => true);
                    x.Instance.MoveRightCommand = new RelayCommand(x.Instance.MoveRight, y => true);
                });

            #endregion

            #region StartStopVM

            builder.RegisterType<StartStopVM>().SingleInstance()
                .OnActivated(x=>
                {
                    x.Instance.InitializeNetCommand = new AsyncRelayCommand(x.Context.Resolve<StartStopVM>().InitializeNetAsync, y => true);
                    x.Instance.ShowSampleImportWindowCommand = new RelayCommand(x.Context.Resolve<StartStopVM>().ShowSampleImportWindow, y => true);
                    x.Instance.TrainCommand = new AsyncRelayCommand(x.Context.Resolve<StartStopVM>().TrainAsync, x.Context.Resolve<StartStopVM>().TrainAsync_CanExecute);
                });

            #endregion

            #region StatusVM

            builder.RegisterType<StatusVM>().SingleInstance();//.As<IStatusVM>()

            #endregion

            #region Sample Import (View and VM)

            builder.RegisterType<SampleImportWindowVM>().SingleInstance()
                .OnActivated(x =>
                {
                    x.Instance.SetSamplesLocationCommand = new AsyncRelayCommand(x.Context.Resolve<SampleImportWindowVM>().SetSamplesLocationAsync, x.Context.Resolve<SampleImportWindowVM>().SetSamplesLocationAsync_CanExecute);
                    x.Instance.OkCommand = new AsyncRelayCommand(x.Context.Resolve<SampleImportWindowVM>().OkAsync, x.Context.Resolve<SampleImportWindowVM>().OkAsync_CanExecute);
                });
            builder.Register(x => new SampleImportWindow() { DataContext = x.Resolve<SampleImportWindowVM>()}).SingleInstance();

            #endregion

            #region Factories and Stewards

            builder.RegisterType<LayerParametersVMFactory>();
            builder.RegisterType<SamplesSteward>().As<ISamplesSteward>();
            builder.RegisterType<SampleSetParametersSteward>().As<ISampleSetParametersSteward>();

            #endregion

            #region Sample Data

            builder.RegisterType<SampleDataBootStrapper>();

            #endregion

            #region Mediator

            builder.RegisterType<SimpleMediator>();

            #endregion

            #region Event Handlers

            // Single?
            // builder.Register(x => new DeepLearningDataProvider.StatusChangedEventHandler(x.Resolve<MainWindowVM>().SampleSet_StatusChanged));//.Named<StatusChangedEventHandler>("sampleSet_StatusChanged")
            // builder.Register(x => new NotifyCollectionChangedEventHandler(x.Resolve<MainWindowVM>().LayerParametersVMCollection_CollectionChanged));//.Named<NotifyCollectionChangedEventHandler>("layerParametersVMCollection_StatusChanged")

            #endregion

            Container = builder.Build();
            
            ServiceProvider = new AutofacServiceProvider(Container);

            //var mainWindowVM = Container.Resolve<MainWindowVM>();

            #region Show

            using (var scope = Container.BeginLifetimeScope())
            {
                var mainWindowVM = scope.Resolve<MainWindowVM>();
                MainWindow = new MainWindow() { DataContext = mainWindowVM };
                MainWindow.Show();
            }

            #endregion

            #endregion

        }
    }
}
