using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Grooming.Commands;

public class CompleteGroomingCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string? StylistNotes { get; set; }
    public decimal FinalPrice { get; set; }
}

public class CompleteGroomingCommandHandler : IRequestHandler<CompleteGroomingCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public CompleteGroomingCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(CompleteGroomingCommand request, CancellationToken cancellationToken)
    {
        var grooming = await _uow.GroomingAppointments.GetByIdAsync(request.Id);
        if (grooming is null) throw new Exception("Servicio de grooming no encontrado");

        grooming.ServiceStatus = "Listo para Recoger";
        grooming.StylistNotes = request.StylistNotes;
        grooming.FinalPrice = request.FinalPrice;

        _uow.GroomingAppointments.Update(grooming);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}