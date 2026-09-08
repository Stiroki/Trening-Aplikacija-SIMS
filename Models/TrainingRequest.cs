using System;

namespace TreningAplikacija.Models;

public enum RequestStatus
{
    Pending,
    Accepted,
    Rejected
}

public class TrainingRequest : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid TrainerId { get; set; }
    public DateTime DateSent { get; set; } = DateTime.Now;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public string Message { get; set; } = string.Empty;
    public string ResponseMessage { get; set; } = string.Empty;
}
