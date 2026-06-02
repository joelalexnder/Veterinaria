using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace Application.UseCases.Auth.Commands;

public class AssignRoleCommand : IRequest<Unit>
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public AssignRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId);

        if (user is null) throw new Exception("Usuario no encontrado");

        user.RoleId = request.RoleId;
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}