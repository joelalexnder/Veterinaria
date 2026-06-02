using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class GroomingPackageRepository : Repository<GroomingPackage>, IGroomingPackageRepository
{
    public GroomingPackageRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<GroomingPackage>> GetAllWithServicesAsync() =>
        await _context.GroomingPackages
            .Include(g => g.GroomingAppointments)
            .ToListAsync();
}