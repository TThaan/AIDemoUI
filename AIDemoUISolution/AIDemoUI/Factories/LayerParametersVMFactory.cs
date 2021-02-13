using AIDemoUI.ViewModels;
using Autofac;
using System;

namespace AIDemoUI.Factories
{
    public interface ILayerParametersVMFactory
    {
        LayerParametersVM CreateLayerParametersVM(int index);
    }

    public class LayerParametersVMFactory : ILayerParametersVMFactory
    {
        #region fields & ctor

        private readonly IServiceProvider _serviceProvider;

        public LayerParametersVMFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region ILayerParametersVMFactory

        public LayerParametersVM CreateLayerParametersVM(int index)
        {
            // Consider scope!
            var session = _serviceProvider.GetService(typeof(ISessionContext)) as ISessionContext;
            var container = _serviceProvider.GetService(typeof(IContainer)) as IContainer;
            return container.Resolve<LayerParametersVM>(
                new TypedParameter(typeof(ISessionContext), session), 
                new TypedParameter(typeof(int), index));
        }

        #endregion
    }
}
