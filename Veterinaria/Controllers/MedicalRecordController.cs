using MediatR;
using VetClinic.Application.UseCases.MedicalRecord.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veterinaria.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicalRecordController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicalRecordController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> Add([FromBody] AddMedicalRecordCommand request)
    {
        await _mediator.Send(request);
        return Ok("Registro médico añadido");
    }
}