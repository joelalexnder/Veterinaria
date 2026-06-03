using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Services;

public interface IAuthService
{
    string GenerateToken(User user);
    bool Verify(string plain, string hash);
    string Hash(string plain);
}