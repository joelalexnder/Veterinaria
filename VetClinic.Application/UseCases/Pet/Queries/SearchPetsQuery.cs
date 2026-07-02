using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Pet;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Pet.Queries;

public class SearchPetsQuery : IRequest<IEnumerable<PetDto>>
{
    public string? Name { get; set; }
    public string? Species { get; set; }
    public int? OwnerId { get; set; }
}

public class SearchPetsQueryHandler : IRequestHandler<SearchPetsQuery, IEnumerable<PetDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SearchPetsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PetDto>> Handle(SearchPetsQuery request, CancellationToken cancellationToken)
    {
        var pets = await _uow.Pets.SearchAsync(request.Name, request.Species, request.OwnerId);
        return _mapper.Map<IEnumerable<PetDto>>(pets);
    }
}