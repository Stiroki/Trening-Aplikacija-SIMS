using System;
namespace TreningAplikacija.Models;

public interface IIdentifiable
{
    Guid Id { get; set; }
}