namespace VetClinic.Web.Models;

public class VaccineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Species { get; set; } = "";
    public int MinAgeDays { get; set; }
    public int BoosterIntervalDays { get; set; }
}

public class AddVaccineCatalogRequest
{
    public string Name { get; set; } = "";
    public string Species { get; set; } = "";
    public int MinAgeDays { get; set; }
    public int BoosterIntervalDays { get; set; }
}

public class VaccinationRecordDto
{
    public int Id { get; set; }
    public string PetName { get; set; } = "";
    public string VaccineName { get; set; } = "";
    public string VeterinarianName { get; set; } = "";
    public string? BatchNumber { get; set; }
    public DateOnly? ApplicationDate { get; set; }
    public DateOnly? NextBoosterDate { get; set; }
    public int DaysOverdue { get; set; }
    public string Status { get; set; } = "";
}

public class RegisterVaccinationRequest
{
    public int PetId { get; set; }
    public int VaccineId { get; set; }
    public int VeterinarianId { get; set; }
    public string? BatchNumber { get; set; }
    public DateOnly ApplicationDate { get; set; }
    public DateOnly NextBoosterDate { get; set; }
}