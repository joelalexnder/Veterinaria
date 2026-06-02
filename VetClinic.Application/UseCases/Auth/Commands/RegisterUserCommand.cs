using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Auth.Commands;

using MediatR;
using AutoMapper;

using Domain.Entities;
using Domain.DTOs.Auth;


public class RegisterUserCommand : IRequest<Unit>
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.IsActive = true;

        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}