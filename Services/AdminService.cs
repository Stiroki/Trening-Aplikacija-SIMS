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

        public AdminService()
        {
            _trainerRepo = new JsonRepository<Trainer>("trainers.json");
            _reviewRepo = new JsonRepository<Review>("reviews.json");
        }

        public void VerifyTrainer(Guid trainerId)
        {
            var trainer = _trainerRepo.GetById(trainerId);
            if (trainer == null) throw new Exception("Trainer not found.");

            trainer.IsVerifiedByAdmin = true;
            _trainerRepo.Update(trainer);
        }

        public void RemoveTrainer(Guid trainerId)
        {
            // treba dodati brisanje i treninga od trenera i vezbi itd
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