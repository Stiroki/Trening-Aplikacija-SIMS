using System;

namespace TreningAplikacija.Models;

public class ClientInternalRating : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TrainerId { get; set; }
    public Guid ClientId { get; set; }
    public int Rating { get; set; } // 1 - 5
    public string Note { get; set; } = string.Empty; // zalaganje, disciplina, navike
    public DateTime Date { get; set; } = DateTime.Now;
}