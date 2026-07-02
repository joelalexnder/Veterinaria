using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Pet;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Pet.Queries ;

public class GetPetDetailQuery : IRequest<PetDetailDto>
{
    public int PetId { get; set; }
}

public class GetPetDetailQueryHandler : IRequestHandler<GetPetDetailQuery, PetDetailDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPetDetailQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PetDetailDto> Handle(GetPetDetailQuery request, CancellationToken cancellationToken)
    {
        var pet = await _uow.Pets.GetWithMedicalHistoryAsync(request.PetId);
        if (pet is null) throw new NotFoundException("Mascota no encontrada");
        return _mapper.Map<PetDetailDto>(pet);
    }
}