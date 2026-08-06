using System;
namespace DefaultNamespace;

public interface IIdentifiable
{
    Guid Id { get; set; }
}