using MediatR;
using VetClinic.Application.UseCases.Appointment.Commands;
using VetClinic.Application.UseCases.Appointment.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veterinaria.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Administrador,Recepcionista")]
    public async Task<IActionResult> Schedule([FromBody] ScheduleAppointmentCommand request)
    {
        await _mediator.Send(request);
        return Ok("Cita agendada");
    }

    [HttpPut("cancel")]
    [Authorize(Roles = "Administrador,Recepcionista")]
    public async Task<IActionResult> Cancel([FromBody] CancelAppointmentCommand request)
    {
        await _mediator.Send(request);
        return NoContent();
    }

    [HttpPut("reschedule")]
    [Authorize(Roles = "Administrador,Recepcionista")]
    public async Task<IActionResult> Reschedule([FromBody] RescheduleAppointmentCommand request)
    {
        await _mediator.Send(request);
        return NoContent();
    }

    [HttpPut("status")]
    [Authorize(Roles = "Administrador,Veterinario,Recepcionista")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateAppointmentStatusCommand request)
    {
        await _mediator.Send(request);
        return NoContent();
    }

    [HttpGet("available-slots")]
    [Authorize(Roles = "Administrador,Recepcionista")]
    public async Task<IActionResult> GetAvailableSlots([FromQuery] int specialistId, [FromQuery] DateOnly date)
        => Ok(await _mediator.Send(new GetAvailableSlotsQuery { SpecialistId = specialistId, Date = date }));

    [HttpGet("by-date")]
    [Authorize(Roles = "Administrador,Veterinario,Recepcionista")]
    public async Task<IActionResult> GetByDate([FromQuery] DateOnly date)
        => Ok(await _mediator.Send(new GetAppointmentsByDateQuery { Date = date }));

    [HttpGet("by-pet/{petId}")]
    [Authorize(Roles = "Administrador,Veterinario,Recepcionista")]
    public async Task<IActionResult> GetByPet([FromRoute] int petId)
        => Ok(await _mediator.Send(new GetAppointmentsByPetQuery { PetId = petId }));

    [HttpGet("specialist-schedule")]
    [Authorize(Roles = "Administrador,Veterinario,Recepcionista")]
    public async Task<IActionResult> GetSpecialistSchedule([FromQuery] int specialistId, [FromQuery] DateOnly date)
        => Ok(await _mediator.Send(new GetSpecialistScheduleQuery { SpecialistId = specialistId, Date = date }));
}