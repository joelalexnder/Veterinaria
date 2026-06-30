using MediatR;
using AutoMapper;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Pet.Commands;

public class RegisterPetCommand : IRequest<Unit>
{
    public int OwnerId { get; set; }
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public string? Breed { get; set; }
    public DateOnly? BirthDate { get; set; }
    public decimal? Weight { get; set; }
    public string? Sex { get; set; }
    public string? Color { get; set; }
    public string? GeneralHealthStatus { get; set; }
}

public class RegisterPetCommandHandler : IRequestHandler<RegisterPetCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RegisterPetCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RegisterPetCommand request, CancellationToken cancellationToken)
    {
        var pet = _mapper.Map<Domain.Entities.Pet>(request);
        pet.CreatedAt = DateTime.Now;

        await _uow.Pets.AddAsync(pet);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}