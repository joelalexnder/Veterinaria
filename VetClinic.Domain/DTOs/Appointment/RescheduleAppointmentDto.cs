namespace VetClinic.Domain.DTOs.Appointment;

public class RescheduleAppointmentDto
{
    public int Id { get; set; }
    public DateOnly NewDate { get; set; }
    public TimeOnly NewStartTime { get; set; }
    public TimeOnly NewEndTime { get; set; }
}