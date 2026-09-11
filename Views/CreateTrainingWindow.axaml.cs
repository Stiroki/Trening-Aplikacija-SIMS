using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.Views
{
    public class TrainingItemDisplay
    {
        public Guid ExerciseId { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string Duration { get; set; } = string.Empty;
    }

    public partial class CreateTrainingWindow : Window
    {
        private readonly Guid _trainerId;
        private readonly Guid _clientId;
        private readonly TrainingService _trainingService;
        private readonly TrainerService _trainerService;

        public ObservableCollection<TrainingItemDisplay> SessionItems { get; set; } = new();

        public CreateTrainingWindow()
        {
            InitializeComponent();
        }

        public CreateTrainingWindow(Guid trainerId, Guid clientId) : this()
        {
            _trainerId = trainerId;
            _clientId = clientId;
            _trainingService = new TrainingService();
            _trainerService = new TrainerService();

            ItemsListBox.ItemsSource = SessionItems;
            ExerciseComboBox.ItemsSource = _trainerService.GetTrainerExercises(_trainerId);
            ExerciseComboBox.DisplayMemberBinding = new Avalonia.Data.Binding("Name");
        }

        private void OnAddItemClicked(object? sender, RoutedEventArgs e)
        {
            if (ExerciseComboBox.SelectedItem is not Exercise selectedEx) return;

            int.TryParse(SetsInput.Text, out int sets);
            int.TryParse(RepsInput.Text, out int reps);

            SessionItems.Add(new TrainingItemDisplay
            {
                ExerciseId = selectedEx.Id,
                ExerciseName = selectedEx.Name,
                Sets = sets > 0 ? sets : 3,
                Reps = reps > 0 ? reps : 10,
                Duration = string.IsNullOrWhiteSpace(DurationInput.Text) ? "60s" : DurationInput.Text
            });
        }

        private void OnSaveSessionClicked(object? sender, RoutedEventArgs e)
        {
            if (SessionItems.Count == 0) return;

            var session = new TrainingSession
            {
                ClientId = _clientId,
                TrainerId = _trainerId,
                DateCreated = DateTime.Now,
                Items = SessionItems.Select(i => new TrainingItem
                {
                    ExerciseId = i.ExerciseId,
                    Sets = i.Sets,
                    Reps = i.Reps,
                    Duration = i.Duration
                }).ToList()
            };

            _trainingService.AssignTrainingSession(session);
            Close();
        }

        private void OnCancelClicked(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}