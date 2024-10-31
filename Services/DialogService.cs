using System.Threading.Tasks;
using System.Windows;

namespace PlateDropletApp.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowMessageAsync(string title, string message)
        {
            // You can enhance this method to support asynchronous dialogs if needed
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            return Task.CompletedTask;
        }
    }
}
