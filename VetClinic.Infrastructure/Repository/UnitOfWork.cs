using System.Collections;
using VetClinic.Domain;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;
using VetClinic.Infrastructure.Repository;



namespace VetClinic.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly VetClinicContext _context;
    private readonly Hashtable _repositories;

    // Repositorios específicos
    private IPetRepository? _pets;
    private IOwnerRepository? _owners;
    private IMedicalRecordRepository? _medicalRecords;
    private IAppointmentRepository? _appointments;
    private IVaccinationRecordRepository? _vaccinationRecords;
    private IUserRepository? _users;
    private IGroomingAppointmentRepository? _groomingAppointments;
    private ISpecialistRepository? _specialists;
    private IVaccineRepository? _vaccines;
    private IGroomingServiceRepository? _groomingServices;
    private IGroomingPackageRepository? _groomingPackages;
    private IRoleRepository? _roles;
    private IServiceAreaRepository? _serviceAreas;
    private IAuditLogRepository? _auditLogs;

    public UnitOfWork(VetClinicContext context)
    {
        _context = context;
        _repositories = new Hashtable();
    }

    public IPetRepository Pets => _pets ??= new PetRepository(_context);
    public IOwnerRepository Owners => _owners ??= new OwnerRepository(_context);
    public IMedicalRecordRepository MedicalRecords => _medicalRecords ??= new MedicalRecordRepository(_context);
    public IAppointmentRepository Appointments => _appointments ??= new AppointmentRepository(_context);
    public IVaccinationRecordRepository VaccinationRecords => _vaccinationRecords ??= new VaccinationRecordRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IGroomingAppointmentRepository GroomingAppointments => _groomingAppointments ??= new GroomingAppointmentRepository(_context);
    public ISpecialistRepository Specialists => _specialists ??= new SpecialistRepository(_context);
    public IVaccineRepository Vaccines => _vaccines ??= new VaccineRepository(_context);
    public IGroomingServiceRepository GroomingServices => _groomingServices ??= new GroomingServiceRepository(_context);
    public IGroomingPackageRepository GroomingPackages => _groomingPackages ??= new GroomingPackageRepository(_context);
    public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
    public IServiceAreaRepository ServiceAreas => _serviceAreas ??= new ServiceAreaRepository(_context);
    public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(_context);

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;

        if (_repositories.ContainsKey(type))
            return (IRepository<T>)_repositories[type]!;

        var repositoryInstance = new Repository<T>(_context);
        _repositories.Add(type, repositoryInstance);
        return repositoryInstance;
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}


