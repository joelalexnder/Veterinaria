namespace VetClinic.Domain.DTOs.VaccinationRecord;

public class RegisterVaccinationDto
{
    public int PetId { get; set; }
    public int VaccineId { get; set; }
    public int VeterinarianId { get; set; }
    public string? BatchNumber { get; set; }
    public DateOnly ApplicationDate { get; set; }
    public DateOnly NextBoosterDate { get; set; }
}