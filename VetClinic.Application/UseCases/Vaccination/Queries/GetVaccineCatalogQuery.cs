using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Vaccine;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Queries;

public class GetVaccineCatalogQuery : IRequest<IEnumerable<VaccineDto>>
{
}

public class GetVaccineCatalogQueryHandler : IRequestHandler<GetVaccineCatalogQuery, IEnumerable<VaccineDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetVaccineCatalogQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccineDto>> Handle(GetVaccineCatalogQuery request, CancellationToken cancellationToken)
    {
        var vaccines = await _uow.Vaccines.GetAllAsync();
        return _mapper.Map<IEnumerable<VaccineDto>>(vaccines);
    }
}