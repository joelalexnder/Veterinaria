using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetClinic.Application.Mappings;
using VetClinic.Application.UseCases.Auth.Commands;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Domain.Ports.Services;
using VetClinic.Infrastructure.Context;
using VetClinic.Infrastructure.Repository;
using VetClinic.Infrastructure.Services;
namespace VetClinic.Infrastructure.Configuracion;
public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<VetClinicContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
       
        services.AddHttpClient<IAiRecommendationService, GeminiAiService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        
        services.AddScoped<IExcelReportService, ExcelReportService>();
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(LoginCommand).Assembly));
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}
