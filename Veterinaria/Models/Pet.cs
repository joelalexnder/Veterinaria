using System;
using System.Collections.Generic;

namespace Veterinaria.Models;

public partial class Pet
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public string Species { get; set; } = null!;

    public string? Breed { get; set; }

    public DateOnly? BirthDate { get; set; }

    public decimal? Weight { get; set; }

    public string? Sex { get; set; }

    public string? Color { get; set; }

    public string? GeneralHealthStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; } = new List<VaccinationRecord>();
}
