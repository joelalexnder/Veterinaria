using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IGroomingServiceRepository : IRepository<GroomingService>
{
    Task<IEnumerable<GroomingService>> GetBySpeciesAndSizeAsync(string species, string size);
}