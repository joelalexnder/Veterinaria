using MediatR;
using VetClinic.Application.UseCases.Pet.Commands;
using VetClinic.Application.UseCases.Pet.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Veterinaria.Controllers;
 
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PetController : ControllerBase
{
    private readonly IMediator _mediator;

    public PetController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Administrador,Recepcionista")]
    public async Task<IActionResult> Register([FromBody] RegisterPetCommand request)
    {
        await _mediator.Send(request);
        return Ok("Mascota registrada");
    }

    [HttpGet("owner/{ownerId}")]
    [Authorize(Roles = "Administrador,Veterinario,Recepcionista")]
    public async Task<IActionResult> GetByOwner([FromRoute] int ownerId)
        => Ok(await _mediator.Send(new GetPetsByOwnerQuery { OwnerId = ownerId }));

    [HttpGet("species/{species}")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetBySpecies([FromRoute] string species)
        => Ok(await _mediator.Send(new GetPetsBySpeciesQuery { Species = species }));

    [HttpGet("search")]
    [Authorize(Roles = "Administrador,Veterinario,Recepcionista")]
    public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? species, [FromQuery] int? ownerId)
        => Ok(await _mediator.Send(new SearchPetsQuery { Name = name, Species = species, OwnerId = ownerId }));

    [HttpGet("{petId}")]
    [Authorize(Roles = "Administrador,Veterinario,Recepcionista")]
    public async Task<IActionResult> GetDetail([FromRoute] int petId)
        => Ok(await _mediator.Send(new GetPetDetailQuery { PetId = petId }));

    [HttpGet("{petId}/medical-history")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetMedicalHistory([FromRoute] int petId)
        => Ok(await _mediator.Send(new GetPetMedicalHistoryQuery { PetId = petId }));
    
    [HttpGet("{petId}/health-summary-ai")]
    [Authorize(Roles = "Administrador,Veterinario")]
    public async Task<IActionResult> GetHealthSummaryAi([FromRoute] int petId)
    {
        var query = new GetPetHealthSummaryAiQuery { PetId = petId };
        var result = await _mediator.Send(query);
    
        return Ok(new { summary = result });
    }
}
