using MediatR;
using VetClinic.Application.UseCases.Owner.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veterinaria.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OwnerController : ControllerBase
{
    private readonly IMediator _mediator;

    public OwnerController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Administrador,Recepcionista")]
    public async Task<IActionResult> Register([FromBody] RegisterOwnerCommand request)
    {
        await _mediator.Send(request);
        return Ok("Propietario registrado");
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador,Recepcionista")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateOwnerCommand request)
    {
        request.Id = id;
        await _mediator.Send(request);
        return NoContent();
    }
}