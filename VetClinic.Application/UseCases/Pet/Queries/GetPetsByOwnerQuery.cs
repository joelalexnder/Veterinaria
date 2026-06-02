using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Pet;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Pet.Queries ;

public class GetPetsByOwnerQuery : IRequest<IEnumerable<PetDto>>
{
    public int OwnerId { get; set; }
}

public class GetPetsByOwnerQueryHandler : IRequestHandler<GetPetsByOwnerQuery, IEnumerable<PetDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPetsByOwnerQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PetDto>> Handle(GetPetsByOwnerQuery request, CancellationToken cancellationToken)
    {
        var pets = await _uow.Pets.GetByOwnerIdAsync(request.OwnerId);
        return _mapper.Map<IEnumerable<PetDto>>(pets);
    }
}