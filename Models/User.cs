using System;

namespace TreningAplikacija.Models;

public abstract class User : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
}