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

    public async Task<IEnumerable<Pet>> SearchAsync(string? name, string? species, int? ownerId)
    {
        var query = _context.Pets.Include(p => p.Owner).AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.ILike(p.Name, $"%{name}%"));

        if (!string.IsNullOrWhiteSpace(species))
            query = query.Where(p => p.Species == species);

        if (ownerId.HasValue)
            query = query.Where(p => p.OwnerId == ownerId.Value);

        return await query.OrderBy(p => p.Name).ToListAsync();
    }
}