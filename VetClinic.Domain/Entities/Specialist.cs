
namespace VetClinic.Domain.Entities;

public partial class Specialist
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ServiceAreaId { get; set; }

    public string? SpecialtyName { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ServiceArea ServiceArea { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
