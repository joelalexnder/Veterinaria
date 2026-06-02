using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetClinic.Infrastructure.Repository;

namespace VetClinic.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Base de datos PostgreSQL con Neon/Supabase
        services.AddDbContext<VetClinicContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}