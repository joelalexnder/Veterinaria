using System;
using System.Collections.Generic;

namespace Veterinaria.Models;

public partial class GroomingService
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal BasePrice { get; set; }

    public string Species { get; set; } = null!;

    public string PetSize { get; set; } = null!;

    public string? CoatType { get; set; }
}
