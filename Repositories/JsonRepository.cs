namespace DefaultNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

// mozemo i nes drugo ali reko json najlaksi zbog oop
public class JsonRepository<T> where T : class
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options;

    public JsonRepository(string fileName)
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        _options = new JsonSerializerOptions { WriteIndented = true };
    }
    
    public List<T> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
    }
    
    public void SaveAll(List<T> entities)
    {
        string json = JsonSerializer.Serialize(entities, _options);
        File.WriteAllText(_filePath, json);
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