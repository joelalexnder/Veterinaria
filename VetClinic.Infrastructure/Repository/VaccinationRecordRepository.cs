using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class VaccinationRecordRepository : Repository<VaccinationRecord>, IVaccinationRecordRepository
{
    public VaccinationRecordRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<VaccinationRecord>> GetByPetIdAsync(int petId) =>
        await _context.VaccinationRecords
            .Where(v => v.PetId == petId)
            .Include(v => v.Vaccine)
            .Include(v => v.Veterinarian)
            .ToListAsync();

    public async Task<IEnumerable<VaccinationRecord>> GetUpcomingAsync(int days)
    {
        var limit = DateOnly.FromDateTime(DateTime.Today.AddDays(days));
        return await _context.VaccinationRecords
            .Where(v => v.NextBoosterDate <= limit && v.NextBoosterDate >= DateOnly.FromDateTime(DateTime.Today))
            .Include(v => v.Pet)
            .Include(v => v.Vaccine)
            .Include(v => v.Veterinarian)
            .ToListAsync();
    }

    public async Task<IEnumerable<VaccinationRecord>> GetOverdueAsync() =>
        await _context.VaccinationRecords
            .Where(v => v.NextBoosterDate < DateOnly.FromDateTime(DateTime.Today))
            .Include(v => v.Pet)
            .Include(v => v.Vaccine)
            .Include(v => v.Veterinarian)
            .ToListAsync();
    
    public async Task<IEnumerable<VaccinationRecord>> GetAllWithDetailsAsync() =>
        await _context.VaccinationRecords
            .Include(v => v.Pet)
            .Include(v => v.Vaccine)
            .Include(v => v.Veterinarian)
            .ToListAsync();

    // NUEVO: registros de vacunación aplicados por un veterinario específico
    public async Task<IEnumerable<VaccinationRecord>> GetByVeterinarianIdAsync(int veterinarianId) =>
        await _context.VaccinationRecords
            .Where(v => v.VeterinarianId == veterinarianId)
            .Include(v => v.Pet)
            .Include(v => v.Vaccine)
            .Include(v => v.Veterinarian)
            .ToListAsync();
}