using System.Threading.Tasks;
using System.Windows.Input;

namespace AIDemoUI.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
