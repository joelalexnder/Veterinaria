using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Pet;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Owner.Queries;

public class GetAllOwnersQuery : IRequest<IEnumerable<OwnerDto>> { }

public class GetAllOwnersQueryHandler : IRequestHandler<GetAllOwnersQuery, IEnumerable<OwnerDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllOwnersQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OwnerDto>> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
    {
        var owners = await _uow.Owners.GetAllWithPetsAsync();
        return _mapper.Map<IEnumerable<OwnerDto>>(owners);
    }
}