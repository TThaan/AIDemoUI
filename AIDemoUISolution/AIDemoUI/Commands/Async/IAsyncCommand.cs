using System.Threading.Tasks;
using System.Windows.Input;

namespace AIDemoUI.Commands.Async
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
        bool IsConcurrentExecutionAllowed { get; set; }
    }
}
