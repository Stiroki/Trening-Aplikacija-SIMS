using Avalonia;
using Avalonia.Controls;
using TreningAplikacija.Models;
using Avalonia.Markup.Xaml;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class ClientProfileWindow : Window
{
    public ClientProfileWindow() : this(new Client())
    {
    }
    public ClientProfileWindow(Client client)
    {
        InitializeComponent();

        var viewModel = new ClientProfileViewModel(client);
        viewModel.BackRequested += OnBackRequested;

        DataContext = viewModel;
        viewModel.LoadClientData();
    }

    private void OnBackRequested()
    {
        Close();
    }
}