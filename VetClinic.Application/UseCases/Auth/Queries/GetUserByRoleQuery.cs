using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Auth;
using VetClinic.Domain.Ports.Repository;


namespace Application.UseCases.Auth.Queries;

public class GetUserByRoleQuery : IRequest<IEnumerable<UserDto>>
{
    public int RoleId { get; set; }
}

public class GetUserByRoleQueryHandler : IRequestHandler<GetUserByRoleQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetUserByRoleQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
    {
        var users = await _uow.Users.GetByRoleIdAsync(request.RoleId);
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
}