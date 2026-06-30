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
        var pet = await _uow.Pets.GetByIdAsync(request.PetId);
        
        if (pet is null) throw new Exception("Mascota no encontrada en el sistema");
        
        var petData = $@"
        Nombre: {pet.Name}, 
        Especie: {pet.Species}, 
        Raza: {pet.Breed ?? "No especificada"}, 
        Peso: {pet.Weight?.ToString() ?? "No registrado"} kg, 
        Sexo: {pet.Sex ?? "No especificado"},
        Estado de salud general: {pet.GeneralHealthStatus ?? "Sin observaciones previas"}.";

        var aiRecommendation = await _aiService.GenerateHealthSummaryAsync(petData);

        return aiRecommendation;
    }
}

