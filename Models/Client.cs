using System;
using System.Collections.Generic;

namespace TreningAplikacija.Models;

public class Client : User
{
    public double Height { get; set; }
    public double Weight { get; set; }
    public string HealthIssues { get; set; } = string.Empty;
    public string Goals { get; set; } = string.Empty;
    public string LocationPreference { get; set; } = string.Empty;
    public int TrainingsPerWeek { get; set; }
    public List<Guid> OwnedEquipmentIds { get; set; } = new List<Guid>();
}