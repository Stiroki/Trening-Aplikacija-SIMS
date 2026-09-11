using System;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels
{
    public class RegisterTrainerViewModel : ViewModelBase
    {
        private readonly AuthService _authService;

        public event Action? RegistrationSucceeded;
        public event Action? BackRequested;

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _gender = string.Empty;
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private double _monthlyFee;
        public double MonthlyFee
        {
            get => _monthlyFee;
            set => SetProperty(ref _monthlyFee, value);
        }

        private double _feePerSession;
        public double FeePerSession
        {
            get => _feePerSession;
            set => SetProperty(ref _feePerSession, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public RegisterTrainerViewModel()
        {
            _authService = new AuthService();
        }

        public void Register()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Sva osnovna polja moraju biti popunjena.";
                return;
            }

            try
            {
                var trainer = new Trainer
                {
                    Name = Name.Trim(),
                    LastName = LastName.Trim(),
                    Email = Email.Trim(),
                    Password = Password,
                    Gender = Gender,
                    MonthlyFee = MonthlyFee,
                    FeePerSession = FeePerSession,
                    IsVerifiedByAdmin = false
                };

                _authService.RegisterTrainer(trainer);
                RegistrationSucceeded?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void GoBack()
        {
            BackRequested?.Invoke();
        }
    }
}