using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IServiceAreaRepository : IRepository<ServiceArea>
{
    Task<ServiceArea?> GetByNameAsync(string name);
}