using System;

namespace TreningAplikacija.Models;

public class Trainer : User
{
    public double MonthlyFee {get; set;}
    public double FeePerSession {get; set;}
    public bool IsVerifiedByAdmin { get; set; } = false;
    public double AverageRating {get; set;}
    public int WarningCount { get; set; } = 0;
}