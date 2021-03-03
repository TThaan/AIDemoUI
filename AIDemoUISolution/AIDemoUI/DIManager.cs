using AIDemoUI.Commands;
using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.SampleData;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System.ComponentModel;
using System.Linq;
using IContainer = Autofac.IContainer;

namespace AIDemoUI
{
    public class DIManager
    {
        #region fields

        IContainer container;

        #endregion

        #region public

        public IContainer Container => container ?? (container = GetContainer());

        #endregion

        #region helpers

        private IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            #region MainWindow (View Model and View)

            builder.RegisterType<MainWindowVM>()
                .SingleInstance()
                .As<IMainWindowVM>()
                .OnActivated(x =>
                {
                    x.Instance.UnfocusCommand = new RelayCommand(x.Instance.Unfocus, y => true);    // repetitive..
                    x.Instance.ExitCommand = new RelayCommand(x.Instance.Exit, y => true);
                    x.Instance.LoadParametersCommand = new AsyncRelayCommand(x.Instance.LoadParametersAsync, y => true);
                    x.Instance.SaveParametersCommand = new AsyncRelayCommand(x.Instance.SaveParametersAsync, y => true);
                    x.Instance.LoadInitializedNetCommand = new AsyncRelayCommand(x.Instance.LoadInitializedNetAsync, y => true);
                    x.Instance.SaveInitializedNetCommand = new AsyncRelayCommand(x.Instance.SaveInitializedNetAsync, y => true);
                    x.Instance.EnterLogNameCommand = new AsyncRelayCommand(x.Instance.EnterLogNameAsync, y => true);
                });
            builder.Register(x => new MainWindow()
            {
                DataContext = x.Resolve<IMainWindowVM>()
                .SetDefaultValues(x.Resolve<ISessionContext>())
            })
                .SingleInstance();

            #endregion

            #region Sample Import (View Model and View)

            builder.RegisterType<SampleImportWindowVM>()
                .SingleInstance()
                .As<ISampleImportWindowVM>()
                .OnActivated(x =>
                {
                    x.Instance.UnfocusCommand = new RelayCommand(x.Instance.Unfocus, y => true);    // repetitive..
                    x.Instance.SetSamplesLocationCommand = new AsyncRelayCommand(x.Instance.SetSamplesLocationAsync, x.Context.Resolve<ISampleImportWindowVM>().SetSamplesLocationAsync_CanExecute);
                    x.Instance.OkCommand = new AsyncRelayCommand(x.Instance.OkAsync, x.Context.Resolve<ISampleImportWindowVM>().OkAsync_CanExecute);
                    x.Instance.SelectedTemplate = x.Instance.Templates.Values.First();
                });
            builder.Register(x => new SampleImportWindow() 
            { 
                DataContext = x.Resolve<ISampleImportWindowVM>()
            })
                .SingleInstance();

            #endregion

            #region Libraries' Containers/ServiceProviders

            builder.RegisterType<NetParameters>()
                .As<INetParameters>()
                .SingleInstance();
            builder.RegisterType<LayerParameters>() // redundant?
                .As<ILayerParameters>();
            builder.RegisterType<TrainerParameters>()
                .SingleInstance()
                .As<ITrainerParameters>();
            builder.RegisterType<SampleSetSteward>()
                .OnActivated(x =>
                {
                    x.Instance.EventHandlers = new PropertyChangedEventHandler[]
                    { 
                        x.Context.Resolve<ISampleImportWindowVM>().Any_PropertyChanged
                    };
                })
                .As<ISampleSetSteward>()
                .SingleInstance();
            builder.RegisterType<SampleSetParameters>()
                .As<ISampleSetParameters>();

            builder.Register(x => Initializer.GetRawNet())  // in NetBuilder!
                .As<INet>()
                .SingleInstance();
            builder.Register(x => Initializer.GetRawTrainer()).
                OnActivated(x =>
                {
                    // Reconsider:
                    x.Instance.PropertyChanged += x.Context.Resolve<ILayerParametersVM>().Any_PropertyChanged;
                    x.Instance.PropertyChanged += x.Context.Resolve<IStartStopVM>().Any_PropertyChanged;
                    x.Instance.PropertyChanged += x.Context.Resolve<IStatusVM>().Any_PropertyChanged;
                })
                .As<ITrainer>()
                .SingleInstance();

