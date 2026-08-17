using System;

namespace TreningAplikacija.Models;

public class Review : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReviewerId { get; set; }
    public Guid RevieweeId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } =  string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
}