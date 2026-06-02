using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Appointment;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Appointment.Queries;

public class GetAppointmentsByDateQuery : IRequest<IEnumerable<AppointmentDto>>
{
    public DateOnly Date { get; set; }
}

public class GetAppointmentsByDateQueryHandler : IRequestHandler<GetAppointmentsByDateQuery, IEnumerable<AppointmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAppointmentsByDateQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> Handle(GetAppointmentsByDateQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _uow.Appointments.GetByDateAsync(request.Date);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }
}