using AIDemoUI.ViewModels;
using Autofac;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.FactoriesAndStewards
{
    public interface ILayerParametersVMFactory
    {
        ILayerParametersVM CreateLayerParametersVM(ILayerParameters layerParameters);
    }

    public class LayerParametersVMFactory : ILayerParametersVMFactory
    {
        #region fields & ctor

        private readonly IComponentContext _context;

        public LayerParametersVMFactory(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        #region ILayerParametersVMFactory

        public ILayerParametersVM CreateLayerParametersVM(ILayerParameters layerParameters)
        {
            // Consider scope!
            var result = _context.Resolve<ILayerParametersVM>();
            result.LayerParameters = layerParameters;

            return result;
        }

        #endregion
    }
}
