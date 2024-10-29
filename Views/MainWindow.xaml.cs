using PlateDropletApp.ViewModels;
using System.Windows;

namespace PlateDropletApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
