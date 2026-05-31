using System;
using System.Collections.Generic;

namespace Veterinaria.Models;

public partial class VaccinationRecord
{
    public int Id { get; set; }

    public int PetId { get; set; }

    public int VaccineId { get; set; }

    public int VeterinarianId { get; set; }

    public string? BatchNumber { get; set; }

    public DateOnly? ApplicationDate { get; set; }

    public DateOnly? NextBoosterDate { get; set; }

    public virtual Pet Pet { get; set; } = null!;

    public virtual Vaccine Vaccine { get; set; } = null!;

    public virtual User Veterinarian { get; set; } = null!;
}
