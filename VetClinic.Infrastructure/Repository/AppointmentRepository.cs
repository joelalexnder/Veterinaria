using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<Appointment>> GetByDateAsync(DateOnly date) =>
        await _context.Appointments
            .Where(a => a.AppointmentDate == date)
            .Include(a => a.Pet)
            .Include(a => a.Specialist)
            .Include(a => a.ServiceArea)
            .ToListAsync();

    public async Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId) =>
        await _context.Appointments
            .Where(a => a.PetId == petId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

    public async Task<bool> HasConflictAsync(int specialistId, int serviceAreaId, DateOnly date, TimeOnly start, TimeOnly end) =>
        await _context.Appointments.AnyAsync(a =>
            a.AppointmentDate == date &&
            (a.SpecialistId == specialistId || a.ServiceAreaId == serviceAreaId) &&
            a.Status != "Cancelada" &&
            a.StartTime < end &&
            a.EndTime > start);
}