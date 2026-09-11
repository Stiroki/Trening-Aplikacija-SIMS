using Avalonia.Controls;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class TrainersListWindow : Window
{
    public TrainersListWindow() : this(new Client())
    {
    }
    public TrainersListWindow(Client client)
    {
        InitializeComponent();
        TrainersListViewModel viewModel = new TrainersListViewModel(client);
        viewModel.BackRequested += OnBackRequested;

        DataContext = viewModel;
    }

    private void OnBackRequested()
    {
        Close();
    }
}