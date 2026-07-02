using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.ServiceArea;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.ServiceArea.Queries;

public class GetAllServiceAreasQuery : IRequest<IEnumerable<ServiceAreaDto>> { }

public class GetAllServiceAreasQueryHandler : IRequestHandler<GetAllServiceAreasQuery, IEnumerable<ServiceAreaDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllServiceAreasQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceAreaDto>> Handle(GetAllServiceAreasQuery request, CancellationToken cancellationToken)
    {
        var areas = await _uow.ServiceAreas.GetAllAsync();
        return _mapper.Map<IEnumerable<ServiceAreaDto>>(areas);
    }
}