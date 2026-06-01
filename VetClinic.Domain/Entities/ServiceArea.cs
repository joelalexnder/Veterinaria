
namespace VetClinic.Domain.Entities;

public partial class ServiceArea
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();
}
