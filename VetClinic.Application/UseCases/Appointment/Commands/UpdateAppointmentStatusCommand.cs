using MediatR;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Appointment.Commands;

public class UpdateAppointmentStatusCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Status { get; set; } = null!;
}

public class UpdateAppointmentStatusCommandHandler : IRequestHandler<UpdateAppointmentStatusCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public UpdateAppointmentStatusCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(UpdateAppointmentStatusCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _uow.Appointments.GetByIdAsync(request.Id);
        if (appointment is null) throw new NotFoundException("Cita no encontrada");

        appointment.Status = request.Status;

        _uow.Appointments.Update(appointment);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}