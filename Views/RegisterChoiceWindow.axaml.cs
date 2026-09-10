using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class RegisterChoiceWindow : Window
{
    public RegisterChoiceWindow()
    {
        InitializeComponent();

        var viewModel = new RegisterChoiceViewModel();
        viewModel.ClientChosen += OnClientChosen;
        viewModel.TrainerChosen += OnTrainerChosen;
        viewModel.BackRequested += OnBackRequested;

        DataContext = viewModel;
    }

    private void OnClientChosen()
    {
        RegisterClientWindow clientWindow = new RegisterClientWindow();
        clientWindow.Show();
        Close();
    }
    
    private void OnTrainerChosen()
    {
        //RegisterTrainerWindow trainerWindow = new RegisterTrainerWindow();
        //trainerWindow.Show();
        //Close();
    }

    private void OnBackRequested()
    {
        LoginWindow loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
}