using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class SpecialistRepository : Repository<Specialist>, ISpecialistRepository
{
    public SpecialistRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<Specialist>> GetByServiceAreaAsync(int serviceAreaId) =>
        await _context.Specialists
            .Where(s => s.ServiceAreaId == serviceAreaId)
            .Include(s => s.User)
            .Include(s => s.ServiceArea)
            .ToListAsync();

    public async Task<Specialist?> GetByUserIdAsync(int userId) =>
        await _context.Specialists
            .FirstOrDefaultAsync(s => s.UserId == userId);

    public async Task<IEnumerable<Specialist>> GetAllWithDetailsAsync() =>
        await _context.Specialists
            .Include(s => s.User)
            .Include(s => s.ServiceArea)
            .ToListAsync();
}