using System.Text;
using MediatR;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Domain.Ports.Services;

namespace VetClinic.Application.UseCases.Pet.Queries;

public class GetPetHealthSummaryAiQuery : IRequest<string>
{
    public int PetId { get; set; }
}

public class GetPetHealthSummaryAiQueryHandler : IRequestHandler<GetPetHealthSummaryAiQuery, string>
{
    private readonly IUnitOfWork _uow;
    private readonly IAiRecommendationService _aiService;

    public GetPetHealthSummaryAiQueryHandler(IUnitOfWork uow, IAiRecommendationService aiService)
    {
        _uow = uow;
        _aiService = aiService;
    }

    public async Task<string> Handle(GetPetHealthSummaryAiQuery request, CancellationToken cancellationToken)
    {
        var pet = await _uow.Pets.GetWithMedicalHistoryAsync(request.PetId);
        if (pet is null) throw new Exception("Mascota no encontrada");

        var sb = new StringBuilder();
        sb.AppendLine($"Mascota: {pet.Name}, Especie: {pet.Species}, Raza: {pet.Breed}, Sexo: {pet.Sex}");
        sb.AppendLine($"Peso actual: {pet.Weight}, Estado general: {pet.GeneralHealthStatus}");
        sb.AppendLine("Historial médico:");

        if (pet.MedicalRecords is not null && pet.MedicalRecords.Any())
        {
            foreach (var record in pet.MedicalRecords.OrderByDescending(m => m.ConsultDate))
            {
                sb.AppendLine($"- {record.ConsultDate:dd/MM/yyyy} | Motivo: {record.Reason} | " +
                              $"Diagnóstico: {record.Diagnosis} | Tratamiento: {record.Treatment} | " +
                              $"Peso registrado: {record.RegisteredWeight}");
            }
        }
        else
        {
            sb.AppendLine("Sin registros médicos previos.");
        }

        return await _aiService.GenerateHealthSummaryAsync(sb.ToString());
    }
}