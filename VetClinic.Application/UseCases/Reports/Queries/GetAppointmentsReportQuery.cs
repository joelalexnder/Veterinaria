using AutoMapper;
using MediatR;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Domain.Ports.Services;

namespace VetClinic.Application.UseCases.Reports.Queries;

public class GetAppointmentsReportQuery : IRequest<byte[]>
{
    public DateOnly? Date { get; set; } // null = todas las citas
}

public class GetAppointmentsReportQueryHandler 
    : IRequestHandler<GetAppointmentsReportQuery, byte[]>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IExcelReportService _excelService;

    public GetAppointmentsReportQueryHandler(
        IUnitOfWork uow, IMapper mapper, IExcelReportService excelService)
    {
        _uow = uow;
        _mapper = mapper;
        _excelService = excelService;
    }

    public async Task<byte[]> Handle(
        GetAppointmentsReportQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Appointment> appointments;

        if (request.Date.HasValue)
            appointments = await _uow.Appointments.GetByDateAsync(request.Date.Value);
        else
            appointments = await _uow.Appointments.GetAllAsync();

        var rows = appointments.Select(a => new AppointmentReportRow
        {
            PetName         = a.Pet?.Name ?? "Sin nombre",
            SpecialistName = a.Specialist?.User?.FullName ?? "Sin especialista",
            ServiceAreaName = a.ServiceArea?.Name ?? "Sin área",
            AppointmentDate = a.AppointmentDate,
            StartTime       = a.StartTime,
            EndTime         = a.EndTime,
            Status          = a.Status
        });

        return _excelService.GenerateAppointmentsReport(rows);
    }
}