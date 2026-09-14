using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public partial class SetPreferencesViewModel : ViewModelBase
{
    private readonly ClientService _clientService;
    private readonly TrainerRequest _request;
    
    public string TrainerName { get; }

    public string[] LocationOptions { get; } = { "Teretana", "Kuća", "Napolje" };

    [ObservableProperty]
    private int _locationIndex = -1;
    
    [ObservableProperty]
    private int _trainingsPerWeek = 3;

    [ObservableProperty] 
    private string _ownedEquipment = "";

    [ObservableProperty] 
    private string _statusMessage = "";

    [ObservableProperty] 
    private bool _isStatusVisible;

    public event Action? Saved;
    public event Action? BackRequested;
    
    public SetPreferencesViewModel(TrainerRequest request, string trainerName)
        : this(request, trainerName, new ClientService())
    {
    }

    public SetPreferencesViewModel(TrainerRequest request, string trainerName, ClientService clientService)
    {
        _request = request;
        _clientService = clientService;
        TrainerName = trainerName;

        if (request.PreferencesSet)
        {
            LocationIndex = Array.IndexOf(LocationOptions, request.LocationPreference);
            TrainingsPerWeek = request.TrainingsPerWeek;
            OwnedEquipment = request.OwnedEquipment;
        }
        else
        {
            Client client = clientService.GetClient(request.ClientId);
            if (client != null)
            {
                LocationIndex = Array.IndexOf(LocationOptions, client.LocationPreference);
                TrainingsPerWeek = client.TrainingsPerWeek;
            }
        }
    }

    [RelayCommand]
    private void Save()
    {
        if (LocationIndex < 0)
        {
            StatusMessage = "Izaberite lokaciju.";
            IsStatusVisible = true;
            return;
        }

        if (TrainingsPerWeek < 1 || TrainingsPerWeek > 7)
        {
            StatusMessage = "Broj treninga mora biti izmedju 1 i 7";
            IsStatusVisible = true;
            return;
        }

        try
        {
            _request.LocationPreference = LocationOptions[LocationIndex];
            _request.TrainingsPerWeek = TrainingsPerWeek;
            _request.OwnedEquipment = OwnedEquipment;
            _request.PreferencesSet = true;

            _clientService.UpdateRequest(_request);

            Saved?.Invoke();
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