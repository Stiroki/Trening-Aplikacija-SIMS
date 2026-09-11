using System;
using System.Collections.ObjectModel;
using System.Linq;
using TreningAplikacija.Models;
using TreningAplikacija.Services;

namespace TreningAplikacija.ViewModels
{
    public class TrainerViewModel : ViewModelBase
    {
        private DateTime? _selectedCalendarDate = DateTime.Today;
        private readonly TrainerService _trainerService;
        public Trainer CurrentTrainer { get; private set; }
        
        public ObservableCollection<TrainerRequest> PendingRequests { get; set; } = new();
        public ObservableCollection<Client> MyClients { get; set; } = new();
        public ObservableCollection<Exercise> Exercises { get; set; } = new();
        public ObservableCollection<Equipment> EquipmentList { get; set; } = new();
        public ObservableCollection<Review> Reviews { get; set; } = new();
        public ObservableCollection<ProgressEntry> SelectedClientProgress { get; set; } = new();
        public ObservableCollection<ClientInternalRating> ClientInternalRatingsHistory { get; set; } = new();
        public ObservableCollection<TrainingSession> SelectedClientSessions { get; set; } = new();
        
        // Selekcije
        private TrainerRequest? _selectedRequest;
        public TrainerRequest? SelectedRequest
        {
            get => _selectedRequest;
            set => SetProperty(ref _selectedRequest, value);
        }

        private Client? _selectedClient;
        public Client? SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (SetProperty(ref _selectedClient, value) && value != null)
                {
                    LoadClientProgress(value.Id);
                    LoadClientSessions(value.Id);
                    LoadClientProgress(value.Id);
                }
            }
        }

        private Exercise? _selectedExercise;
        public Exercise? SelectedExercise
        {
            get => _selectedExercise;
            set => SetProperty(ref _selectedExercise, value);
        }

        private Equipment? _selectedEquipment;
        public Equipment? SelectedEquipment
        {
            get => _selectedEquipment;
            set => SetProperty(ref _selectedEquipment, value);
        }

        public TrainerViewModel(Trainer trainer)
        {
            CurrentTrainer = trainer;
            _trainerService = new TrainerService();
            LoadCalendarSessions(DateTime.Today);
            RefreshAll();
        }

        public void RefreshAll()
        {
            LoadRequests();
            LoadClients();
            LoadExercises();
            LoadEquipment();
            LoadReviews();
        }

        public void LoadRequests()
        {
            PendingRequests.Clear();
            foreach (var req in _trainerService.GetPendingRequests(CurrentTrainer.Id))
                PendingRequests.Add(req);
        }

        public void LoadClients()
        {
            MyClients.Clear();
            foreach (var cl in _trainerService.GetMyClients(CurrentTrainer.Id))
                MyClients.Add(cl);
        }

        public void LoadExercises()
        {
            Exercises.Clear();
            foreach (var ex in _trainerService.GetTrainerExercises(CurrentTrainer.Id))
                Exercises.Add(ex);
        }

        public void LoadEquipment()
        {
            EquipmentList.Clear();
            foreach (var eq in _trainerService.GetAllEquipment())
                EquipmentList.Add(eq);
        }

        public void LoadReviews()
        {
            Reviews.Clear();
            foreach (var rev in _trainerService.GetMyReviews(CurrentTrainer.Id))
                Reviews.Add(rev);
        }
        public void LoadClientSessions(Guid clientId)
        {
            SelectedClientSessions.Clear();
            foreach (var session in _trainerService.GetClientSessionsForTrainer(CurrentTrainer.Id, clientId))
            {
                SelectedClientSessions.Add(session);
            }
        }
        public void LoadClientProgress(Guid clientId)
        {
            SelectedClientProgress.Clear();
            foreach (var prog in _trainerService.GetClientProgress(clientId))
                SelectedClientProgress.Add(prog);
        }

        public void AcceptRequest(Guid requestId, string note)
        {
            _trainerService.AcceptRequest(requestId, note);
            RefreshAll();
        }

        public void RejectRequest(Guid requestId, string reason)
        {
            _trainerService.RejectRequest(requestId, reason);
            RefreshAll();
        }

        public void SaveEquipment(Equipment equipment, bool isNew)
        {
            if (isNew) _trainerService.AddEquipment(equipment);
            else _trainerService.UpdateEquipment(equipment);
            LoadEquipment();
        }

        public void DeleteEquipment(Guid eqId)
        {
            _trainerService.DeleteEquipment(eqId);
            LoadEquipment();
        }

        public void SaveExercise(Exercise exercise, bool isNew)
        {
            exercise.TrainerId = CurrentTrainer.Id;
            if (isNew) _trainerService.AddExercise(exercise);
            else _trainerService.UpdateExercise(exercise);
            LoadExercises();
        }

        public void DeleteExercise(Guid exId)
        {
            _trainerService.DeleteExercise(exId);
            LoadExercises();
        }

        public void SaveProfile()
        {
            _trainerService.UpdateProfile(CurrentTrainer);
        }

        public string GetClientName(Guid clientId)
        {
            var cl = _trainerService.GetClientById(clientId);
            return cl != null ? $"{cl.Name} {cl.LastName}" : "Klijent";
        }
        
        // Kalendar
        public DateTime? SelectedCalendarDate
        {
            get => _selectedCalendarDate;
            set
            {
                if (SetProperty(ref _selectedCalendarDate, value) && value.HasValue)
                {
                    LoadCalendarSessions(value.Value);
                }
            }
        }

        public ObservableCollection<TrainingSession> CalendarSessions { get; set; } = new();

        // Interna ocena za izabranog klijenta
        private int _currentClientInternalRating = 5;
        public int CurrentClientInternalRating
        {
            get => _currentClientInternalRating;
            set => SetProperty(ref _currentClientInternalRating, value);
        }

        private string _currentClientInternalNote = string.Empty;
        public string CurrentClientInternalNote
        {
            get => _currentClientInternalNote;
            set => SetProperty(ref _currentClientInternalNote, value);
        }

        public void LoadCalendarSessions(DateTime date)
        {
            CalendarSessions.Clear();
            foreach (var s in _trainerService.GetTrainerSessionsByDate(CurrentTrainer.Id, date))
            {
                CalendarSessions.Add(s);
            }
        }

        public void DeleteSession(Guid sessionId)
        {
            _trainerService.DeleteTrainingSession(sessionId);
            if (SelectedClient != null)
            {
                LoadClientSessions(SelectedClient.Id);
            }
            if (SelectedCalendarDate.HasValue)
            {
                LoadCalendarSessions(SelectedCalendarDate.Value);
            }
        }

        public void SaveClientInternalRating()
        {
            if (SelectedClient == null) return;
            _trainerService.RateClientInternally(CurrentTrainer.Id, SelectedClient.Id, CurrentClientInternalRating, CurrentClientInternalNote);
            LoadClientInternalRatings(SelectedClient.Id);
        }

        public void LoadClientInternalRatings(Guid clientId)
        {
            ClientInternalRatingsHistory.Clear();
            var myRating = _trainerService.GetClientInternalRating(CurrentTrainer.Id, clientId);
            if (myRating != null)
            {
                CurrentClientInternalRating = myRating.Rating;
                CurrentClientInternalNote = myRating.Note;
            }
            else
            {
                CurrentClientInternalRating = 5;
                CurrentClientInternalNote = string.Empty;
            }

            foreach (var r in _trainerService.GetAllClientInternalRatings(clientId))
            {
                ClientInternalRatingsHistory.Add(r);
            }
        }
    }
}