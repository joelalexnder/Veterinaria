using MediatR;
using AutoMapper;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Grooming.Commands;


public class ScheduleGroomingCommand : IRequest<Unit>
{
    public int AppointmentId { get; set; }
    public int? GroomingPackageId { get; set; }
}

public class ScheduleGroomingCommandHandler : IRequestHandler<ScheduleGroomingCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ScheduleGroomingCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(ScheduleGroomingCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _uow.Appointments.GetByIdAsync(request.AppointmentId);
        if (appointment is null) throw new NotFoundException("Cita no encontrada");

        var grooming = _mapper.Map<GroomingAppointment>(request);
        grooming.ServiceStatus = "Pendiente";

        await _uow.GroomingAppointments.AddAsync(grooming);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}