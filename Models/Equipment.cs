using System;

namespace TreningAplikacija.Models;

public class Equipment : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Type { get; set; } // sprava ili oprema neka
    public string Description { get; set; }
}