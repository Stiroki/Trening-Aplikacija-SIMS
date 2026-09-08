using System;

namespace TreningAplikacija.Models;

public class ProgressEntry : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    
    public double Chest { get; set; }
    public double Waist { get; set; }
    public double Hips { get; set; }
    public double Biceps { get; set; }
    public double Thigh { get; set; }
    
    public double Weight { get; set; }
    public string Comment { get; set; } = string.Empty;
}