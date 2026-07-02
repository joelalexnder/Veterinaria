using MediatR;
using VetClinic.Application.UseCases.Specialist.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Application.UseCases.Specialist.Queries;

namespace Veterinaria.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SpecialistController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecialistController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Register([FromBody] RegisterSpecialistCommand request)
    {
        await _mediator.Send(request);
        return Ok("Especialista registrado");
    }
    // En SpecialistController.cs
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] int? serviceAreaId)
        => Ok(await _mediator.Send(new GetAllSpecialistsQuery { ServiceAreaId = serviceAreaId }));
}