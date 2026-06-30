using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Specialist.Commands;

public class RegisterSpecialistCommand : IRequest<Unit>
{
    public int UserId { get; set; }
    public int ServiceAreaId { get; set; }
    public string? SpecialtyName { get; set; }
}

public class RegisterSpecialistCommandHandler : IRequestHandler<RegisterSpecialistCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RegisterSpecialistCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(RegisterSpecialistCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId);
        if (user is null) throw new Exception("Usuario no encontrado");

        var specialist = new Domain.Entities.Specialist
        {
            UserId = request.UserId,
            ServiceAreaId = request.ServiceAreaId,
            SpecialtyName = request.SpecialtyName
        };

        await _uow.Specialists.AddAsync(specialist);
        await _uow.SaveChangesAsync();
        return Unit.Value;
    }
}