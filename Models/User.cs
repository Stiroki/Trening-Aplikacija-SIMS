using System;

namespace TreningAplikacija.Models;

public abstract class User : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
}