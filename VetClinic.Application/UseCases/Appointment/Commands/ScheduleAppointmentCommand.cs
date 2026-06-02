using MediatR;
using AutoMapper;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Appointment.Commands;

public class ScheduleAppointmentCommand : IRequest<Unit>
{
    public int PetId { get; set; }
    public int SpecialistId { get; set; }
    public int ServiceAreaId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

public class ScheduleAppointmentCommandHandler : IRequestHandler<ScheduleAppointmentCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ScheduleAppointmentCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(ScheduleAppointmentCommand request, CancellationToken cancellationToken)
    {
        var hasConflict = await _uow.Appointments.HasConflictAsync(
            request.SpecialistId,
            request.ServiceAreaId,
            request.AppointmentDate,
            request.StartTime,
            request.EndTime);

        if (hasConflict)
            throw new Exception("Ya existe una cita en ese horario para el especialista o consultorio");

        var appointment = _mapper.Map<Domain.Entities.Appointment>(request);
        appointment.Status = "Pendiente";
        appointment.CreatedAt = DateTime.UtcNow;

        await _uow.Appointments.AddAsync(appointment);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}