using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Owner.Commands;

public class DeleteOwnerCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeleteOwnerCommandHandler : IRequestHandler<DeleteOwnerCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public DeleteOwnerCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
    {
        var owner = await _uow.Owners.GetWithPetsAsync(request.Id);
        if (owner is null) throw new Exception("Propietario no encontrado");

        if (owner.Pets.Any())
            throw new Exception("No se puede eliminar: este propietario tiene mascotas registradas");

        _uow.Owners.Delete(owner);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}