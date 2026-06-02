using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.MedicalRecord;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Pet.Queries ;

public class GetPetMedicalHistoryQuery : IRequest<IEnumerable<MedicalRecordDto>>
{
    public int PetId { get; set; }
}

public class GetPetMedicalHistoryQueryHandler : IRequestHandler<GetPetMedicalHistoryQuery, IEnumerable<MedicalRecordDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPetMedicalHistoryQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MedicalRecordDto>> Handle(GetPetMedicalHistoryQuery request, CancellationToken cancellationToken)
    {
        var records = await _uow.MedicalRecords.GetByPetIdAsync(request.PetId);
        return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
    }
}