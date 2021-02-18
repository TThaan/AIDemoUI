using Autofac;
using NeuralNetBuilder.FactoriesAndParameters;
using System.Collections.ObjectModel;

namespace AIDemoUI.FactoriesAndStewards
{
    public interface ILayerParametersCollectionFactory
    {
        ObservableCollection<ILayerParameters> CreateLayerParametersCollection();
    }
    /// <summary>
    /// Redundant if no CollectionChanged needed?
    /// </summary>
    public class LayerParametersCollectionFactory : ILayerParametersCollectionFactory
    {
            #region fields & ctor

            private readonly IComponentContext _context;

            public LayerParametersCollectionFactory(IComponentContext context)
            {
                _context = context;
            }

            #endregion

            #region ILayerParametersVMCollectionFactory

            public ObservableCollection<ILayerParameters> CreateLayerParametersCollection()
            {
                // Consider scope!
                return _context.Resolve<ObservableCollection<ILayerParameters>>();
            }

            #endregion
        }
    }
