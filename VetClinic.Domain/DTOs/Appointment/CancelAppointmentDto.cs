namespace VetClinic.Domain.DTOs.Appointment;

public class CancelAppointmentDto
{
    public int Id { get; set; }
    public string CancellationReason { get; set; } = null!;
}