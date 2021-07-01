using AIDemoUI.Views;
using Autofac;
using System;

namespace AIDemoUI.FactoriesAndStewards
{
    public interface IDelegateFactory
    {
        //Action HideSampleImportWindow();
        Func<bool?> ShowSampleImportWindow();
    }

    public class DelegateFactory : IDelegateFactory
    {
        #region fields & ctor

        private readonly IComponentContext _context;

        public DelegateFactory(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        #region IDelegateFactory

        public Func<bool?> ShowSampleImportWindow()
        {
            return () => _context.Resolve<SampleImportWindow>().ShowDialog();
        }
        //public Action HideSampleImportWindow()
        //{
        //    return () => _context.Resolve<SampleImportWindow>().Hide();
        //}

        #endregion
    }
}
