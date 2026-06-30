using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.ServiceArea.Commands;

public class RegisterServiceAreaCommand : IRequest<Unit>
{
    public string Name { get; set; } = null!;
}

public class RegisterServiceAreaCommandHandler : IRequestHandler<RegisterServiceAreaCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public RegisterServiceAreaCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(RegisterServiceAreaCommand request, CancellationToken cancellationToken)
    {
        var area = new Domain.Entities.ServiceArea { Name = request.Name };
        await _uow.ServiceAreas.AddAsync(area);
        await _uow.SaveChangesAsync();
        return Unit.Value;
    }
}