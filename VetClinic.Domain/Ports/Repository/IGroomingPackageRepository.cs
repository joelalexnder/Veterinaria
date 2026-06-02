using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IGroomingPackageRepository : IRepository<GroomingPackage>
{
    Task<IEnumerable<GroomingPackage>> GetAllWithServicesAsync();
}