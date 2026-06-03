using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Auth;
using VetClinic.Domain.Ports;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Domain.Ports.Services;

namespace VetClinic.Application.UseCases.Auth.Commands;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public LoginCommandHandler(IUnitOfWork uow, IMapper mapper, IAuthService authService)
    {
        _uow = uow;
        _mapper = mapper;
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetByEmailAsync(request.Email);

        if (user is null || !_authService.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales incorrectas");

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = _authService.GenerateToken(user);
        response.ExpiresAt = DateTime.UtcNow.AddHours(8);

        return response;
    }
}