namespace VetClinic.Web.Models;

public class AppointmentDto
{
    public int Id { get; set; }
    public string PetName { get; set; } = "";
    public string SpecialistName { get; set; } = "";
    public string ServiceAreaName { get; set; } = "";
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Status { get; set; }
}

public class ScheduleAppointmentRequest
{
    public int PetId { get; set; }
    public int SpecialistId { get; set; }
    public int ServiceAreaId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

public class ServiceAreaDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class RegisterServiceAreaRequest
{
    public string Name { get; set; } = "";
}

public class SpecialistDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string? SpecialtyName { get; set; }
    public string ServiceAreaName { get; set; } = "";
}

public class RegisterSpecialistRequest
{
    public int UserId { get; set; }
    public int ServiceAreaId { get; set; }
    public string? SpecialtyName { get; set; }
}