            #endregion

            #region SessionContext

            builder.RegisterType<SessionContext>()
                .As<ISessionContext>()
                .SingleInstance();

            #endregion

            #region LayerParametersVMFactory

            builder.RegisterType<LayerParametersVMFactory>().As<ILayerParametersVMFactory>();

            #endregion

            #region LayerParametersVM

            builder.RegisterType<LayerParametersVM>()
                .As<ILayerParametersVM>()
                .OnActivated(x =>
                {
                    x.Instance.UnfocusCommand = new RelayCommand(x.Instance.Unfocus, y => true);    // repetitive..
                }); ;

            #endregion

            #region NetParametersVM

            builder.RegisterType<NetParametersVM>()
                .SingleInstance()
                .As<INetParametersVM>()
                .OnActivated(x =>
                {
                    x.Instance.UnfocusCommand = new RelayCommand(x.Instance.Unfocus, y => true);    // repetitive..
                    x.Instance.AddCommand = new RelayCommand(x.Instance.Add, y => true);
                    x.Instance.DeleteCommand = new RelayCommand(x.Instance.Delete, y => true);
                    x.Instance.MoveLeftCommand = new RelayCommand(x.Instance.MoveLeft, y => true);
                    x.Instance.MoveRightCommand = new RelayCommand(x.Instance.MoveRight, y => true);
                    x.Instance.UseGlobalParametersCommand = new AsyncRelayCommand(x.Instance.UseGlobalParametersAsync, x.Instance.UseGlobalParametersAsync_CanExecute);
                });

            // Use named collection or create in NetParameters' Registering!
            //builder.Register(x => new ObservableCollection<ILayerParametersVM>())
            //    .SingleInstance()
            //    .OnActivated(x => x.Instance.CollectionChanged += x.Context.Resolve<IMainWindowVM>().LayerParametersCollection_CollectionChanged);

            #endregion

            #region StartStopVM

            builder.RegisterType<StartStopVM>()
                .SingleInstance()
                .As<IStartStopVM>()
                .OnActivating(x =>
                {
                    x.Instance.UnfocusCommand = new RelayCommand(x.Instance.Unfocus, y => true);    // repetitive..
                    x.Instance.InitializeNetCommand = new AsyncRelayCommand(x.Instance.InitializeNetAsync, x.Instance.InitializeNetAsync_CanExecute);
                    x.Instance.ShowSampleImportWindowCommand = new AsyncRelayCommand(x.Instance.ShowSampleImportWindow, y => true);
                    x.Instance.TrainCommand = new AsyncRelayCommand(x.Instance.TrainAsync, x.Instance.TrainAsync_CanExecute);
                    x.Instance.StepCommand = new AsyncRelayCommand(x.Instance.StepAsync, x.Instance.StepAsync_CanExecute);
                });

            #endregion

            #region StatusVM

            builder.RegisterType<StatusVM>()
                .As<IStatusVM>()
                .SingleInstance()
                .OnActivated(x =>
                {
                    x.Instance.UnfocusCommand = new RelayCommand(x.Instance.Unfocus, y => true);    // repetitive..
                });

            #endregion

            #region Factories and Stewards

            builder.RegisterType<LayerParametersVMFactory>()
                .As<ILayerParametersVMFactory>();
            builder.RegisterType<LayerParametersFactory>()
                .As<ILayerParametersFactory>();
            //builder.RegisterType<SamplesSteward>()
            //    .As<ISamplesSteward>();
            builder.RegisterType<SampleSetParametersSteward>()
                .As<ISampleSetParametersSteward>();

            #endregion

            #region Sample Data

            builder.RegisterType<RawData>();

            #endregion

            #region Mediator

            builder.RegisterType<SimpleMediator>()
                .As<ISimpleMediator>()
                .SingleInstance();

            #endregion

            #region DefaultValuesInitializer

            // builder.RegisterType<DefaultValuesInitializer>();

            #endregion

            return builder.Build();
        }

        #endregion
    }
}
