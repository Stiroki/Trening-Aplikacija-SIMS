using System;

namespace DefaultNamespace;

public class Trainer : User
{
    public double MonthlyFee {get; set;}
    public double FeePerSession {get; set;}
    public bool isVerifiedByAdmin { get; set; } = false;
    public double AverageRaing {get; set;}
}