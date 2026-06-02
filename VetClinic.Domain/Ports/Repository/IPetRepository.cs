using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IPetRepository : IRepository<Pet>
{
    Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId);
    Task<IEnumerable<Pet>> GetBySpeciesAsync(string species);
    Task<Pet?> GetWithMedicalHistoryAsync(int petId);
}