namespace VetClinic.Domain.DTOs.Appointment;

public class ScheduleAppointmentDto
{
    public int PetId { get; set; }
    public int SpecialistId { get; set; }
    public int ServiceAreaId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}