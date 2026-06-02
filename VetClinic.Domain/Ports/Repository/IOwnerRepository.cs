using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IOwnerRepository : IRepository<Owner>
{
    Task<Owner?> GetByDniAsync(string dni);
    Task<Owner?> GetWithPetsAsync(int ownerId);
}