using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Specialist;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Specialist.Queries;

public class GetAllSpecialistsQuery : IRequest<IEnumerable<SpecialistDto>>
{
    public int? ServiceAreaId { get; set; } // opcional, para filtrar
}

public class GetAllSpecialistsQueryHandler : IRequestHandler<GetAllSpecialistsQuery, IEnumerable<SpecialistDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllSpecialistsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SpecialistDto>> Handle(GetAllSpecialistsQuery request, CancellationToken cancellationToken)
    {
        var specialists = request.ServiceAreaId.HasValue
            ? await _uow.Specialists.GetByServiceAreaAsync(request.ServiceAreaId.Value)
            : await _uow.Specialists.GetAllWithDetailsAsync();

        return _mapper.Map<IEnumerable<SpecialistDto>>(specialists);
    }
}