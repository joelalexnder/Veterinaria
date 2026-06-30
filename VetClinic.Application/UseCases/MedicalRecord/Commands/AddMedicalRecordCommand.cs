using MediatR;
using AutoMapper;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.MedicalRecord.Commands;


public class AddMedicalRecordCommand : IRequest<Unit>
{
    public int PetId { get; set; }
    public int VeterinarianId { get; set; }
    public string Reason { get; set; } = null!;
    public string Diagnosis { get; set; } = null!;
    public string? Treatment { get; set; }
    public string? Observations { get; set; }
    public decimal? RegisteredWeight { get; set; }
}

public class AddMedicalRecordCommandHandler : IRequestHandler<AddMedicalRecordCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AddMedicalRecordCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var record = _mapper.Map<Domain.Entities.MedicalRecord>(request);
        record.ConsultDate = DateTime.Now;

        await _uow.MedicalRecords.AddAsync(record);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}