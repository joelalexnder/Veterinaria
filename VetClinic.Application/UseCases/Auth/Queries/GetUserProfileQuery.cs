using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Auth;
using VetClinic.Domain.Ports.Repository;

namespace Application.UseCases.Auth.Queries;

public class GetUserProfileQuery : IRequest<UserDto>
{
    public int UserId { get; set; }
}

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetUserProfileQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetUserWithRoleByIdAsync(request.UserId);
        if (user is null) throw new Exception("Usuario no encontrado");
        return _mapper.Map<UserDto>(user);
    }
}