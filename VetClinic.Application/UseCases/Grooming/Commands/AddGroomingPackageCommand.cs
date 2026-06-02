using MediatR;
using AutoMapper;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Grooming.Commands;

public class AddGroomingPackageCommand : IRequest<Unit>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal SpecialPrice { get; set; }
}

public class AddGroomingPackageCommandHandler : IRequestHandler<AddGroomingPackageCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AddGroomingPackageCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddGroomingPackageCommand request, CancellationToken cancellationToken)
    {
        var package = _mapper.Map<GroomingPackage>(request);

        await _uow.GroomingPackages.AddAsync(package);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}