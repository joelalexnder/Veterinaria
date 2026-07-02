using MediatR;
using AutoMapper;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Domain.Ports.Services;

namespace VetClinic.Application.UseCases.Auth.Commands;

public class RegisterUserCommand : IRequest<int>
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IUnitOfWork uow, IMapper mapper, IAuthService authService)
    {
        _uow = uow;
        _mapper = mapper;
        _authService = authService;
    }

    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _uow.Users.GetByEmailAsync(request.Email);
        if (existing is not null) throw new ConflictException("El email ya está registrado");

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.PasswordHash = _authService.Hash(request.Password);
        user.IsActive = true;
        user.CreatedAt = DateTime.Now;

        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();

        return user.Id;
    }
}