using MediatR;
using AutoMapper;
using VetClinic.Domain.Entities;
using VetClinic.Domain.Ports.Repository;


namespace VetClinic.Application.UseCases.Vaccination.Commands;

public class RegisterVaccinationCommand : IRequest<Unit>
{
    public int PetId { get; set; }
    public int VaccineId { get; set; }
    public int VeterinarianId { get; set; }
    public string? BatchNumber { get; set; }
    public DateOnly ApplicationDate { get; set; }
    public DateOnly NextBoosterDate { get; set; }
}

public class RegisterVaccinationCommandHandler : IRequestHandler<RegisterVaccinationCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RegisterVaccinationCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RegisterVaccinationCommand request, CancellationToken cancellationToken)
    {
        var record = _mapper.Map<VaccinationRecord>(request);

        await _uow.VaccinationRecords.AddAsync(record);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}