using MediatR;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Commands;

public class UpdateVaccineScheduleCommand : IRequest<Unit>
{
    public int VaccinationRecordId { get; set; }
    public DateOnly NextBoosterDate { get; set; }
}

public class UpdateVaccineScheduleCommandHandler : IRequestHandler<UpdateVaccineScheduleCommand, Unit>
{
    private readonly IUnitOfWork _uow;

    public UpdateVaccineScheduleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Unit> Handle(UpdateVaccineScheduleCommand request, CancellationToken cancellationToken)
    {
        var record = await _uow.VaccinationRecords.GetByIdAsync(request.VaccinationRecordId);
        if (record is null) throw new Exception("Registro de vacunación no encontrado");

        record.NextBoosterDate = request.NextBoosterDate;

        _uow.VaccinationRecords.Update(record);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}