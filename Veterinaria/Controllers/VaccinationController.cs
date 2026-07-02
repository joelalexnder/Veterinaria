using MediatR;
using VetClinic.Application.UseCases.Vaccination.Commands;
using VetClinic.Application.UseCases.Vaccination.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veterinaria.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VaccinationController : ControllerBase
{
    private readonly IMediator _mediator;

    public VaccinationController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> Register([FromBody] RegisterVaccinationCommand request)
    {
        await _mediator.Send(request);
        return Ok("Vacunación registrada");
    }

    [HttpPost("catalog")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AddToCatalog([FromBody] AddVaccineCatalogCommand request)
    {
        await _mediator.Send(request);
        return Ok("Vacuna agregada al catálogo");
    }

    [HttpPut("schedule")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateVaccineScheduleCommand request)
    {
        await _mediator.Send(request);
        return NoContent();
    }

    [HttpGet("schedule/{petId}")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetSchedule([FromRoute] int petId)
        => Ok(await _mediator.Send(new GetVaccinationScheduleQuery { PetId = petId }));

    [HttpGet("upcoming")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetUpcoming([FromQuery] int days = 30)
        => Ok(await _mediator.Send(new GetUpcomingVaccinationsQuery { Days = days }));

    [HttpGet("overdue")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetOverdue()
        => Ok(await _mediator.Send(new GetOverdueVaccinationsQuery()));

    [HttpGet("history/{petId}")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetHistory([FromRoute] int petId)
        => Ok(await _mediator.Send(new GetVaccinationHistoryQuery { PetId = petId }));
    
    [HttpGet("catalog")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetCatalog()
        => Ok(await _mediator.Send(new GetVaccineCatalogQuery()));
    
    [HttpGet("all")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAllVaccinationsWithStatusQuery()));
}