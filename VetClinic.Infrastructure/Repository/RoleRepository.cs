using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(VetClinicContext context) : base(context) { }

    public async Task<Role?> GetByNameAsync(string name) =>
        await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
}