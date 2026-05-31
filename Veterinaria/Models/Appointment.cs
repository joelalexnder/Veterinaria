using System;
using System.Collections.Generic;

namespace Veterinaria.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int PetId { get; set; }

    public int SpecialistId { get; set; }

    public int ServiceAreaId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Status { get; set; }

    public string? CancellationReason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual GroomingAppointment? GroomingAppointment { get; set; }

    public virtual Pet Pet { get; set; } = null!;

    public virtual ServiceArea ServiceArea { get; set; } = null!;

    public virtual Specialist Specialist { get; set; } = null!;
}
