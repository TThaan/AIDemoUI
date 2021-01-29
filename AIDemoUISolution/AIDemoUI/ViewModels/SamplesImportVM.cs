using AIDemoUI.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIDemoUI.ViewModels
{
    public class SamplesImportVM : BaseSubVM
    {
        #region fields & ctor
        
        IRelayCommand importSamplesCommand;

        public SamplesImportVM(MainWindowVM mainVM)
            : base(mainVM)
        {
            SetDefaultValues();
        }

        #region helpers
        
        void SetDefaultValues()
        {
            
        }

        #endregion

        #endregion

        #region public 
        
        #endregion

        #region Relay Command

        public IRelayCommand ImportSamplesCommand
        {
            get
            {
                if (importSamplesCommand == null)
                {
                    importSamplesCommand = new RelayCommand(ImportSamplesCommand_Execute, ImportSamplesCommand_CanExecute);
                }
                return importSamplesCommand;
            }
        }
        void ImportSamplesCommand_Execute(object parameter)
        {
            _mainVM.SampleImportWindow.Show();
        }
        bool ImportSamplesCommand_CanExecute(object parameter)
        {
            return true;
        }

        #endregion
    }
}
