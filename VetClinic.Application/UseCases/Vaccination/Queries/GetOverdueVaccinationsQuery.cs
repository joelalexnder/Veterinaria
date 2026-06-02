using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.VaccinationRecord;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Queries;

public class GetOverdueVaccinationsQuery : IRequest<IEnumerable<VaccinationRecordDto>>
{
}

public class GetOverdueVaccinationsQueryHandler : IRequestHandler<GetOverdueVaccinationsQuery, IEnumerable<VaccinationRecordDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetOverdueVaccinationsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccinationRecordDto>> Handle(GetOverdueVaccinationsQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.VaccinationRecords.GetOverdueAsync();
        return _mapper.Map<IEnumerable<VaccinationRecordDto>>(records);
    }
}