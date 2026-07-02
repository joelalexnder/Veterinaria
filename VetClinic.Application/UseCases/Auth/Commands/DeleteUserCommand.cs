using MediatR;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Auth.Commands;

public class DeleteUserCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public DeleteUserCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetByIdAsync(request.Id);
        if (user is null) throw new NotFoundException("Usuario no encontrado");

        // Si es especialista (Veterinario/Estilista), verificar que no tenga citas asociadas
        var specialist = await _uow.Specialists.GetByUserIdAsync(request.Id);
        if (specialist is not null)
        {
            throw new ConflictException(
                $"No se puede eliminar a '{user.FullName}' porque está registrado como especialista. " +
                "Primero debes reasignar o eliminar sus citas y su registro de especialista.");
        }

        // Si EF Core lanza un error de FK al guardar (por otras relaciones no cubiertas arriba,
        // como MedicalRecords o AuditLogs), dejamos que suba sin atraparla aquí.
        // El ErrorHandlingMiddleware en la capa de API se encarga de traducirla a 409.
        _uow.Users.Delete(user);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}