using System;
using System.Collections.Generic;

namespace Veterinaria.Models;

public partial class MedicalRecord
{
    public int Id { get; set; }

    public int PetId { get; set; }

    public int VeterinarianId { get; set; }

    public DateTime? ConsultDate { get; set; }

    public string Reason { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string? Treatment { get; set; }

    public string? Observations { get; set; }

    public decimal? RegisteredWeight { get; set; }

    public virtual Pet Pet { get; set; } = null!;

    public virtual User Veterinarian { get; set; } = null!;
}
