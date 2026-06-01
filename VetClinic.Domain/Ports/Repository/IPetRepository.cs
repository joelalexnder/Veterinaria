using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IPetRepository
{
    Task<IEnumerable<Pet>> GetAllAsync();
    Task<Pet?> GetByIdAsync(int id);
    Task<IEnumerable<Pet>> GetByOwnerAsync(int ownerId);
    Task<IEnumerable<Pet>> GetBySpeciesAsync(string species);
    Task AddAsync(Pet pet);
    Task UpdateAsync(Pet pet);
}