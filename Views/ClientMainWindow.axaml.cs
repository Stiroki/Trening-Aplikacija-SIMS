using Avalonia.Controls;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class ClientMainWindow : Window
{
    private readonly ClientMainViewModel _viewModel;
    
    public ClientMainWindow() : this(new Client())
    {
    }

    public ClientMainWindow(Client client)
    {
        InitializeComponent();

        _viewModel = new ClientMainViewModel(client);
        _viewModel.NavigateToProfile += OnNavigateToProfile;
        _viewModel.NavigateToTrainers += OnNavigateToTrainers;
        _viewModel.NavigateToMyTrainings += OnNavigateToMyTrainings;
        _viewModel.LogoutRequested += OnLogoutRequested;

        DataContext = _viewModel;
    }

    private void OnNavigateToProfile()
    {
        ClientProfileWindow profileWindow = new ClientProfileWindow(_viewModel.Client);
        profileWindow.Show();
    }

    private void OnNavigateToTrainers()
    {
        TrainersListWindow trainersWindow = new TrainersListWindow(_viewModel.Client);
        trainersWindow.Show();
    }

    private void OnNavigateToMyTrainings()
    {
        MyTrainingsWindow trainingsWindow = new MyTrainingsWindow(_viewModel.Client);
        trainingsWindow.Show();
    }

    private void OnLogoutRequested()
    {
        LoginWindow loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
}