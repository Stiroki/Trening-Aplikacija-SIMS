using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService(); 
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text?.Trim() ?? string.Empty;
            string password = PasswordTextBox.Text ?? string.Empty;

            ErrorTextBlock.IsVisible = false;

            try
            {
                var loggedInUser = _authService.Login(email, password);

                if (loggedInUser != null)
                {
                    if (loggedInUser is Client)
                    {
                        //var clientWindow = new ClientWindow();
                        //clientWindow.Show();
                    }
                    else if (loggedInUser is Trainer)
                    {
                        //var trainerWindow = new TrainerWindow();
                        //trainerWindow.Show();
                    }
                    
                    this.Close(); 
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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            //var registerWindow = new RegisterWindow();
            //registerWindow.Show();
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.IsVisible = true;
        }
    }
}