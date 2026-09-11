using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public partial class TrainerListItem : ObservableObject
{
    public Trainer Trainer { get; }

    [ObservableProperty] 
    private string _requestStatusText = string.Empty;

    [ObservableProperty] 
    private bool _canSendRequest = true;

    public TrainerListItem(Trainer trainer)
    {
        Trainer = trainer;
    }
}
public partial class TrainersListViewModel : ViewModelBase
{
    private readonly ClientService _clientService;
    private readonly Client _client;

    [ObservableProperty] 
    private string _statusMessage = string.Empty;

    [ObservableProperty] 
    private bool _isStatusVisible;

    public ObservableCollection<TrainerListItem> Trainers { get; } = new();
    public event Action? BackRequested;
    
    public TrainersListViewModel(Client client) : this(client, new ClientService())
    {
    }

    public TrainersListViewModel(Client client, ClientService clientService)
    {
        _client = client;
        _clientService = clientService;

        LoadTrainers();
    }

    private void LoadTrainers()
    {
        Trainers.Clear();

        List<Trainer> trainers = _clientService.GetVerifiedTrainers();
        List<TrainerRequest> myRequests = _clientService.GetMyRequests(_client.Id);

        foreach (Trainer trainer in trainers)
        {
            TrainerListItem item = new TrainerListItem(trainer);

            TrainerRequest existingRequest = myRequests.FirstOrDefault(r => r.TrainerId == trainer.Id);
            if (existingRequest != null)
            {
                item.RequestStatusText = existingRequest.Status switch
                {
                    RequestStatus.Pending => "Zahtev poslat - čeka se odgovor",
                    RequestStatus.Accepted => "Zahtev prihvaćen",
                    RequestStatus.Rejected => "Zahtev odbijen - možete ponovo poslati",
                    _ => string.Empty
                };
                item.CanSendRequest = existingRequest.Status != RequestStatus.Pending &&
                                      existingRequest.Status != RequestStatus.Rejected;
            }
            Trainers.Add(item);
        }
    }

    [RelayCommand]
    private void SendRequest(TrainerListItem item)
    {
        if (item == null) return;
        try
        {
            _clientService.SendRequest(_client.Id, item.Trainer.Id, string.Empty);

            item.RequestStatusText = "Zahtev poslat - čeka se odgovor";
            item.CanSendRequest = false;

            ShowStatus($"Zahtev poslat: trener - {item.Trainer.Name} {item.Trainer.LastName}");
        }
        catch (Exception ex)
        {
            ShowStatus(ex.Message);
        }
    }

    [RelayCommand]
    private void Back()
    {
        BackRequested?.Invoke();
    }

    private void ShowStatus(string message)
    {
        StatusMessage = message;
        IsStatusVisible = true;
    }
}