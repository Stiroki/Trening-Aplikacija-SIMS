using System;

namespace TreningAplikacija.Models;

public enum NotificationType
{
    RequestAccepted,
    RequestRejected,
    NewTraining,
    PaymentRequired,
    GoalReached
}
public class Notification : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public bool IsRead { get; set; } = false;
}