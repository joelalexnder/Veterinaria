using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Appointment;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Grooming.Queries;

public class GetStylistAvailabilityQuery : IRequest<IEnumerable<AppointmentDto>>
{
    public int SpecialistId { get; set; }
    public DateOnly Date { get; set; }
}

public class GetStylistAvailabilityQueryHandler : IRequestHandler<GetStylistAvailabilityQuery, IEnumerable<AppointmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetStylistAvailabilityQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> Handle(GetStylistAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _uow.Appointments.GetByDateAsync(request.Date);
        var filtered = appointments.Where(a => a.SpecialistId == request.SpecialistId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(filtered);
    }
}