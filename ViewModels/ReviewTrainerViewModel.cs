using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public partial class ReviewTrainerViewModel : ViewModelBase
{
    private readonly ClientService _clientService;
    private readonly Guid _clientId;
    private readonly Guid _trainerId;
    
    public string TrainerItem { get; }

    [ObservableProperty] 
    private decimal _rating = 5;

    [ObservableProperty] 
    private string _comment = string.Empty;

    [ObservableProperty] 
    private string _statusMessage = string.Empty;

    [ObservableProperty] 
    private bool _isStatusVisible;

    public event Action? ReviewSubmitted;
    public event Action? BackRequested;
    
    public ReviewTrainerViewModel(Guid clientId, Trainer trainer)
        : this(clientId, trainer, new ClientService())
    {
    }

    public ReviewTrainerViewModel(Guid clientId, Trainer trainer, ClientService clientService)
    {
        _clientId = clientId;
        _trainerId = trainer.Id;
        _clientService = clientService;
        string TrainerName = $"{trainer.Name} {trainer.LastName}";
    }

    [RelayCommand]
    private void Submit()
    {
        if (string.IsNullOrWhiteSpace(Comment))
        {
            StatusMessage = "Komentar je obavezan.";
            IsStatusVisible = true;
            return;
        }

        try
        {
            _clientService.ReviewTrainer(_clientId, _trainerId, (int)Rating, Comment);
            ReviewSubmitted?.Invoke();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            IsStatusVisible = true;
        }
    }

    [RelayCommand]
    private void Back()
    {
        BackRequested?.Invoke();
    }
}