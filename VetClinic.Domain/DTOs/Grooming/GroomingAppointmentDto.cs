namespace VetClinic.Domain.DTOs.Grooming;

public class GroomingAppointmentDto
{
    public int Id { get; set; }
    public string PetName { get; set; } = null!;
    public string? PackageName { get; set; }
    public string? StylistNotes { get; set; }
    public decimal? FinalPrice { get; set; }
    public string? ServiceStatus { get; set; }
    public DateOnly AppointmentDate { get; set; }
}