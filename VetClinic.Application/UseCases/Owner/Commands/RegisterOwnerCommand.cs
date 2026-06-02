using MediatR;
using AutoMapper;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Owner.Commands;

public class RegisterOwnerCommand : IRequest<Unit>
{
    public string FullName { get; set; } = null!;
    public string Dni { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

public class RegisterOwnerCommandHandler : IRequestHandler<RegisterOwnerCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RegisterOwnerCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RegisterOwnerCommand request, CancellationToken cancellationToken)
    {
        var owner = _mapper.Map<Domain.Entities.Owner>(request);
        owner.CreatedAt = DateTime.UtcNow;

        await _uow.Owners.AddAsync(owner);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}