using MediatR;
using VetClinic.Domain.DTOs.VaccinationRecord;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Vaccination.Queries;

public class GetAllVaccinationsWithStatusQuery : IRequest<IEnumerable<VaccinationRecordDto>> { }

public class GetAllVaccinationsWithStatusQueryHandler
    : IRequestHandler<GetAllVaccinationsWithStatusQuery, IEnumerable<VaccinationRecordDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllVaccinationsWithStatusQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<VaccinationRecordDto>> Handle(
        GetAllVaccinationsWithStatusQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.VaccinationRecords.GetAllWithDetailsAsync();
        var today = DateOnly.FromDateTime(DateTime.Today);

        return records.Select(v =>
        {
            string status = v.NextBoosterDate switch
            {
                null => "Sin refuerzo",
                var d when d < today => "Vencida",
                var d when d <= today.AddDays(30) => "Próxima",
                _ => "Al día"
            };

            int daysOverdue = v.NextBoosterDate.HasValue && v.NextBoosterDate < today
                ? today.DayNumber - v.NextBoosterDate.Value.DayNumber
                : 0;

            return new VaccinationRecordDto
            {
                Id = v.Id,
                PetName = v.Pet?.Name ?? "Sin nombre",
                VaccineName = v.Vaccine?.Name ?? "Sin vacuna",
                VeterinarianName = v.Veterinarian?.FullName ?? "Sin veterinario",
                BatchNumber = v.BatchNumber,
                ApplicationDate = v.ApplicationDate,
                NextBoosterDate = v.NextBoosterDate,
                DaysOverdue = daysOverdue,
                Status = status
            };
        })
        .OrderBy(r => r.Status switch
        {
            "Vencida" => 0,
            "Próxima" => 1,
            "Al día" => 2,
            _ => 3
        });
    }
}