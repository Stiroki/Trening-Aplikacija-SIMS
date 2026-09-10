using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class RegisterClientWindow : Window
{
    public RegisterClientWindow()
    {
        InitializeComponent();

        var viewModel = new RegisterClientViewModel();
        viewModel.RegistrationSucceeded += OnRegistrationSucceeded;
        viewModel.BackToLoginRequested += OnBackToLogin;

        DataContext = viewModel;
    }

    private void OnRegistrationSucceeded()
    {
        LoginWindow loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
    
    private void OnBackToLogin()
    {
        LoginWindow loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
}