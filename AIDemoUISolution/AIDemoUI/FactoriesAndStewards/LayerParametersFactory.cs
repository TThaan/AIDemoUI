using Autofac;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.FactoriesAndStewards
{
    public interface ILayerParametersFactory
    {
        ILayerParameters CreateLayerParameters();
        ILayerParameters CloneLayerParameters(ILayerParameters layerParameters);
    }

    public class LayerParametersFactory : ILayerParametersFactory
    {
        #region fields & ctor

        private readonly IComponentContext _context;

        public LayerParametersFactory(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        #region ILayerParametersVMFactory

        /// <summary>
        /// Create default LayerParameters with Id = 0.
        /// </summary>
        public ILayerParameters CreateLayerParameters()
        {
            // Consider scope!
            var result = _context.Resolve<ILayerParameters>();
            result.Id = 0;

            return result;
        }
        /// <summary>
        /// Creates new LayerParameters with the same values as the injected LayerParameters
        /// but a higher id.
        /// </summary>
        public ILayerParameters CloneLayerParameters(ILayerParameters layerParameters)
        {
            // Consider scope!
            var result = _context.Resolve<ILayerParameters>();
            result.Id = layerParameters.Id + 1;

            result.NeuronsPerLayer = layerParameters.NeuronsPerLayer;
            result.ActivationType = layerParameters.ActivationType;
            result.BiasMin = layerParameters.BiasMin;
            result.BiasMax = layerParameters.BiasMax;
            result.WeightMin = layerParameters.WeightMin;
            result.WeightMax = layerParameters.WeightMax;

            return result;
        }

        #endregion
    }
}
