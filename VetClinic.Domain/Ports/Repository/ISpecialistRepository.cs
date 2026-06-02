using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface ISpecialistRepository : IRepository<Specialist>
{
    Task<IEnumerable<Specialist>> GetByServiceAreaAsync(int serviceAreaId);
    Task<Specialist?> GetByUserIdAsync(int userId);
}