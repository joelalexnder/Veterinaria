using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Grooming;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Grooming.Queries;

public class GetGroomingPackagesQuery : IRequest<IEnumerable<GroomingPackageDto>>
{
}

public class GetGroomingPackagesQueryHandler : IRequestHandler<GetGroomingPackagesQuery, IEnumerable<GroomingPackageDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetGroomingPackagesQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroomingPackageDto>> Handle(GetGroomingPackagesQuery request, CancellationToken cancellationToken)
    {
        var packages = await _uow.GroomingPackages.GetAllWithServicesAsync();
        return _mapper.Map<IEnumerable<GroomingPackageDto>>(packages);
    }
}