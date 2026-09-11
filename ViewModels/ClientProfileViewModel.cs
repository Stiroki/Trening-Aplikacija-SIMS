using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Services;
using TreningAplikacija.Models;

namespace TreningAplikacija.ViewModels;

public partial class ClientProfileViewModel : ViewModelBase
{
    private readonly ClientService _clientService;
    private readonly Client _client;

    [ObservableProperty] 
    private string _name = string.Empty;
    
    [ObservableProperty] 
    private string _lastName = string.Empty;
    
    [ObservableProperty] 
    private string _email = string.Empty;
    
    [ObservableProperty] 
    private DateTimeOffset? _dateOfBirth;
    
    [ObservableProperty] 
    private string _gender = string.Empty;
    
    [ObservableProperty] 
    private double _height;
    
    [ObservableProperty] 
    private double _weight;
    
    [ObservableProperty] 
    private string _healthIssues = string.Empty;
    
    [ObservableProperty] 
    private string _goals = string.Empty;
    
    [ObservableProperty] 
    private string _locationPreference = string.Empty;
    
    [ObservableProperty] 
    private int _trainingsPerWeek;
    
    [ObservableProperty] 
    private string _statusMessage = string.Empty;
    
    [ObservableProperty] 
    private bool _isStatusVisible;
    
    public string[] GenderOptions { get; } = { "Muški", "Ženski" };
    public string[] LocationOptions { get; } = { "Teretana", "Kuća", "Napolje" };

    public event Action? BackRequested;

    public ClientProfileViewModel(Client client) : this(client, new ClientService())
    {
        
    }

    public ClientProfileViewModel(Client client, ClientService clientService)
    {
        _client = client;
        _clientService = clientService;
    }

    public void LoadClientData()
    {
        Name = _client.Name;
        LastName = _client.LastName;
        Email = _client.Email;
        Gender = _client.Gender;
        DateOfBirth = _client.DateOfBirth.HasValue ? new DateTimeOffset(_client.DateOfBirth.Value) : null;
        Height = _client.Height;
        Weight = _client.Weight;
        HealthIssues = _client.HealthIssues;
        Goals = _client.Goals;
        LocationPreference = _client.LocationPreference;
        TrainingsPerWeek = _client.TrainingsPerWeek;
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(LastName))
        {
            ShowStatus("Ime i/ili prezime su obavezni", true);
            return;
        }
        
        if (string.IsNullOrWhiteSpace(Email))
        {
            ShowStatus("Email je obavezan", true);
            return;
        }
        
        _client.Name = Name.Trim();
        _client.LastName = LastName.Trim();
        _client.Email = Email.Trim();
        _client.Gender = Gender;
        _client.DateOfBirth = DateOfBirth?.DateTime;
        _client.Height = Height;
        _client.Weight = Weight;
        _client.HealthIssues = HealthIssues;
        _client.Goals = Goals;
        _client.LocationPreference = LocationPreference;
        _client.TrainingsPerWeek = TrainingsPerWeek;

        try
        {
            _clientService.UpdateProfile(_client);
            ShowStatus("Podaci uspešno sačuvani!", false);
        }
        catch (Exception ex)
        {
            ShowStatus(ex.Message, true);
        }
    }

    [RelayCommand]
    private void Back()
    {
        BackRequested?.Invoke();
    }

    private void ShowStatus(string message, bool isError)
    {
        StatusMessage = message;
        IsStatusVisible = true;
    }
}