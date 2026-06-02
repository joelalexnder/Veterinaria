using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Grooming.Commands;

public class StartGroomingCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class StartGroomingCommandHandler : IRequestHandler<StartGroomingCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public StartGroomingCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(StartGroomingCommand request, CancellationToken cancellationToken)
    {
        var grooming = await _uow.GroomingAppointments.GetByIdAsync(request.Id);
        if (grooming is null) throw new Exception("Servicio de grooming no encontrado");

        grooming.ServiceStatus = "En Proceso";

        _uow.GroomingAppointments.Update(grooming);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}