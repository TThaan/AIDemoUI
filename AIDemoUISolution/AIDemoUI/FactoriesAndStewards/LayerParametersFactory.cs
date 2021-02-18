using Autofac;
using NeuralNetBuilder.FactoriesAndParameters;

namespace AIDemoUI.FactoriesAndStewards
{
    public interface ILayerParametersFactory
    {
        ILayerParameters CreateLayerParameters();
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

        public ILayerParameters CreateLayerParameters()
        {
            // Consider scope!
            return _context.Resolve<ILayerParameters>();
        }

        #endregion
    }
}
