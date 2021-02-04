using System.Windows.Input;

namespace AIDemoUI.Commands
{
    public interface IRelayCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
