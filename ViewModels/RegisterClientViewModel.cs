using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels;

public partial class RegisterClientViewModel : ViewModelBase
{
    private readonly AuthService _authService;

    [ObservableProperty] 
    private string _name = string.Empty;
    
    [ObservableProperty] 
    private string _lastName = string.Empty;
    
    [ObservableProperty] 
    private string _email = string.Empty;
    
    [ObservableProperty] 
    private string _password = string.Empty;
    
    [ObservableProperty] 
    private string _confirmPassword = string.Empty;
    
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
    private string _errorMessage = string.Empty;
    
    [ObservableProperty] 
    private bool _isErrorVisible;
    
    public string[] GenderOptions { get; } = { "Muški", "Ženski" };
    public string[] LocationOptions { get; } = { "Teretana", "Kuća", "Napolje" };

    public event Action? RegistrationSucceeded;
    public event Action? BackToLoginRequested;

    public RegisterClientViewModel() : this(new AuthService())
    {
    }

    public RegisterClientViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private void Register()
    {
        IsErrorVisible = false;

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(LastName))
        {
            ShowError("Unesite ime i/ili prezime.");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ShowError("Unesite email i/ili lozinku.");
            return;
        }

        if (Password != ConfirmPassword)
        {
            ShowError("Lozinke se ne poklapaju.");
            return;
        }

        try
        {
            Client client = new Client
            {
                Name = Name.Trim(),
                LastName = LastName.Trim(),
                Email = Email.Trim(),
                Password = Password,
                DateOfBirth = DateOfBirth?.DateTime,
                Gender = Gender,
                Height = Height,
                Weight = Weight,
                HealthIssues = HealthIssues,
                Goals = Goals,
                LocationPreference = LocationPreference,
                TrainingsPerWeek = TrainingsPerWeek
            };
            _authService.RegisterClient(client);
            RegistrationSucceeded?.Invoke();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    [RelayCommand]
    private void BackToLogin()
    {
        BackToLoginRequested?.Invoke();
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        IsErrorVisible = true;
    }

}