using AIDemoUI.Commands;
using AIDemoUI.Factories;
using AIDemoUI.SampleData;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped(typeof(ISessionContext), typeof(SessionContext));

            #endregion

            #region Autofac

            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);    // After registering all services!  // Later??

            #region Libraries' Containers/ServiceProviders

            builder.RegisterType<NetParameters>().As<INetParameters>().SingleInstance();
            builder.RegisterType<LayerParameters>().As<ILayerParameters>();
            builder.RegisterType<TrainerParameters>().As<ITrainerParameters>().SingleInstance();    // Single?

            #endregion

            #region SessionContext

            builder.RegisterType<SessionContext>().As<ISessionContext>().SingleInstance();

            builder.RegisterType<LayerParametersVMFactory>().As<ILayerParametersVMFactory>();

            #endregion

            #region MainWindowVM

            builder.RegisterType<MainWindowVM>().SingleInstance();

            builder.Register(x => new RelayCommand(x.Resolve<MainWindowVM>().Exit, y => true))
                .Named<IRelayCommand>("ExitCommand");
            builder.Register(x => new AsyncRelayCommand(x.Resolve<MainWindowVM>().LoadParametersAsync, y => true))
                .Named<IAsyncCommand>("LoadParametersCommandAsync");
            builder.Register(x => new AsyncRelayCommand(x.Resolve<MainWindowVM>().SaveParametersAsync, y => true))
                .Named<IAsyncCommand>("SaveParametersCommandAsync");
            builder.Register(x => new AsyncRelayCommand(x.Resolve<MainWindowVM>().LoadInitializedNetAsync, y => true))
                .Named<IAsyncCommand>("LoadInitializedNetCommandAsync");
            builder.Register(x => new AsyncRelayCommand(x.Resolve<MainWindowVM>().SaveInitializedNetAsync, y => true))
                .Named<IAsyncCommand>("SaveInitializedNetCommandAsync");
            builder.Register(x => new AsyncRelayCommand(x.Resolve<MainWindowVM>().EnterLogNameAsync, y => true))
                .Named<IAsyncCommand>("EnterLogNameCommandAsync");

            #endregion

            #region NetParametersVM

            builder.RegisterType<NetParametersVM>().SingleInstance();

            builder.Register(x => 
            {
                var result = new ObservableCollection<LayerParametersVM>();//ILayerParametersVM // Use Factory..?
                result.CollectionChanged += x.Resolve<NetParametersVM>().LayerParametersVMs_CollectionChanged;
                return result;
            })
                .SingleInstance();

            #endregion

            #region LayerParametersVM

            builder.RegisterType<LayerParametersVM>();//.As<ILayerParametersVM>()

            builder.Register(x => new RelayCommand(x.Resolve<LayerParametersVM>().Add, x.Resolve<LayerParametersVM>().Add_CanExecute))
                .As<IRelayCommand>();

            #endregion

            #region StartStopVM

            builder.RegisterType<StartStopVM>().SingleInstance();

            #endregion

            #region StatusVM

            builder.RegisterType<StatusVM>().SingleInstance();//.As<IStatusVM>()

            #endregion

            #region Sample Import (View and VM)

            builder.RegisterType<SampleImportWindowVM>().SingleInstance();
            builder.Register(x => new SampleImportWindow() { DataContext = x.Resolve<SampleImportWindow>()}).SingleInstance();

            #endregion

            #region Factories

            builder.RegisterType<LayerParametersVMFactory>();

            #endregion

            #region Sample Data

            builder.RegisterType<SampleDataBootStrapper>();

            #endregion

            Container = builder.Build();
            
            new AutofacServiceProvider(Container);

            var mainWindowVM = Container.Resolve<MainWindowVM>();

            #region Show

            using (var scope = Container.BeginLifetimeScope())
            {
                //var mainWindowVM = scope.Resolve<MainWindowVM>();

                MainWindow = new MainWindow() { DataContext = mainWindowVM };
                MainWindow.Show();
            }

            #endregion

            #endregion

        }
    }
}
