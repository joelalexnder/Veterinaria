
using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Appointment;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Appointment.Queries;

public class GetAppointmentsByPetQuery : IRequest<IEnumerable<AppointmentDto>>
{
    public int PetId { get; set; }
}

public class GetAppointmentsByPetQueryHandler : IRequestHandler<GetAppointmentsByPetQuery, IEnumerable<AppointmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAppointmentsByPetQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> Handle(GetAppointmentsByPetQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _uow.Appointments.GetByPetIdAsync(request.PetId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }
}