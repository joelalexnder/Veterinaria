using MediatR;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Domain.Ports.Services;

namespace VetClinic.Application.UseCases.Reports.Queries;

public class GetVaccinationReportQuery : IRequest<byte[]> { }

public class GetVaccinationReportQueryHandler
    : IRequestHandler<GetVaccinationReportQuery, byte[]>
{
    private readonly IUnitOfWork _uow;
    private readonly IExcelReportService _excelService;

    public GetVaccinationReportQueryHandler(
        IUnitOfWork uow, IExcelReportService excelService)
    {
        _uow = uow;
        _excelService = excelService;
    }

    public async Task<byte[]> Handle(
        GetVaccinationReportQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.VaccinationRecords.GetOverdueAsync();

        var rows = records.Select(v => new VaccinationReportRow
        {
            PetName          = v.Pet?.Name ?? "Sin nombre",
            VaccineName      = v.Vaccine?.Name ?? "Sin vacuna",
            VeterinarianName = v.Veterinarian?.FullName ?? "Sin veterinario",
            BatchNumber      = v.BatchNumber,
            ApplicationDate  = v.ApplicationDate,
            NextBoosterDate  = v.NextBoosterDate,
            DaysOverdue      = v.NextBoosterDate.HasValue
                ? Math.Max(0, DateOnly.FromDateTime(DateTime.Today)
                    .DayNumber - v.NextBoosterDate.Value.DayNumber)
                : 0
        });

        return _excelService.GenerateVaccinationsReport(rows);
    }
}

