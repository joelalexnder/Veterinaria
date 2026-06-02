using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.VaccinationRecord;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Queries;

public class GetVaccinationHistoryQuery : IRequest<IEnumerable<VaccinationRecordDto>>
{
    public int PetId { get; set; }
}

public class GetVaccinationHistoryQueryHandler : IRequestHandler<GetVaccinationHistoryQuery, IEnumerable<VaccinationRecordDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetVaccinationHistoryQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccinationRecordDto>> Handle(GetVaccinationHistoryQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.VaccinationRecords.GetByPetIdAsync(request.PetId);
        return _mapper.Map<IEnumerable<VaccinationRecordDto>>(records);
    }
}