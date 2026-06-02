using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class VaccineRepository : Repository<Vaccine>, IVaccineRepository
{
    public VaccineRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<Vaccine>> GetBySpeciesAsync(string species) =>
        await _context.Vaccines
            .Where(v => v.Species == species)
            .ToListAsync();
}