using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Ports.Repository;

public interface IGroomingAppointmentRepository : IRepository<GroomingAppointment>
{
    Task<IEnumerable<GroomingAppointment>> GetByDateAsync(DateOnly date);
    Task<IEnumerable<GroomingAppointment>> GetByPetIdAsync(int petId);
}