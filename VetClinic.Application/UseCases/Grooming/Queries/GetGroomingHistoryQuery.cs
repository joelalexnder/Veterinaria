using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Grooming;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Grooming.Queries;

public class GetGroomingHistoryQuery : IRequest<IEnumerable<GroomingAppointmentDto>>
{
    public int PetId { get; set; }
}

public class GetGroomingHistoryQueryHandler : IRequestHandler<GetGroomingHistoryQuery, IEnumerable<GroomingAppointmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetGroomingHistoryQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroomingAppointmentDto>> Handle(GetGroomingHistoryQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.GroomingAppointments.GetByPetIdAsync(request.PetId);
        return _mapper.Map<IEnumerable<GroomingAppointmentDto>>(records);
    }
}