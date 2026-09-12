using System;
using System.Collections.Generic;
using System.Linq;
using TreningAplikacija.Repositories;
using TreningAplikacija.Models;

namespace TreningAplikacija.Services;

public class ClientService
{
    private readonly JsonRepository<Client> _clientRepo;
    private readonly JsonRepository<Trainer> _trainerRepo;
    private readonly JsonRepository<TrainerRequest> _requestRepo;
    private readonly JsonRepository<Review> _reviewRepo;
    private readonly JsonRepository<ProgressEntry> _progressRepo;

    public ClientService()
    {
        _clientRepo = new JsonRepository<Client>("clients.json");
        _trainerRepo = new JsonRepository<Trainer>("trainers.json");
        _requestRepo = new JsonRepository<TrainerRequest>("trainer_requests.json");
        _reviewRepo = new JsonRepository<Review>("reviews.json");
        _progressRepo = new JsonRepository<ProgressEntry>("progress_entries.json");
    }

    // Profil
    public Client? GetClient(Guid clientId)
    {
        return _clientRepo.GetById(clientId);
    }

    public void UpdateProfile(Client client)
    {
        _clientRepo.Update(client);
    }
    
    // Treneri
    public List<Trainer> GetVerifiedTrainers()
    {
        return _trainerRepo.GetAll()
            .Where(t => t.IsVerifiedByAdmin)
            .ToList();
    }
    
    // Zahtevi
    public void SendRequest(Guid clientId, Guid trainerId, string message)
    {
        bool existing = _requestRepo.GetAll()
            .Any(r => r.ClientId == clientId
                && r.TrainerId == trainerId && r.Status == RequestStatus.Pending);
        
        if (existing)
        {
            throw new Exception("Vec ste poslali zahtev ovom treneru.");
        }

        TrainerRequest request = new TrainerRequest
        {
            ClientId = clientId,
            TrainerId = trainerId,
            Message = message
        };
        
        _requestRepo.Create(request);
    }

    public List<TrainerRequest> GetMyRequests(Guid clientId)
    {
        return _requestRepo.GetAll()
            .Where(r => r.ClientId == clientId)
            .OrderByDescending(r => r.DateSent)
            .ToList();
    }
    
    // Napredak
    public void AddProgressEntry(ProgressEntry entry)
    {
        _progressRepo.Create(entry);
    }

    public List<ProgressEntry> GetProgressHistory(Guid clientId)
    {
        return _progressRepo.GetAll()
            .Where(p => p.ClientId == clientId)
            .OrderByDescending(p => p.Date)
            .ToList();
    }
    
    //Recenzije
    public void ReviewTrainer(Guid clientId, Guid trainerId, int rating, string comment)
    {
        Review existing = _reviewRepo.GetAll()
            .FirstOrDefault(r => r.ReviewerId == clientId && r.RevieweeId == trainerId);
        if (existing != null)
        {
            throw new Exception("Ocenili ste ovog trenera.");
        }
        if (rating < 1 || rating > 5)
        {
            throw new Exception("Ocena mora biti izmedju 1 i 5.");
        }

        Review review = new Review
        {
            ReviewerId = clientId,
            RevieweeId = trainerId,
            Rating = rating,
            Comment = comment
        };
        
        _reviewRepo.Create(review);
    }

    public List<Review> GetTrainerReviews(Guid trainerId)
    {
        return _reviewRepo.GetAll()
            .Where(r => r.RevieweeId == trainerId)
            .OrderByDescending(r => r.Date)
            .ToList();
    }
    
    public bool HasReviewedTrainer(Guid clientId, Guid trainerId)
    {
        return _reviewRepo.GetAll()
            .Any(r => r.ReviewerId == clientId && r.RevieweeId == trainerId);
    }
}