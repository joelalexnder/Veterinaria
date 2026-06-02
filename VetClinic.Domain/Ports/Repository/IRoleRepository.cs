using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
}