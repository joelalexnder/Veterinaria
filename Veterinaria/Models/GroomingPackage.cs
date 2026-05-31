using System;
using System.Collections.Generic;

namespace Veterinaria.Models;

public partial class GroomingPackage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal SpecialPrice { get; set; }

    public virtual ICollection<GroomingAppointment> GroomingAppointments { get; set; } = new List<GroomingAppointment>();
}
