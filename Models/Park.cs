using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Rudenko_praktika.Models;

public partial class Park
{
    public int ParkId { get; set; }

    public string ParkName { get; set; } = null!;

    public string PlantingDensity { get; set; } = null!;

    public decimal ParkArea { get; set; }

    public TimeOnly? ParkOpening { get; set; }

    public TimeOnly? ParkClosure { get; set; }
    [NotMapped]
    public string Schedule
    {
        get
        {
            string res = "";
            res = ParkOpening?.ToString("HH:mm") + "-" + ParkClosure?.ToString("HH:mm");
            if (res == "00:00-00:00")
                return "круглосуточно";
            return res;
        }
    }

    public string City { get; set; } = null!;

    public string? Street { get; set; }

    public string? Building { get; set; }

    public int Index { get; set; }
    [NotMapped]
    public string Address
    {
        get
        {
            string res = City + ", ";
            if (Street != null)
                res += Street + ", ";
            if (Building != null)
                res += Building + ", ";
            res += Index;

            return res;
        }
    }

    public string? PhoneNumber { get; set; }
    [NotMapped]
    public string PhoneView
    {
        get
        {
            string res = "нет";
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
                res = PhoneNumber;
            return res;
        }
    }

    public string? Website { get; set; }
    [NotMapped]
    public string WebsiteView
    {
        get
        {
            string res = "нет";
            if (!string.IsNullOrWhiteSpace(Website))
                res = Website;
            return res;
        }
    }

    public virtual ICollection<Fountain> Fountains { get; set; } = new List<Fountain>();

    public virtual ICollection<Pavilion> Pavilions { get; set; } = new List<Pavilion>();

    public virtual ICollection<PlantingPark> PlantingParks { get; set; } = new List<PlantingPark>();

}
