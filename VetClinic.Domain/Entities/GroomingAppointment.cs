
namespace VetClinic.Domain.Entities;

public partial class GroomingAppointment
{
    public int Id { get; set; }

    public int AppointmentId { get; set; }

    public int? GroomingPackageId { get; set; }

    public string? StylistNotes { get; set; }

    public decimal? FinalPrice { get; set; }

    public string? ServiceStatus { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual GroomingPackage? GroomingPackage { get; set; }
}
