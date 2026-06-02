using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class GroomingServiceRepository : Repository<GroomingService>, IGroomingServiceRepository
{
    public GroomingServiceRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<GroomingService>> GetBySpeciesAndSizeAsync(string species, string size) =>
        await _context.GroomingServices
            .Where(g => g.Species == species && g.PetSize == size)
            .ToListAsync();
}