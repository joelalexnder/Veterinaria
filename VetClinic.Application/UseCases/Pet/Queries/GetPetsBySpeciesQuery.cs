using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Pet;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Pet.Queries ;

public class GetPetsBySpeciesQuery : IRequest<IEnumerable<PetDto>>
{
    public string Species { get; set; } = null!;
}

public class GetPetsBySpeciesQueryHandler : IRequestHandler<GetPetsBySpeciesQuery, IEnumerable<PetDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPetsBySpeciesQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PetDto>> Handle(GetPetsBySpeciesQuery request, CancellationToken cancellationToken)
    {
        var pets = await _uow.Pets.GetBySpeciesAsync(request.Species);
        return _mapper.Map<IEnumerable<PetDto>>(pets);
    }
}