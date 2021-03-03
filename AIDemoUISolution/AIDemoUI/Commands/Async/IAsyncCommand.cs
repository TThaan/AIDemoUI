using System.Threading.Tasks;
using System.Windows.Input;

namespace AIDemoUI.Commands.Async
{
    public interface IAsyncRelayCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
        bool IsConcurrentExecutionAllowed { get; set; }
    }
}
