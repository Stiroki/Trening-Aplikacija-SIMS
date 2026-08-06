using System;

namespace DefaultNamespace;

public class Review
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReviewerId { get; set; }
    public Guid RevieweeId { get; set; }
    public int Raiting { get; set; }
    public string Comment { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}