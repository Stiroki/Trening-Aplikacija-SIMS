using System;
using System.Collections.Generic;
using System.Linq;
using TreningAplikacija.Models;
using TreningAplikacija.Repositories;

namespace TreningAplikacija.Services
{
    public class TrainerService
    {
        private readonly JsonRepository<Trainer> _trainerRepo;
        private readonly JsonRepository<Client> _clientRepo;
        private readonly JsonRepository<TrainerRequest> _requestRepo;
        private readonly JsonRepository<Exercise> _exerciseRepo;
        private readonly JsonRepository<Equipment> _equipmentRepo;
        private readonly JsonRepository<TrainingSession> _sessionRepo;
        private readonly JsonRepository<ProgressEntry> _progressRepo;
        private readonly JsonRepository<Review> _reviewRepo;
        private readonly NotificationService _notificationService;
        private readonly JsonRepository<ClientInternalRating> _internalRatingRepo = new("client_internal_ratings.json");
        
        public TrainerService()
        {
            _trainerRepo = new JsonRepository<Trainer>("trainers.json");
            _clientRepo = new JsonRepository<Client>("clients.json");
            _requestRepo = new JsonRepository<TrainerRequest>("trainer_requests.json");
            _exerciseRepo = new JsonRepository<Exercise>("exercises.json");
            _equipmentRepo = new JsonRepository<Equipment>("equipment.json");
            _sessionRepo = new JsonRepository<TrainingSession>("training_sessions.json");
            _progressRepo = new JsonRepository<ProgressEntry>("progress_entries.json");
            _reviewRepo = new JsonRepository<Review>("reviews.json");
            _notificationService = new NotificationService();
        }

        // Profil
        public Trainer? GetTrainer(Guid trainerId) => _trainerRepo.GetById(trainerId);

        public void UpdateProfile(Trainer trainer) => _trainerRepo.Update(trainer);

        // Zahtevi
        public List<TrainerRequest> GetPendingRequests(Guid trainerId)
        {
            return _requestRepo.GetAll()
                .Where(r => r.TrainerId == trainerId && r.Status == RequestStatus.Pending)
                .OrderByDescending(r => r.DateSent)
                .ToList();
        }

        public void AcceptRequest(Guid requestId, string responseMsg = "")
        {
            var req = _requestRepo.GetById(requestId);
            if (req == null) throw new Exception("Zahtev nije pronađen.");

            req.Status = RequestStatus.Accepted;
            req.ResponseMessage = responseMsg;
            _requestRepo.Update(req);

            var trainer = _trainerRepo.GetById(req.TrainerId);
            string trainerName = trainer != null ? $"{trainer.Name} {trainer.LastName}" : "Trener";

            _notificationService.CreateNotification(
                req.ClientId,
                NotificationType.RequestAccepted,
                $"{trainerName} je prihvatio vaš zahtev za saradnju.");
        }

        public void RejectRequest(Guid requestId, string reason = "")
        {
            var req = _requestRepo.GetById(requestId);
            if (req == null) throw new Exception("Zahtev nije pronađen.");

            req.Status = RequestStatus.Rejected;
            req.ResponseMessage = reason;
            _requestRepo.Update(req);

            var trainer = _trainerRepo.GetById(req.TrainerId);
            string trainerName = trainer != null ? $"{trainer.Name} {trainer.LastName}" : "Trener";

            _notificationService.CreateNotification(
                req.ClientId,
                NotificationType.RequestRejected,
                $"{trainerName} je odbio vaš zahtev. {(!string.IsNullOrWhiteSpace(reason) ? $"Razlog: {reason}" : "")}");
        }

        // Klijenti
        public List<Client> GetMyClients(Guid trainerId)
        {
            var acceptedClientIds = _requestRepo.GetAll()
                .Where(r => r.TrainerId == trainerId && r.Status == RequestStatus.Accepted)
                .Select(r => r.ClientId)
                .Distinct()
                .ToHashSet();

            return _clientRepo.GetAll().Where(c => acceptedClientIds.Contains(c.Id)).ToList();
        }

        public Client? GetClientById(Guid clientId) => _clientRepo.GetById(clientId);

        public List<ProgressEntry> GetClientProgress(Guid clientId)
        {
            return _progressRepo.GetAll()
                .Where(p => p.ClientId == clientId)
                .OrderByDescending(p => p.Date)
                .ToList();
        }

        // CRUD opreme
        public List<Equipment> GetAllEquipment() => _equipmentRepo.GetAll();

