
namespace VetClinic.Domain.Entities;

public partial class Owner
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
