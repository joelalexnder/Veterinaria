using MediatR;
using VetClinic.Application.UseCases.ServiceArea.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veterinaria.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceAreaController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceAreaController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Register([FromBody] RegisterServiceAreaCommand request)
    {
        await _mediator.Send(request);
        return Ok("Área de servicio registrada");
    }
}