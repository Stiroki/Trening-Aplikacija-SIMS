using Avalonia.Controls;
using Avalonia.Interactivity;
using TreningAplikacija.Models;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Views
{
    public partial class TrainerMainWindow : Window
    {
        private TrainerViewModel ViewModel => (TrainerViewModel)DataContext!;

        public TrainerMainWindow()
        {
            InitializeComponent();
        }

        public TrainerMainWindow(Trainer trainer) : this()
        {
            DataContext = new TrainerViewModel(trainer);
        }

        private void OnAcceptRequestClicked(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedRequest == null) return;
            ViewModel.AcceptRequest(ViewModel.SelectedRequest.Id, RequestResponseNote.Text ?? "");
            RequestResponseNote.Text = string.Empty;
        }

        private void OnRejectRequestClicked(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedRequest == null) return;
            ViewModel.RejectRequest(ViewModel.SelectedRequest.Id, RequestResponseNote.Text ?? "");
            RequestResponseNote.Text = string.Empty;
        }

        private void OnAddEquipmentClicked(object? sender, RoutedEventArgs e)
        {
            var eq = new Equipment
            {
                Name = EqNameInput.Text ?? "",
                Type = EqTypeInput.Text ?? "",
                Description = EqDescInput.Text ?? ""
            };
            ViewModel.SaveEquipment(eq, true);
            ClearEquipmentInputs();
        }

        private void OnUpdateEquipmentClicked(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedEquipment == null) return;
            ViewModel.SelectedEquipment.Name = EqNameInput.Text ?? ViewModel.SelectedEquipment.Name;
            ViewModel.SelectedEquipment.Type = EqTypeInput.Text ?? ViewModel.SelectedEquipment.Type;
            ViewModel.SelectedEquipment.Description = EqDescInput.Text ?? ViewModel.SelectedEquipment.Description;
            ViewModel.SaveEquipment(ViewModel.SelectedEquipment, false);
            ClearEquipmentInputs();
        }

        private void OnDeleteEquipmentClicked(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedEquipment == null) return;
            ViewModel.DeleteEquipment(ViewModel.SelectedEquipment.Id);
            ClearEquipmentInputs();
        }

        private void ClearEquipmentInputs()
        {
            EqNameInput.Text = "";
            EqTypeInput.Text = "";
            EqDescInput.Text = "";
        }

        private void OnAddExerciseClicked(object? sender, RoutedEventArgs e)
        {
            var ex = new Exercise
            {
                Name = ExerciseNameInput.Text ?? "",
                Description = ExerciseDescInput.Text ?? "",
                VideoUrl = ExerciseUrlInput.Text ?? ""
            };
            ViewModel.SaveExercise(ex, true);
            ClearExerciseInputs();
        }

        private void OnUpdateExerciseClicked(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedExercise == null) return;
            ViewModel.SelectedExercise.Name = ExerciseNameInput.Text ?? ViewModel.SelectedExercise.Name;
            ViewModel.SelectedExercise.Description = ExerciseDescInput.Text ?? ViewModel.SelectedExercise.Description;
            ViewModel.SelectedExercise.VideoUrl = ExerciseUrlInput.Text ?? ViewModel.SelectedExercise.VideoUrl;
            ViewModel.SaveExercise(ViewModel.SelectedExercise, false);
            ClearExerciseInputs();
        }

        private void OnDeleteExerciseClicked(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedExercise == null) return;
            ViewModel.DeleteExercise(ViewModel.SelectedExercise.Id);
            ClearExerciseInputs();
        }

        private void ClearExerciseInputs()
        {
            ExerciseNameInput.Text = "";
            ExerciseDescInput.Text = "";
            ExerciseUrlInput.Text = "";
        }

        private void OnSaveProfileClicked(object? sender, RoutedEventArgs e)
        {
            ViewModel.SaveProfile();
        }

        private async void OnCreateTrainingSessionClicked(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedClient == null) return;

            var dialog = new CreateTrainingWindow(ViewModel.CurrentTrainer.Id, ViewModel.SelectedClient.Id);
            await dialog.ShowDialog(this);
        }
        
        private void OnLogoutClicked(object? sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
        private void OnDeleteSessionClicked(object? sender, RoutedEventArgs e)
        {
            if (ClientSessionsListBox.SelectedItem is TrainingSession session)
            {
                ViewModel.DeleteSession(session.Id);
            }
        }

        private void OnSaveInternalRatingClicked(object? sender, RoutedEventArgs e)
        {
            ViewModel.SaveClientInternalRating();
        }
    }
}