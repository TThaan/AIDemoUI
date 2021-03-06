﻿using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.SampleData;
using AIDemoUI.ViewModels;
using AIDemoUI.Views;
using Autofac;
using DeepLearningDataProvider;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System.ComponentModel;
using Autofac.Features.AttributeFilters;
using IContainer = Autofac.IContainer;

namespace AIDemoUI
{
    public class DIManager
    {
        #region fields

        IContainer container;

        #endregion

        #region properties

        public IContainer Container => container ?? (container = GetContainer());

        #region helpers

        private IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            #region Windows/Views

            builder.RegisterType<MainWindowVM>()
                .SingleInstance()
                .As<IMainWindowVM>();

            builder.Register(x => new SampleImportWindow()
            {
                DataContext = x.Resolve<ISampleImportWindowVM>()
            })
                .SingleInstance();

            #endregion

            #region View Models

            builder.Register(x => new MainWindow()
            {
                DataContext = x.Resolve<IMainWindowVM>()
                .SetDefaultValues(x.Resolve<ISessionContext>())
            })
                .SingleInstance();
            builder.RegisterType<NetParametersVM>()
                .SingleInstance()
                .As<INetParametersVM>();

            builder.RegisterType<LayerParametersVM>()
                .As<ILayerParametersVM>();

            builder.RegisterType<StartStopVM>()
                .SingleInstance()
                .WithAttributeFiltering()
                .As<IStartStopVM>();

            builder.RegisterType<SampleImportWindowVM>()
                .SingleInstance()
                .As<ISampleImportWindowVM>();

            builder.RegisterType<StatusVM>()
                .As<IStatusVM>()
                .SingleInstance();

            #endregion

            #region Data/Model classes

            builder.RegisterType<NetParameters>()
                .As<INetParameters>()
                .SingleInstance();
            builder.RegisterType<LayerParameters>()
                .As<ILayerParameters>();
            builder.RegisterType<TrainerParameters>()
                .SingleInstance()
                .As<ITrainerParameters>();
            builder.RegisterType<SampleSetParameters>()
                .As<ISampleSetParameters>();

            builder.Register(x => Initializer.GetRawNet())
                .As<INet>()
                .SingleInstance();
            builder.Register(x => Initializer.GetRawTrainer()).
                OnActivated(x =>
                {
                    // Reconsider:
                    x.Instance.PropertyChanged += x.Context.Resolve<ILayerParametersVM>().Any_PropertyChanged;
                    x.Instance.PropertyChanged += x.Context.Resolve<IStartStopVM>().Any_PropertyChanged;
                    x.Instance.PropertyChanged += x.Context.Resolve<IStatusVM>().Any_PropertyChanged;
                    x.Instance.TrainerStatusChanged += x.Context.Resolve<IStartStopVM>().Trainer_TrainerStatusChanged;
                })
                .As<ITrainer>()
                .SingleInstance();

            #endregion

            #region Factories and Stewards

            builder.RegisterType<DelegateFactory>()
                .As<IDelegateFactory>();

            // Include in DelegateFactory?:
            builder.RegisterType<LayerParametersVMFactory>()
                .As<ILayerParametersVMFactory>();
            builder.RegisterType<LayerParametersFactory>()
                .As<ILayerParametersFactory>();
            builder.RegisterType<SampleSetParametersSteward>()
                .As<ISampleSetParametersSteward>();
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

            #endregion

            #region Others

            builder.RegisterType<SessionContext>()
                .As<ISessionContext>()
                .SingleInstance();

            builder.RegisterType<SimpleMediator>()
                .As<ISimpleMediator>()
                .SingleInstance();

            builder.RegisterType<RawData>();

            #endregion

            return builder.Build();
        }

        #endregion

        #endregion
    }
}
