using MediatR;
using AutoMapper;
using VetClinic.Domain.DTOs.Auth;
using VetClinic.Domain.Ports.Repository;
using VetClinic.Domain.Entities;

namespace VetClinic.Application.UseCases.Auth.Queries;

public class GetAuditLogQuery : IRequest<IEnumerable<AuditLogDto>>
{
    public int? UserId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}

public class GetAuditLogQueryHandler : IRequestHandler<GetAuditLogQuery, IEnumerable<AuditLogDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAuditLogQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuditLogDto>> Handle(GetAuditLogQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<AuditLog> logs;

        if (request.UserId.HasValue)
            logs = await _uow.AuditLogs.GetByUserIdAsync(request.UserId.Value);
        else if (request.From.HasValue && request.To.HasValue)
            logs = await _uow.AuditLogs.GetByDateRangeAsync(request.From.Value, request.To.Value);
        else
            logs = await _uow.AuditLogs.GetAllAsync();

        return _mapper.Map<IEnumerable<AuditLogDto>>(logs);
    }
}