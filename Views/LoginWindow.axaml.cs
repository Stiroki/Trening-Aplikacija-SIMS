using Avalonia.Controls;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            var viewModel = new LogInViewModel();
            viewModel.LoginSucceeded += OnLoginSucceeded;
            viewModel.RegisterRequested += OnRegisterRequested;

            DataContext = viewModel;
        }

        private void OnLoginSucceeded(User user)
        {
            if (user is Client client)
            {
                ClientMainWindow clientWindow = new ClientMainWindow(client);
                clientWindow.Show();
            }
            else if (user is Trainer)
            {
                //var trainerWindow = new TrainerWindow();
                //trainerWindow.Show();
            }

            Close();
        }

        private void OnRegisterRequested()
        {
            RegisterChoiceWindow registerWindow = new RegisterChoiceWindow();
            registerWindow.Show();
            Close();
        }
    }
}