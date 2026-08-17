using System;
using System.Collections.Generic;

namespace TreningAplikacija.Models;

public class Exercise : IIdentifiable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TrainerId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string VideoUrl { get; set; } // nez kako cemo prikazati jos tkd placeholder

    public List<Guid> EquipmentIds { get; set; } = new List<Guid>();
}