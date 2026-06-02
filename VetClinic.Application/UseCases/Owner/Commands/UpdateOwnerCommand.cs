using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Owner.Commands;

public class UpdateOwnerCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

public class UpdateOwnerCommandHandler : IRequestHandler<UpdateOwnerCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public UpdateOwnerCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
    {
        var owner = await _uow.Owners.GetByIdAsync(request.Id);
        if (owner is null) throw new Exception("Propietario no encontrado");

        owner.Phone = request.Phone ?? owner.Phone;
        owner.Email = request.Email ?? owner.Email;
        owner.Address = request.Address ?? owner.Address;

        _uow.Owners.Update(owner);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}