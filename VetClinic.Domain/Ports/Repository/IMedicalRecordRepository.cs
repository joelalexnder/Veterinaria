using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IMedicalRecordRepository : IRepository<MedicalRecord>
{
    Task<IEnumerable<MedicalRecord>> GetByPetIdAsync(int petId);
    Task<IEnumerable<MedicalRecord>> GetByDateRangeAsync(int petId, DateTime from, DateTime to);
}