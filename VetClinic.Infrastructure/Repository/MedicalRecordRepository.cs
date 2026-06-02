using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class MedicalRecordRepository : Repository<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<MedicalRecord>> GetByPetIdAsync(int petId) =>
        await _context.MedicalRecords
            .Where(m => m.PetId == petId)
            .OrderByDescending(m => m.ConsultDate)
            .ToListAsync();

    public async Task<IEnumerable<MedicalRecord>> GetByDateRangeAsync(int petId, DateTime from, DateTime to) =>
        await _context.MedicalRecords
            .Where(m => m.PetId == petId && m.ConsultDate >= from && m.ConsultDate <= to)
            .OrderByDescending(m => m.ConsultDate)
            .ToListAsync();
}