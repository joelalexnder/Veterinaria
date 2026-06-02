using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Entities;
using VetClinic.Infrastructure.Context;

namespace VetClinic.Infrastructure.Repository;

public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(VetClinicContext context) : base(context) { }

    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId) =>
        await _context.AuditLogs
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();

    public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime from, DateTime to) =>
        await _context.AuditLogs
            .Where(a => a.Timestamp >= from && a.Timestamp <= to)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
}