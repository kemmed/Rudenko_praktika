using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rudenko_praktika.Models;

public partial class Fountain
{
    public int FountainId { get; set; }
    public int FountainCipher { get; set; }

    public DateTime? ConstructionDate { get; set; }
    [NotMapped]
    public string DateViwe
    {
        get
        {
            string res = "-";
            if (ConstructionDate != null)
                res = ConstructionDate.Value.ToString("dd.MM.yyyy");
            return res;
        }
    }

    public decimal FountainArea { get; set; }

    public decimal NormalWaterConsumption { get; set; }

    public decimal MaximumWaterConsumption { get; set; }

    public int? ParkId { get; set; }

    public virtual Park? Park { get; set; }
}
