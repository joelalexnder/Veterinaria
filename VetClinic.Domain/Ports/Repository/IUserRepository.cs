using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByRoleIdAsync(int roleId);
    
    Task<User?> GetUserWithRoleByIdAsync(int id);
    
}