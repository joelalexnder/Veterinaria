using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Grooming;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Grooming.Queries;

public class GetGroomingAppointmentsByDateQuery : IRequest<IEnumerable<GroomingAppointmentDto>>
{
    public DateOnly Date { get; set; }
}

public class GetGroomingAppointmentsByDateQueryHandler : IRequestHandler<GetGroomingAppointmentsByDateQuery, IEnumerable<GroomingAppointmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetGroomingAppointmentsByDateQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroomingAppointmentDto>> Handle(GetGroomingAppointmentsByDateQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.GroomingAppointments.GetByDateAsync(request.Date);
        return _mapper.Map<IEnumerable<GroomingAppointmentDto>>(records);
    }
}