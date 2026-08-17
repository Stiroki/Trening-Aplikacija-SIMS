using System;

namespace TreningAplikacija.Models;

public class Client : User
{
    public double Height {get; set;}
    public double Weight {get; set;}
    public string HealthIssues {get; set;}
    public string Goals {get; set;}
    public string LocationPreference {get; set;}
}