using MediatR;
using Application.UseCases.Auth.Commands;
using Application.UseCases.Auth.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Application.UseCases.Auth.Commands;
using VetClinic.Application.UseCases.Auth.Queries;

namespace Veterinaria.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("register")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        await _mediator.Send(request);
        return Ok("Usuario registrado");
    }

    [HttpPut("update-role")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleCommand request)
    {
        await _mediator.Send(request);
        return NoContent();
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
    {
        await _mediator.Send(request);
        return Ok(new
        {
            message = "Contraseña actualizada correctamente."
        });
    }

    [HttpGet("users")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetUsersByRole([FromQuery] int roleId)
        => Ok(await _mediator.Send(new GetUserByRoleQuery { RoleId = roleId }));

    [HttpGet("profile/{userId}")]
    [Authorize]
    public async Task<IActionResult> GetProfile([FromRoute] int userId)
        => Ok(await _mediator.Send(new GetUserProfileQuery { UserId = userId }));

    [HttpGet("audit-log")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetAuditLog([FromQuery] int? userId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        => Ok(await _mediator.Send(new GetAuditLogQuery { UserId = userId, From = from, To = to }));
}