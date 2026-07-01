using MediatR;
using AutoMapper;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Commands;

public class AddVaccineCatalogCommand : IRequest<Unit>
{
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public int MinAgeDays { get; set; }
    public int BoosterIntervalDays { get; set; }
}

public class AddVaccineCatalogCommandHandler : IRequestHandler<AddVaccineCatalogCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AddVaccineCatalogCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddVaccineCatalogCommand request, CancellationToken cancellationToken)
    {
        var vaccine = _mapper.Map<Domain.Entities.Vaccine>(request);

        await _uow.Vaccines.AddAsync(vaccine);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}