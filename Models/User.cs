using System;

namespace DefaultNamespace;

public abstract class User
{
    public Guid Id { get; set; } = Guid.newGuid();
    public string Name {get; set;};
    public string LastName {get; set;}
    public string Email {get; set;}
    public string Password {get; set;}
}