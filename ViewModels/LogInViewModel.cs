using System;
using TreningAplikacija.Services;
using TreningAplikacija.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace TreningAplikacija.ViewModels;

public partial class LogInViewModel : ViewModelBase
{
    private readonly AuthService _authService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isErrorVisible;

    // View se prijavljuje na ove evente da bi otvorio novi prozor / zatvorio ovaj.
    // ViewModel namerno ne zna ništa o prozorima (Window) - to je posao View-a.
    public event Action<User>? LoginSucceeded;
    public event Action? RegisterRequested;

    public LogInViewModel() : this(new AuthService())
    {
    }

    public LogInViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private void Login()
    {
        IsErrorVisible = false;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ShowError("Unesite email i lozinku.");
            return;
        }

        try
        {
            var user = _authService.Login(Email.Trim(), Password);

            if (user != null)
            {
                LoginSucceeded?.Invoke(user);
            }
            else
            {
                ShowError("Pogrešan email ili lozinka!");
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    [RelayCommand]
    private void Register()
    {
        RegisterRequested?.Invoke();
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
        IsErrorVisible = true;
    }
}