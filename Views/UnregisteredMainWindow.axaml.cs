using Avalonia.Controls;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views
{
    public partial class UnregisteredMainWindow : Window
    {
        public UnregisteredMainWindow()
        {
            InitializeComponent();
            
            var viewModel = new UnregisteredMainViewModel();
            viewModel.BackToLoginRequested += OnBackToLoginRequested;
            
            DataContext = viewModel;
        }

        private void OnBackToLoginRequested()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}

