using System.ComponentModel.DataAnnotations;
using MediatR;
using AutoMapper;
using VetClinic.Domain.Ports.Repository;

namespace VetClinic.Application.UseCases.Owner.Commands;

public class RegisterOwnerCommand : IRequest<Unit>
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [RegularExpression(@"^(?=.*[a-zA-ZÀ-ÿ]).+$",
        ErrorMessage = "El nombre debe contener al menos una letra")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(8,
        MinimumLength = 8,
        ErrorMessage = "El DNI debe tener exactamente 8 dígitos")]
    [RegularExpression(@"^\d{8}$",
        ErrorMessage = "El DNI solo debe contener números")]
    public string Dni { get; set; } = null!;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(9,
        MinimumLength = 9,
        ErrorMessage = "El teléfono debe tener 9 dígitos")]
    [RegularExpression(@"^\d{9}$",
        ErrorMessage = "El teléfono solo debe contener números")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    public string? Email { get; set; }

    public string? Address { get; set; }
}

public class RegisterOwnerCommandHandler : IRequestHandler<RegisterOwnerCommand, Unit>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RegisterOwnerCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RegisterOwnerCommand request, CancellationToken cancellationToken)
    {
        var owner = _mapper.Map<Domain.Entities.Owner>(request);
        owner.CreatedAt = DateTime.Now;

        await _uow.Owners.AddAsync(owner);
        await _uow.SaveChangesAsync();

        return Unit.Value;
    }
}