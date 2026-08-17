using System;
using System.Collections.Generic;
using System.Linq;
using TreningAplikacija.Models;
using TreningAplikacija.Repositories;

namespace TreningAplikacija.Services
{
    public class TrainingService
    {
        private readonly JsonRepository<Exercise> _exerciseRepo;
        private readonly JsonRepository<TrainingSession> _sessionRepo;

        public TrainingService()
        {
            _exerciseRepo = new JsonRepository<Exercise>("exercises.json");
            _sessionRepo = new JsonRepository<TrainingSession>("training_sessions.json");
        }
        
        public void CreateExercise(Exercise exercise)
        {
            _exerciseRepo.Create(exercise);
        }

        public List<Exercise> GetTrainerExercises(Guid trainerId)
        {
            return _exerciseRepo.GetAll().Where(e => e.TrainerId == trainerId).ToList();
        }

        public void AssignTrainingSession(TrainingSession session)
        {
            if (session.Items.Count == 0)
            {
                throw new Exception("Training session must contain at least one exercise.");
            }
            _sessionRepo.Create(session);
        }

        public List<TrainingSession> GetClientSessions(Guid clientId)
        {
            return _sessionRepo.GetAll()
                               .Where(s => s.ClientId == clientId)
                               .OrderByDescending(s => s.DateCreated)
                               .ToList();
        }

        public void RateExercise(Guid sessionId, Guid exerciseId, int rating, string comment)
        {
            var session = _sessionRepo.GetById(sessionId);
            if (session == null) throw new Exception("Session not found.");

            var item = session.Items.FirstOrDefault(i => i.ExerciseId == exerciseId);
            if (item == null) throw new Exception("Exercise not found in this session.");

            item.Rating = rating;
            item.ClientComment = comment;

            _sessionRepo.Update(session);
        }

        public void CompleteAndRateSession(Guid sessionId, int overallRating, string overallComment)
        {
            var session = _sessionRepo.GetById(sessionId);
            if (session == null) throw new Exception("Session not found.");

            session.IsCompleted = true;
            session.OverallRating = overallRating;
            session.OverallComment = overallComment;

            _sessionRepo.Update(session);
        }
    }
}