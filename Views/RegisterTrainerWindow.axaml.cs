using Avalonia.Controls;
using Avalonia.Interactivity;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views
{
    public partial class RegisterTrainerWindow : Window
    {
        private RegisterTrainerViewModel ViewModel => (RegisterTrainerViewModel)DataContext!;

        public RegisterTrainerWindow()
        {
            InitializeComponent();

            var vm = new RegisterTrainerViewModel();
            vm.RegistrationSucceeded += OnRegistrationSucceeded;
            vm.BackRequested += OnBackRequested;

            DataContext = vm;
        }

        private void OnRegisterClicked(object? sender, RoutedEventArgs e)
        {
            ViewModel.Register();
        }

        private void OnBackClicked(object? sender, RoutedEventArgs e)
        {
            ViewModel.GoBack();
        }

        private void OnRegistrationSucceeded()
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }

        private void OnBackRequested()
        {
            var choiceWindow = new RegisterChoiceWindow();
            choiceWindow.Show();
            Close();
        }
    }
}