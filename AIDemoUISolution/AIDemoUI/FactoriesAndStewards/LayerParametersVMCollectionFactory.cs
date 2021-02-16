using AIDemoUI.ViewModels;
using Autofac;
using System;
using System.Collections.ObjectModel;

namespace AIDemoUI.FactoriesAndStewards
{
    public interface ILayerParametersVMCollectionFactory
    {
        ObservableCollection<LayerParametersVM> CreateLayerParametersVMs();
    }

    public class LayerParametersVMCollectionFactory : ILayerParametersVMCollectionFactory
    {
            #region fields & ctor

            private readonly IServiceProvider _serviceProvider;

            public LayerParametersVMCollectionFactory(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            #endregion

            #region ILayerParametersVMCollectionFactory

            public ObservableCollection<LayerParametersVM> CreateLayerParametersVMs()
            {
                // Consider scope!
                var container = _serviceProvider.GetService(typeof(IContainer)) as IContainer;
                return container.Resolve<ObservableCollection<LayerParametersVM>>();
            }

            #endregion
        }
    }
