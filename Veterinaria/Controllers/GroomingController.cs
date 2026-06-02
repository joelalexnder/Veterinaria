using MediatR;
using VetClinic.Application.UseCases.Grooming.Commands;
using VetClinic.Application.UseCases.Grooming.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veterinaria.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroomingController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroomingController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Administrador,Estilista,Recepcionista")]
    public async Task<IActionResult> Schedule([FromBody] ScheduleGroomingCommand request)
    {
        await _mediator.Send(request);
        return Ok("Turno de grooming agendado");
    }

    [HttpPut("start/{id}")]
    [Authorize(Roles = "Administrador,Estilista")]
    public async Task<IActionResult> Start([FromRoute] int id)
    {
        await _mediator.Send(new StartGroomingCommand { Id = id });
        return NoContent();
    }

    [HttpPut("complete")]
    [Authorize(Roles = "Administrador,Estilista")]
    public async Task<IActionResult> Complete([FromBody] CompleteGroomingCommand request)
    {
        await _mediator.Send(request);
        return NoContent();
    }

    [HttpPost("package")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AddPackage([FromBody] AddGroomingPackageCommand request)
    {
        await _mediator.Send(request);
        return Ok("Paquete agregado");
    }

    [HttpGet("history/{petId}")]
    [Authorize(Roles = "Administrador,Estilista,Veterinario")]
    public async Task<IActionResult> GetHistory([FromRoute] int petId)
        => Ok(await _mediator.Send(new GetGroomingHistoryQuery { PetId = petId }));

    [HttpGet("by-date")]
    [Authorize(Roles = "Administrador,Estilista,Recepcionista")]
    public async Task<IActionResult> GetByDate([FromQuery] DateOnly date)
        => Ok(await _mediator.Send(new GetGroomingAppointmentsByDateQuery { Date = date }));

    [HttpGet("packages")]
    [Authorize(Roles = "Administrador,Estilista,Recepcionista")]
    public async Task<IActionResult> GetPackages()
        => Ok(await _mediator.Send(new GetGroomingPackagesQuery()));

    [HttpGet("stylist-availability")]
    [Authorize(Roles = "Administrador,Estilista,Recepcionista")]
    public async Task<IActionResult> GetStylistAvailability([FromQuery] int specialistId, [FromQuery] DateOnly date)
        => Ok(await _mediator.Send(new GetStylistAvailabilityQuery { SpecialistId = specialistId, Date = date }));
}