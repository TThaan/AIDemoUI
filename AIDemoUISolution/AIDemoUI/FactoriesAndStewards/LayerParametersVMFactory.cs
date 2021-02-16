using AIDemoUI.ViewModels;
using Autofac;

namespace AIDemoUI.FactoriesAndStewards
{
    public interface ILayerParametersVMFactory
    {
        LayerParametersVM CreateLayerParametersVM(int index);
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

        public LayerParametersVM CreateLayerParametersVM(int index)
        {
            // Consider scope!
            var session = _context.Resolve<ISessionContext>();
            return _context.Resolve<LayerParametersVM>(
                new TypedParameter(typeof(ISessionContext), session),
                new TypedParameter(typeof(SimpleMediator), _context.Resolve<SimpleMediator>()),
                new TypedParameter(typeof(int), index));
        }

        #endregion
    }
}
