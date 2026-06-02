using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IVaccineRepository : IRepository<Vaccine>
{
    Task<IEnumerable<Vaccine>> GetBySpeciesAsync(string species);
}