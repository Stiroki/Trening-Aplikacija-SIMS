using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TreningAplikacija.Models;
using TreningAplikacija.Repositories;

namespace TreningAplikacija.ViewModels
{
    public partial class UnregisteredMainViewModel : ViewModelBase
    {
        private readonly JsonRepository<Trainer> _trainerRepo;
        private readonly JsonRepository<Review> _reviewRepo;
        private readonly JsonRepository<Exercise> _exerciseRepo;

        [ObservableProperty]
        private ObservableCollection<Trainer> _trainers;

        [ObservableProperty]
        private ObservableCollection<Review> _reviews;

        [ObservableProperty]
        private ObservableCollection<Exercise> _exercises;

        public event Action? BackToLoginRequested;

        public UnregisteredMainViewModel()
        {
            _trainerRepo = new JsonRepository<Trainer>("trainers.json");
            _reviewRepo = new JsonRepository<Review>("reviews.json");
            _exerciseRepo = new JsonRepository<Exercise>("exercises.json");

            _trainers = new ObservableCollection<Trainer>(_trainerRepo.GetAll());
            _reviews = new ObservableCollection<Review>(_reviewRepo.GetAll());
            _exercises = new ObservableCollection<Exercise>(_exerciseRepo.GetAll());
        }

        [RelayCommand]
        private void BackToLogin()
        {
            BackToLoginRequested?.Invoke();
        }
    }
}