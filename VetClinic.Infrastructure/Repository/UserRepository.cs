using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(VetClinicContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<IEnumerable<User>> GetByRoleIdAsync(int roleId) =>
        await _context.Users
            .Where(u => u.RoleId == roleId)
            .ToListAsync();
}