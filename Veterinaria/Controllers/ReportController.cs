using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Application.UseCases.Reports.Queries;

namespace Veterinaria.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Veterinario")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Genera reporte Excel de citas. Si no se pasa fecha, incluye todas.
    /// </summary>
    [HttpGet("appointments")]
    public async Task<IActionResult> GetAppointmentsReport([FromQuery] DateOnly? date)
    {
        var file = await _mediator.Send(new GetAppointmentsReportQuery { Date = date });
        var fileName = date.HasValue
            ? $"Citas_{date.Value:yyyyMMdd}.xlsx"
            : $"Citas_Todas_{DateTime.Now:yyyyMMdd}.xlsx";

        return File(file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    /// <summary>
    /// Genera reporte Excel de vacunaciones vencidas/pendientes.
    /// </summary>
    [HttpGet("vaccinations")]
    public async Task<IActionResult> GetVaccinationsReport()
    {
        var file = await _mediator.Send(new GetVaccinationReportQuery());
        return File(file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Vacunaciones_Pendientes_{DateTime.Now:yyyyMMdd}.xlsx");
    }
}