using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.VaccinationRecord;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Queries;
public class GetVaccinationScheduleQuery : IRequest<IEnumerable<VaccinationRecordDto>>
{
    public int PetId { get; set; }
}

public class GetVaccinationScheduleQueryHandler : IRequestHandler<GetVaccinationScheduleQuery, IEnumerable<VaccinationRecordDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetVaccinationScheduleQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccinationRecordDto>> Handle(GetVaccinationScheduleQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.VaccinationRecords.GetByPetIdAsync(request.PetId);
        return _mapper.Map<IEnumerable<VaccinationRecordDto>>(records);
    }
}