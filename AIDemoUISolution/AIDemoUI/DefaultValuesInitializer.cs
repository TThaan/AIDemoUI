using AIDemoUI.FactoriesAndStewards;
using AIDemoUI.ViewModels;
using Autofac;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AIDemoUI
{
    /// <summary>
    /// Each default property here is a single instance, i.e.
    /// DefaultLayerParameter01 is the same instance as DefaultNetParameters.DefaultLayerParametersCollection[0] 
    /// and also the same instance as DefaultNetParametersVM.DefaultLayerParametersVMCollection[0].
    /// 
    /// So far only DefaultMainWindowVM is used as public prop..
    /// </summary>
    public class DefaultValuesInitializer
    {
        #region fields & ctor

        private readonly IComponentContext _context;
        private IMainWindowVM defaultMainWindowVM;  // redundant?
        private ITrainerParameters defaultTrainerParameters;
        private ILayerParameters defaultLayerParameters01, defaultLayerParameters02, defaultLayerParameters03;
        private INetParameters defaultNetParameters;
        //private ILayerParametersVM defaultLayerParametersVM01, defaultLayerParametersVM02, defaultLayerParametersVM03;
        private INetParametersVM defaultNetParametersVM;
        private IStartStopVM defaultStartStopVM;
        private IStatusVM defaultStatusVM;

        public DefaultValuesInitializer(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        //internal void SetDefaultValues(BaseVM viewModel)
        //{
        //    Type type = viewModel.GetType();

        //    if (type == typeof(INetParametersVM))
        //    {
        //        ConfigINetParametersVM(viewModel as INetParametersVM);
        //    }
        //    else if (true)
        //    {

        //    }
        //}

        //private void ConfigINetParametersVM(INetParametersVM viewModel)
        //{
        //    viewModel.
        //}




        #region public

        public IMainWindowVM DefaultMainWindowVM => defaultMainWindowVM ?? (defaultMainWindowVM = GetDefaultMainWindowVM());
        public IStartStopVM DefaultStartStopVM => defaultStartStopVM ?? (defaultStartStopVM = GetDefaultStartStopVM());
        public IStatusVM DefaultStatusVM => defaultStatusVM ?? (defaultStatusVM = GetDefaultStatusVM());
        public INetParametersVM DefaultNetParametersVM => defaultNetParametersVM ?? (defaultNetParametersVM = GetDefaultNetParametersVM());
        //public ILayerParametersVM DefaultLayerParametersVM01 => defaultLayerParametersVM01 ?? (defaultLayerParametersVM01 = GetDefaultLayerParametersVM01());
        //public ILayerParametersVM DefaultLayerParametersVM02 => defaultLayerParametersVM02 ?? (defaultLayerParametersVM02 = GetDefaultLayerParametersVM02());
        //public ILayerParametersVM DefaultLayerParametersVM03 => defaultLayerParametersVM03 ?? (defaultLayerParametersVM03 = GetDefaultLayerParametersVM03());
        public INetParameters DefaultNetParameters => defaultNetParameters ?? (defaultNetParameters = GetDefaultNetParameters());
        public ILayerParameters DefaultLayerParameters01  => defaultLayerParameters01 ?? (defaultLayerParameters01 = GetDefaultLayerParameters01());
        public ILayerParameters DefaultLayerParameters02 => defaultLayerParameters02 ?? (defaultLayerParameters02 = GetDefaultLayerParameters02());
        public ILayerParameters DefaultLayerParameters03 => defaultLayerParameters03 ?? (defaultLayerParameters03 = GetDefaultLayerParameters03());
        public ITrainerParameters DefaultTrainerParameters => defaultTrainerParameters ?? (defaultTrainerParameters = GetDefaultTrainerParameters());
        public string DefaultFileName => "";

        #endregion

        #region helpers

        private IMainWindowVM GetDefaultMainWindowVM()
        {
            var result = _context.Resolve<IMainWindowVM>();

            result.NetParametersVM = DefaultNetParametersVM;
            result.StartStopVM = DefaultStartStopVM;
            result.StatusVM = DefaultStatusVM;

            return result;
        }
        private IStartStopVM GetDefaultStartStopVM()
        {
            var result = _context.Resolve<IStartStopVM>();

            result.IsStarted = false;
            result.IsLogged = false;
            result.LogName = Path.GetTempPath() + "AIDemoUI.txt";

            return result;
        }
        private IStatusVM GetDefaultStatusVM()
        {
            var result = _context.Resolve<IStatusVM>();

            result.ProgressBarValue = 0;
            result.ProgressBarMax = 100;
            result.ProgressBarText = "Wpf AI Demo";

            return result;
        }
        private ISessionContext GetDefaultSessionContext()
        {
            var result = _context.Resolve<ISessionContext>();

            result.NetParameters = DefaultNetParameters;
            result.TrainerParameters = DefaultTrainerParameters;

            return result;
        }
        private INetParametersVM GetDefaultNetParametersVM()
        {
            var result = _context.Resolve<INetParametersVM>();

            

            result.AreParametersGlobal = true;
            result.BiasMin_Global = 0;
            result.BiasMax_Global = 0;
            result.WeightMin_Global = -1;
            result.WeightMax_Global = 1;
            
            //result.LayerParametersVMCollection = new ObservableCollection<ILayerParametersVM>
            //{
            //    DefaultLayerParametersVM01, DefaultLayerParametersVM02, DefaultLayerParametersVM03
            //};

            return result;
        }
        //private ILayerParametersVM GetDefaultLayerParametersVM01()
        //{
        //    return _context.Resolve<ILayerParametersVMFactory>().CreateLayerParametersVM(DefaultLayerParameters01);
        //}
        //private ILayerParametersVM GetDefaultLayerParametersVM02()
        //{
        //    return _context.Resolve<ILayerParametersVMFactory>().CreateLayerParametersVM(DefaultLayerParameters02);
        //}
        //private ILayerParametersVM GetDefaultLayerParametersVM03()
        //{
        //    return _context.Resolve<ILayerParametersVMFactory>().CreateLayerParametersVM(DefaultLayerParameters03);
        //}
        private INetParameters GetDefaultNetParameters()
        {
            var result = _context.Resolve<INetParameters>();

            // FileName = "DefaultFileName",
            result.LayerParametersCollection.Add(DefaultLayerParameters01);
            result.LayerParametersCollection.Add(DefaultLayerParameters02);
            result.LayerParametersCollection.Add(DefaultLayerParameters03);
            // WeightInitType = WeightInitType.Xavier

            return result;
        }
        private ILayerParameters GetDefaultLayerParameters01()
        {
            var result = _context.Resolve<ILayerParameters>();

            result.Id = 0;
            result.NeuronsPerLayer = 6;
            result.ActivationType = ActivationType.NullActivator;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -2;
            result.WeightMax = 2;

            return result;
        }
        private ILayerParameters GetDefaultLayerParameters02()
        {
            var result = _context.Resolve<ILayerParameters>();

            result.Id = 1;
            result.NeuronsPerLayer = 12;
            result.ActivationType = ActivationType.Tanh;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -2;
            result.WeightMax = 2;

            return result;
        }
        private ILayerParameters GetDefaultLayerParameters03()
        {
            var result = _context.Resolve<ILayerParameters>();

            result.Id = 2;
            result.NeuronsPerLayer = 6;
            result.ActivationType = ActivationType.LeakyReLU;
            result.BiasMin = 0;
            result.BiasMax = 0;
            result.WeightMin = -2;
            result.WeightMax = 2;

            return result;
        }
        private ITrainerParameters GetDefaultTrainerParameters()
        {
            var result = _context.Resolve<ITrainerParameters>();

            result.LearningRate = .1f;
            result.LearningRateChange = .9f;
            result.Epochs = 10;
            result.CostType = CostType.SquaredMeanError;

            return result;
        }

        #endregion
    }
}
