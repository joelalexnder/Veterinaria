namespace VetClinic.Domain.Ports.Repository;

public interface IUnitOfWork : IDisposable
{
    IPetRepository Pets { get; }
    IOwnerRepository Owners { get; }
    IMedicalRecordRepository MedicalRecords { get; }
    IAppointmentRepository Appointments { get; }
    IVaccinationRecordRepository VaccinationRecords { get; }
    IUserRepository Users { get; }
    IGroomingAppointmentRepository GroomingAppointments { get; }
    ISpecialistRepository Specialists { get; }
    IVaccineRepository Vaccines { get; }
    IGroomingServiceRepository GroomingServices { get; }
    IGroomingPackageRepository GroomingPackages { get; }
    IRoleRepository Roles { get; }
    IServiceAreaRepository ServiceAreas { get; }
    IAuditLogRepository AuditLogs { get; }
    IRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
}