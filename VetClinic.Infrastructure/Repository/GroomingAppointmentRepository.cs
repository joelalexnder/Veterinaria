using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class GroomingAppointmentRepository : Repository<GroomingAppointment>, IGroomingAppointmentRepository
{
    public GroomingAppointmentRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<GroomingAppointment>> GetByDateAsync(DateOnly date) =>
        await _context.GroomingAppointments
            .Include(g => g.Appointment)
            .Where(g => g.Appointment.AppointmentDate == date)
            .ToListAsync();

    public async Task<IEnumerable<GroomingAppointment>> GetByPetIdAsync(int petId) =>
        await _context.GroomingAppointments
            .Include(g => g.Appointment)
            .Where(g => g.Appointment.PetId == petId)
            .ToListAsync();
}