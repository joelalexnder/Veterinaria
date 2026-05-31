using System;
using System.Collections.Generic;

namespace Veterinaria.Models;

public partial class AuditLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string ResourceName { get; set; } = null!;

    public string? ResourceId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Details { get; set; }

    public virtual User? User { get; set; }
}
