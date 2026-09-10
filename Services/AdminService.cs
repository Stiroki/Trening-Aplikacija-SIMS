using System;
using System.Collections.Generic;
using System.Linq;
using TreningAplikacija.Models;
using TreningAplikacija.Repositories;

namespace TreningAplikacija.Services
{
    public class AdminService
    {
        private readonly JsonRepository<Trainer> _trainerRepo;
        private readonly JsonRepository<Review> _reviewRepo;
        private readonly NotificationService _notificationService;

        public AdminService()
        {
            _trainerRepo = new JsonRepository<Trainer>("trainers.json");
            _reviewRepo = new JsonRepository<Review>("reviews.json");
            _notificationService = new NotificationService();
        }

        public List<Trainer> GetPendingTrainerRegistrations()
        {
            return _trainerRepo.GetAll()
                .Where(t => !t.IsVerifiedByAdmin)
                .ToList();
        }

        public void VerifyTrainer(Guid trainerId)
        {
            var trainer = _trainerRepo.GetById(trainerId);
            if (trainer == null) throw new Exception("Trainer not found.");

            trainer.IsVerifiedByAdmin = true;
            _trainerRepo.Update(trainer);

            _notificationService.CreateNotification(
                trainer.Id,
                NotificationType.RequestAccepted,
                "Vaša prijava je odobrena. Dobrodošli na platformu.");
        }

        public void RejectTrainer(Guid trainerId, string reason)
        {
            var trainer = _trainerRepo.GetById(trainerId);
            if (trainer == null) throw new Exception("Trainer not found.");

            _notificationService.CreateNotification(
                trainer.Id,
                NotificationType.RequestRejected,
                string.IsNullOrWhiteSpace(reason)
                    ? "Vaša prijava je odbijena."
                    : $"Vaša prijava je odbijena. Razlog: {reason}");

            _trainerRepo.Delete(trainerId);
        }

        public void RemoveTrainer(Guid trainerId)
        {
            _trainerRepo.Delete(trainerId);
        }
        
        public List<Review> GetAllReviews()
        {
            return _reviewRepo.GetAll()
                              .OrderByDescending(r => r.Date)
                              .ToList();
        }

        public List<Trainer> GetTrainersSortedByRating()
        {
            var trainers = _trainerRepo.GetAll();
            var reviews = _reviewRepo.GetAll();

            foreach (var trainer in trainers)
            {
                var trainerReviews = reviews.Where(r => r.RevieweeId == trainer.Id).ToList();
                
                if (trainerReviews.Any())
                {
                    trainer.AverageRating = trainerReviews.Average(r => r.Rating);
                }
                else
                {
                    trainer.AverageRating = 0;
                }
                
                _trainerRepo.Update(trainer);
            }

            return trainers.OrderByDescending(t => t.AverageRating).ToList();
        }
    }
}