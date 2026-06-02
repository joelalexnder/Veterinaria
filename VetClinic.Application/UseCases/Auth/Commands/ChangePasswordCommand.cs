using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace Application.UseCases.Auth.Commands;

public class ChangePasswordCommand : IRequest<Unit>
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public ChangePasswordCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId);

        if (user is null) throw new Exception("Usuario no encontrado");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Contraseña actual incorrecta");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}