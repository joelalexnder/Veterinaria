using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class OwnerRepository : Repository<Owner>, IOwnerRepository
{
    public OwnerRepository(VetClinicContext context) : base(context) { }

    public async Task<Owner?> GetByDniAsync(string dni) =>
        await _context.Owners.FirstOrDefaultAsync(o => o.Dni == dni);

    public async Task<Owner?> GetWithPetsAsync(int ownerId) =>
        await _context.Owners
            .Include(o => o.Pets)
            .FirstOrDefaultAsync(o => o.Id == ownerId);
}