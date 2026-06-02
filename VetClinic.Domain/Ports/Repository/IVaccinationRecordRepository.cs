using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IVaccinationRecordRepository : IRepository<VaccinationRecord>
{
    Task<IEnumerable<VaccinationRecord>> GetByPetIdAsync(int petId);
    Task<IEnumerable<VaccinationRecord>> GetUpcomingAsync(int days);
    Task<IEnumerable<VaccinationRecord>> GetOverdueAsync();
}