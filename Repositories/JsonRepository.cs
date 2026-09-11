using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TreningAplikacija.Models;

namespace TreningAplikacija.Repositories
{
    public class JsonRepository<T> where T : class, IIdentifiable
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;
        private static readonly object _lock = new object();

        public JsonRepository(string fileName)
        {
            string rootDir = GetProjectRootDirectory();
            string dataDirectory = Path.Combine(rootDir, "Data");

            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            _filePath = Path.Combine(dataDirectory, fileName);
            _options = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        private static string GetProjectRootDirectory()
        {
            var currentDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (currentDir != null)
            {
                if (currentDir.GetFiles("*.csproj").Length > 0 || currentDir.GetFiles("*.sln*").Length > 0)
                {
                    return currentDir.FullName;
                }
                currentDir = currentDir.Parent;
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public List<T> GetAll()
        {
            lock (_lock)
            {
                if (!File.Exists(_filePath))
                {
                    return new List<T>();
                }

                try
                {
                    string json = File.ReadAllText(_filePath);
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        return new List<T>();
                    }

                    return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
                }
                catch
                {
                    return new List<T>();
                }
            }
        }

        public void SaveAll(List<T> entities)
        {
            lock (_lock)
            {
                string json = JsonSerializer.Serialize(entities, _options);
                File.WriteAllText(_filePath, json);
            }
        }

        public void Create(T entity)
        {
            var entities = GetAll();
            entities.Add(entity);
            SaveAll(entities);
        }

        public T? GetById(Guid id)
        {
            var entities = GetAll();
            return entities.FirstOrDefault(e => e.Id == id);
        }

        public void Update(T updatedEntity)
        {
            var entities = GetAll();
            var index = entities.FindIndex(e => e.Id == updatedEntity.Id);
            
            if (index != -1)
            {
                entities[index] = updatedEntity;
                SaveAll(entities);
            }
        }

        public void Delete(Guid id)
        {
            var entities = GetAll();
            var entityToRemove = entities.FirstOrDefault(e => e.Id == id);
            
            if (entityToRemove != null)
            {
                entities.Remove(entityToRemove);
                SaveAll(entities);
            }
        }
    }
}