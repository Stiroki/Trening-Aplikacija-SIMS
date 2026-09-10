using System;

namespace TreningAplikacija.Models;

public enum PaymentStatus
{
    Pending,
    Paid,
    Overdue
}

public enum PaymentType
{
    ClientToTrainer,
    TrainerCommission
}

public class Payment : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid TrainerId { get; set; }
    public double Amount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string Period { get; set; } = string.Empty;
    public PaymentType Type { get; set; } = PaymentType.ClientToTrainer;
}