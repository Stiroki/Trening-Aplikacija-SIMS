using System;
using System.Linq;
using TreningAplikacija.Models;
using TreningAplikacija.Repositories;

namespace TreningAplikacija.Services
{
    public class AuthService
    {
        private readonly JsonRepository<Client> _clientRepo;
        private readonly JsonRepository<Trainer> _trainerRepo;
        private readonly JsonRepository<Admin> _adminRepo;

        public AuthService()
        {
            _clientRepo = new JsonRepository<Client>("clients.json");
            _trainerRepo = new JsonRepository<Trainer>("trainers.json");
            _adminRepo = new JsonRepository<Admin>("admins.json");

            if (!_adminRepo.GetAll().Any())
            {
                _adminRepo.Create(new Admin
                {
                    Name = "Admin",
                    LastName = "Admin",
                    Email = "admin@trening.com",
                    Password = "admin123"
                });
            }
        }

        public bool IsEmailTaken(string email)
        {
            bool isClientTaken = _clientRepo.GetAll().Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            bool isTrainerTaken = _trainerRepo.GetAll().Any(t => t.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            
            return isClientTaken || isTrainerTaken;
        }

        public void RegisterClient(Client newClient)
        {
            if (IsEmailTaken(newClient.Email))
            {
                throw new Exception("Nalog čeka odobrenje administratora."); 
            }
            
            _clientRepo.Create(newClient);
        }

        public void RegisterTrainer(Trainer newTrainer)
        {
            if (IsEmailTaken(newTrainer.Email))
            {
                throw new Exception("Nalog čeka odobrenje administratora.");
            }

            newTrainer.IsVerifiedByAdmin = false;
            
            _trainerRepo.Create(newTrainer);
        }

        public User? Login(string email, string password)
        {
            var admin = _adminRepo.GetAll()
                .FirstOrDefault(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && a.Password == password);

            if (admin != null)
                return admin;

            var client = _clientRepo.GetAll()
                .FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && c.Password == password);

            if (client != null)
            {
                if (!client.IsVerifiedByAdmin)
                {
                    throw new Exception("Nalog čeka odobrenje administratora.");
                }

                return client;
            }

            var trainer = _trainerRepo.GetAll()
                .FirstOrDefault(t => t.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && t.Password == password);
            
            if (trainer != null)
            {
                if (!trainer.IsVerifiedByAdmin)
                {
                    throw new Exception("Nalog čeka odobrenje administratora.");
                }
                
                return trainer;
            }

            return null; 
        }
    }
}