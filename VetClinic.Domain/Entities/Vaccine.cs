
namespace VetClinic.Domain.Entities;

public partial class Vaccine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Species { get; set; } = null!;

    public int MinAgeDays { get; set; }

    public int BoosterIntervalDays { get; set; }

    public virtual ICollection<VaccinationRecord> VaccinationRecords { get; set; } = new List<VaccinationRecord>();
}
