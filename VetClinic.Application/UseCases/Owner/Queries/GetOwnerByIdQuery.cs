using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Pet;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Owner.Queries;

public class GetOwnerByIdQuery : IRequest<OwnerDto>
{
    public int Id { get; set; }
}

public class GetOwnerByIdQueryHandler : IRequestHandler<GetOwnerByIdQuery, OwnerDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetOwnerByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<OwnerDto> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
    {
        var owner = await _uow.Owners.GetByIdAsync(request.Id);
        if (owner is null) throw new Exception("Propietario no encontrado");
        return _mapper.Map<OwnerDto>(owner);
    }
}