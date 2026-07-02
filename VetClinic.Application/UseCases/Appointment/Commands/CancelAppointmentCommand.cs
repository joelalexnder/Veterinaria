using MediatR;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Appointment.Commands;

public class CancelAppointmentCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string CancellationReason { get; set; } = null!;
}

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public CancelAppointmentCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _uow.Appointments.GetByIdAsync(request.Id);
        if (appointment is null) throw new NotFoundException("Cita no encontrada");

        appointment.Status = "Cancelada";
        appointment.CancellationReason = request.CancellationReason;

        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}