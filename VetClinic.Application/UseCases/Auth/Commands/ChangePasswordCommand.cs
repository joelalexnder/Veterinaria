using MediatR;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Domain.Ports.Services;

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
    private readonly IAuthService _authService;

    public ChangePasswordCommandHandler(IUnitOfWork uow, IAuthService authService)
    {
        _uow = uow;
        _authService = authService;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId);
        if (user is null) throw new NotFoundException("Usuario no encontrado");

        if (!_authService.Verify(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Contraseña actual incorrecta");

        user.PasswordHash = _authService.Hash(request.NewPassword);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}