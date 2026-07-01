using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.VaccinationRecord;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Queries;

public class GetVaccinationsByVeterinarianQuery : IRequest<IEnumerable<VaccinationRecordDto>>
{
    public int VeterinarianId { get; set; }
}

public class GetVaccinationsByVeterinarianQueryHandler
    : IRequestHandler<GetVaccinationsByVeterinarianQuery, IEnumerable<VaccinationRecordDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetVaccinationsByVeterinarianQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VaccinationRecordDto>> Handle(
        GetVaccinationsByVeterinarianQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.VaccinationRecords.GetByVeterinarianIdAsync(request.VeterinarianId);
        return _mapper.Map<IEnumerable<VaccinationRecordDto>>(records);
    }
}