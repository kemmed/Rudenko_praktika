using System;
using System.Collections.Generic;

namespace Rudenko_praktika.Models;

public partial class PlantingPark
{
    public int PlParkId { get; set; }

    public int ParkId { get; set; }

    public int PlantingId { get; set; }

    public int PlantingsNumber { get; set; }

    public virtual Park Park { get; set; } = null!;

    public virtual Planting Planting { get; set; } = null!;
}
