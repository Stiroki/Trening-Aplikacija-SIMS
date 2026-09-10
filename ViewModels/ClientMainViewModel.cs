using System;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
namespace TreningAplikacija.ViewModels;

public partial class ClientMainViewModel : ViewModelBase
{
    private readonly Client _client;

    public string WelcomeMessage => $"Dobrodošli, {_client.Name} {_client.LastName}!";
    public Client Client => _client;

    public event Action? NavigateToProfile;
    public event Action? NavigateToTrainers;
    public event Action? NavigateToMyTrainings;
    public event Action? LogoutRequested;

    public ClientMainViewModel(Client client)
    {
        _client = client;
    }
    
    [RelayCommand]
    private void OpenProfile() => NavigateToProfile.Invoke();
    
    [RelayCommand]
    private void OpenTrainers() => NavigateToTrainers.Invoke();
    
    [RelayCommand]
    private void OpenMyTrainings() => NavigateToMyTrainings.Invoke();
    
    [RelayCommand]
    private void Logout() => LogoutRequested.Invoke();
}