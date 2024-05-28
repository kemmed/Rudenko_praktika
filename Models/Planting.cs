using System;
using System.Collections.Generic;

namespace Rudenko_praktika.Models;

public partial class Planting
{
    public int PlantingId { get; set; }

    public string PlantingName { get; set; } = null!;

    public string CultureType { get; set; } = null!;

    public decimal AverageLifeExpectancy { get; set; }

    public virtual ICollection<PlantingPark> PlantingParks { get; set; } = new List<PlantingPark>();
}
