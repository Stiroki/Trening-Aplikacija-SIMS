using Avalonia.Controls;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views;

public partial class TrainersListWindow : Window
{
    private readonly Client _client;
    public TrainersListWindow() : this(new Client())
    {
    }
    public TrainersListWindow(Client client)
    {
        InitializeComponent();
        TrainersListViewModel viewModel = new TrainersListViewModel(client);
        viewModel.BackRequested += OnBackRequested;
        viewModel.ReviewRequested += OnReviewRequested;
        viewModel.SetPreferencesRequested += OnSetPreferencesRequested;
        _client = client;

        DataContext = viewModel;
    }

    private void OnReviewRequested(TrainerListItem item)
    {
        ReviewTrainerWindow reviewWindow = new ReviewTrainerWindow(_client.Id, item.Trainer);
        reviewWindow.Closed += (_, _) =>
        {
            var viewModel = (TrainersListViewModel)DataContext!;
            viewModel.RefreshTrainers();
        };
        reviewWindow.Show();
    }

    private void OnSetPreferencesRequested(TrainerListItem item)
    {
        var trainerName = $"{item.Trainer.Name} {item.Trainer.LastName}";
        var preferencesWindow = new SetPreferencesWindow(item.Request!, trainerName);
        preferencesWindow.Closed += (_, _) =>
        {
            var viewModel = (TrainersListViewModel)DataContext!;
            viewModel.RefreshTrainers();
        };
        preferencesWindow.Show();
    }
    
    private void OnBackRequested()
    {
        Close();
    }
}