        public void AddEquipment(Equipment equipment) => _equipmentRepo.Create(equipment);

        public void UpdateEquipment(Equipment equipment) => _equipmentRepo.Update(equipment);

        public void DeleteEquipment(Guid equipmentId) => _equipmentRepo.Delete(equipmentId);

        // CRUD vezbi
        public List<Exercise> GetTrainerExercises(Guid trainerId)
        {
            return _exerciseRepo.GetAll().Where(e => e.TrainerId == trainerId).ToList();
        }

        public void AddExercise(Exercise exercise) => _exerciseRepo.Create(exercise);

        public void UpdateExercise(Exercise exercise) => _exerciseRepo.Update(exercise);

        public void DeleteExercise(Guid exerciseId) => _exerciseRepo.Delete(exerciseId);

        // kreiranje treninga
        public void CreateTrainingSession(TrainingSession session)
        {
            if (session.Items == null || !session.Items.Any())
                throw new Exception("Trening mora sadržati barem jednu vežbu.");

            _sessionRepo.Create(session);

            _notificationService.CreateNotification(
                session.ClientId,
                NotificationType.NewTraining,
                "Trener vam je dodelio novi plan treninga.");
        }

        public List<TrainingSession> GetAssignedSessions(Guid trainerId, Guid clientId)
        {
            return _sessionRepo.GetAll()
                .Where(s => s.TrainerId == trainerId && s.ClientId == clientId)
                .OrderByDescending(s => s.DateCreated)
                .ToList();
        }

        // recenzije
        public List<Review> GetMyReviews(Guid trainerId)
        {
            return _reviewRepo.GetAll()
                .Where(r => r.RevieweeId == trainerId)
                .OrderByDescending(r => r.Date)
                .ToList();
        }
        // spisak opreme klijenta
        public List<Equipment> GetClientEquipment(Guid clientId)
        {
            var client = _clientRepo.GetById(clientId);
            if (client == null || client.OwnedEquipmentIds == null) return new List<Equipment>();

            return _equipmentRepo.GetAll()
                .Where(e => client.OwnedEquipmentIds.Contains(e.Id))
                .ToList();
        }

        // sve sesije treninga za klijenta
        public List<TrainingSession> GetClientSessionsForTrainer(Guid trainerId, Guid clientId)
        {
            return _sessionRepo.GetAll()
                .Where(s => s.TrainerId == trainerId && s.ClientId == clientId)
                .OrderByDescending(s => s.DateCreated)
                .ToList();
        }
        
        // --- WORKOUT CRUD ---
        public void UpdateTrainingSession(TrainingSession session)
        {
            _sessionRepo.Update(session);
        }

        public void DeleteTrainingSession(Guid sessionId)
        {
            _sessionRepo.Delete(sessionId);
        }

        //Kalendar
        public List<TrainingSession> GetTrainerSessionsByDate(Guid trainerId, DateTime date)
        {
            return _sessionRepo.GetAll()
                .Where(s => s.TrainerId == trainerId && s.DateCreated.Date == date.Date)
                .OrderBy(s => s.DateCreated)
                .ToList();
        }

        // interno ocenjivanje klijenta
        public void RateClientInternally(Guid trainerId, Guid clientId, int rating, string note)
        {
            if (rating < 1 || rating > 5) throw new Exception("Ocena mora biti između 1 i 5.");

            var existing = _internalRatingRepo.GetAll()
                .FirstOrDefault(r => r.TrainerId == trainerId && r.ClientId == clientId);

            if (existing != null)
            {
                existing.Rating = rating;
                existing.Note = note;
                existing.Date = DateTime.Now;
                _internalRatingRepo.Update(existing);
            }
            else
            {
                _internalRatingRepo.Create(new ClientInternalRating
                {
                    TrainerId = trainerId,
                    ClientId = clientId,
                    Rating = rating,
                    Note = note,
                    Date = DateTime.Now
                });
            }
        }

        public ClientInternalRating? GetClientInternalRating(Guid trainerId, Guid clientId)
        {
            return _internalRatingRepo.GetAll()
                .FirstOrDefault(r => r.TrainerId == trainerId && r.ClientId == clientId);
        }

        public List<ClientInternalRating> GetAllClientInternalRatings(Guid clientId)
        {
            return _internalRatingRepo.GetAll()
                .Where(r => r.ClientId == clientId)
                .OrderByDescending(r => r.Date)
                .ToList();
        }
    }
}