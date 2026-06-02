using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetByDateAsync(DateOnly date);
    Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId);
    Task<bool> HasConflictAsync(int specialistId, int serviceAreaId, DateOnly date, TimeOnly start, TimeOnly end);
}