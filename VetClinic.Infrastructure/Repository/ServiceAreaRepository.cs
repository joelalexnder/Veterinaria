using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class ServiceAreaRepository : Repository<ServiceArea>, IServiceAreaRepository
{
    public ServiceAreaRepository(VetClinicContext context) : base(context) { }

    public async Task<ServiceArea?> GetByNameAsync(string name) =>
        await _context.ServiceAreas
            .FirstOrDefaultAsync(s => s.Name == name);
}