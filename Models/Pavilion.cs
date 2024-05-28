using System;
using System.Collections.Generic;

namespace Rudenko_praktika.Models;

public partial class Pavilion
{
    public int PavilionId { get; set; }

    public string PavilionName { get; set; } = null!;

    public string PavilionType { get; set; } = null!;

    public decimal PavilionArea { get; set; }

    public int? ParkId { get; set; }

    public virtual Park? Park { get; set; }
}
