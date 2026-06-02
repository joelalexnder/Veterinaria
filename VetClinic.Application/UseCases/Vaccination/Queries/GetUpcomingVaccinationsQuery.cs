using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.VaccinationRecord;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Queries;

public class GetUpcomingVaccinationsQuery : IRequest<IEnumerable<VaccinationRecordDto>>
{
    public int Days { get; set; } = 30;
}

public class GetUpcomingVaccinationsQueryHandler : IRequestHandler<GetUpcomingVaccinationsQuery, IEnumerable<VaccinationRecordDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetUpcomingVaccinationsQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccinationRecordDto>> Handle(GetUpcomingVaccinationsQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.VaccinationRecords.GetUpcomingAsync(request.Days);
        return _mapper.Map<IEnumerable<VaccinationRecordDto>>(records);
    }
}