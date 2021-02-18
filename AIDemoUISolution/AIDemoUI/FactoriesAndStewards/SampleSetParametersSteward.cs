using Autofac;
using DeepLearningDataProvider;

namespace AIDemoUI.FactoriesAndStewards
{
    public interface ISampleSetParametersSteward
    {
        ISampleSetParameters SampleSetParameters { get; set; }
        ISampleSetParameters CreateSampleSetParameters(SetName setName);
    }

    public class SampleSetParametersSteward : ISampleSetParametersSteward
    {
        #region fields & ctor
        
        private readonly IComponentContext _context;


        public SampleSetParametersSteward(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        #region public

        public ISampleSetParameters SampleSetParameters { get; set; }
        public ISampleSetParameters CreateSampleSetParameters(SetName setName)
        {
            // Consider scope!
            // Remove parameter!
            return SampleSetParameters = 
                _context.Resolve<SampleSetParameters>(new TypedParameter(typeof(SetName), setName));
        }

        #endregion
    }
}
