using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class PetRepository : Repository<Pet>, IPetRepository
{
    public PetRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId) =>
        await _context.Pets
            .Include(p => p.Owner) 
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();

    public async Task<IEnumerable<Pet>> GetBySpeciesAsync(string species) =>
        await _context.Pets.Where(p => p.Species == species).ToListAsync();

    public async Task<Pet?> GetWithMedicalHistoryAsync(int petId) =>
        await _context.Pets
            .Include(p => p.Owner)
            .Include(p => p.MedicalRecords)
                .ThenInclude(mr => mr.Veterinarian)
            .FirstOrDefaultAsync(p => p.Id == petId);
}