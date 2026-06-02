namespace VetClinic.Domain.DTOs.Appointment;

public class AppointmentDto
{
    public int Id { get; set; }
    public string PetName { get; set; } = null!;
    public string SpecialistName { get; set; } = null!;
    public string ServiceAreaName { get; set; } = null!;
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Status { get; set; }
}