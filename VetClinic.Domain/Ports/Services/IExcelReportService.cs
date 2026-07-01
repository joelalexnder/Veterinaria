namespace VetClinic.Domain.Ports.Services;

public interface IExcelReportService
{
    byte[] GenerateAppointmentsReport(IEnumerable<AppointmentReportRow> data);
    byte[] GenerateVaccinationsReport(IEnumerable<VaccinationReportRow> data);
}

public class AppointmentReportRow
{
    public string PetName { get; set; } = null!;
    public string SpecialistName { get; set; } = null!;
    public string ServiceAreaName { get; set; } = null!;
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Status { get; set; }
}

public class VaccinationReportRow
{
    public string PetName { get; set; } = null!;
    public string VaccineName { get; set; } = null!;
    public string VeterinarianName { get; set; } = null!;
    public string? BatchNumber { get; set; }
    public DateOnly? ApplicationDate { get; set; }
    public DateOnly? NextBoosterDate { get; set; }
    public int DaysOverdue { get; set; }
    public string Status { get; set; } = null!;
}