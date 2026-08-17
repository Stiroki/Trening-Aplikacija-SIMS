using System;
using System.Collections.Generic;

namespace TreningAplikacija.Models;

public class TrainingSession : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid TrainerId { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public bool IsCompleted { get; set; } = false;
    public List<TrainingItem> Items { get; set; } = new List<TrainingItem>();
    
    public int? OverallRating { get; set; }
    public string OverallComment { get; set; } =  string.Empty;
}