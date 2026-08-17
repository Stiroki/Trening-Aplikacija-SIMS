using System;

namespace TreningAplikacija.Models;

public class TrainingItem
{
    public Guid ExerciseId { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public string Duration { get; set; } // fazon koliko rest ili hold
    
    public int? Rating { get; set; }
    public string ClientComment { get; set; }
}