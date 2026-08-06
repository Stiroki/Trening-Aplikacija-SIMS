using System;

namespace DefaultNamespace;

public class TrainingSession
{
    public Guid Id { get; set; } = Guid.newGuid();
    public Guid ClinetId { get; set; }
    public Guid TrainerId { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public bool isCompleted { get; set; } = false;
    public List<TrainingItem> Items { get; set; } = new List<TrainingItem>();
    
    public int? OverallRating { get; set; }
    public string OverallComment { get; set; }
}