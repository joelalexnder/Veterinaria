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
        // CAMBIO: antes era GetOverdueAsync() -> solo traía vencidas.
        // Ahora traemos el historial completo con Pet, Vaccine y Veterinarian cargados.
        var records = await _uow.VaccinationRecords.GetAllWithDetailsAsync();

        var today = DateOnly.FromDateTime(DateTime.Today);

        var rows = records.Select(v =>
        {
            string status = v.NextBoosterDate switch
            {
                null => "Sin refuerzo",
                var d when d < today => "Vencida",
                var d when d <= today.AddDays(30) => "Próxima",
                _ => "Al día"
            };

            int daysOverdue = v.NextBoosterDate.HasValue && v.NextBoosterDate < today
                ? today.DayNumber - v.NextBoosterDate.Value.DayNumber
                : 0;

            return new VaccinationReportRow
            {
                PetName          = v.Pet?.Name ?? "Sin nombre",
                VaccineName      = v.Vaccine?.Name ?? "Sin vacuna",
                VeterinarianName = v.Veterinarian?.FullName ?? "Sin veterinario",
                BatchNumber      = v.BatchNumber,
                ApplicationDate  = v.ApplicationDate,
                NextBoosterDate  = v.NextBoosterDate,
                DaysOverdue      = daysOverdue,
                Status           = status
            };
        })
        // Ordena: vencidas primero, luego próximas, luego al día
        .OrderBy(r => r.Status switch
        {
            "Vencida" => 0,
            "Próxima" => 1,
            "Al día" => 2,
            _ => 3
        });

        return _excelService.GenerateVaccinationsReport(rows);
    }
}