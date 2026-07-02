using MediatR;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Appointment.Commands;


public class RescheduleAppointmentCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public DateOnly NewDate { get; set; }
    public TimeOnly NewStartTime { get; set; }
    public TimeOnly NewEndTime { get; set; }
}

public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RescheduleAppointmentCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _uow.Appointments.GetByIdAsync(request.Id);
        if (appointment is null) throw new NotFoundException("Cita no encontrada");

        var hasConflict = await _uow.Appointments.HasConflictAsync(
            appointment.SpecialistId,
            appointment.ServiceAreaId,
            request.NewDate,
            request.NewStartTime,
            request.NewEndTime);

        if (hasConflict) throw new ConflictException("Ya existe una cita en ese nuevo horario");

        appointment.AppointmentDate = request.NewDate;
        appointment.StartTime = request.NewStartTime;
        appointment.EndTime = request.NewEndTime;
        appointment.Status = "Pendiente";

        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}