using AutoMapper;
using VetClinic.Application.UseCases.Appointment.Commands;
using VetClinic.Application.UseCases.Auth.Commands;
using VetClinic.Application.UseCases.Grooming.Commands;
using VetClinic.Application.UseCases.MedicalRecord.Commands;
using VetClinic.Application.UseCases.Owner.Commands;
using VetClinic.Application.UseCases.Pet.Commands;
using VetClinic.Application.UseCases.Vaccination.Commands;
using VetClinic.Domain.DTOs.Appointment;
using VetClinic.Domain.DTOs.Auth;
using VetClinic.Domain.DTOs.Grooming;
using VetClinic.Domain.DTOs.MedicalRecord;
using VetClinic.Domain.DTOs.Pet;
using VetClinic.Domain.DTOs.ServiceArea;
using VetClinic.Domain.DTOs.Specialist;
using VetClinic.Domain.DTOs.VaccinationRecord;
using VetClinic.Domain.DTOs.Vaccine;
using VetClinic.Domain.Entities;

namespace VetClinic.Application.Mappings;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ─── Pet ───────────────────────────────────────────
        CreateMap<RegisterPetCommand, Pet>();
        CreateMap<Pet, PetDto>()
            .ForMember(dest => dest.OwnerName,
                opt => opt.MapFrom(src => src.Owner != null ? src.Owner.FullName : string.Empty));
        CreateMap<Pet, PetDetailDto>()
            .ForMember(dest => dest.OwnerName,
                opt => opt.MapFrom(src => src.Owner != null ? src.Owner.FullName : string.Empty))
            .ForMember(dest => dest.LastMedicalRecord,
                opt => opt.MapFrom(src => src.MedicalRecords
                    .OrderByDescending(m => m.ConsultDate)
                    .FirstOrDefault()));

        // ─── Owner ─────────────────────────────────────────
        CreateMap<RegisterOwnerCommand, Owner>();
        CreateMap<Owner, OwnerDto>()
            .ForMember(dest => dest.PetNames,
                opt => opt.MapFrom(src => src.Pets.Select(p => p.Name).ToList()));

        // ─── MedicalRecord ─────────────────────────────────
        CreateMap<AddMedicalRecordCommand, MedicalRecord>();
        CreateMap<MedicalRecord, MedicalRecordDto>()
            .ForMember(dest => dest.PetName,
                opt => opt.MapFrom(src => src.Pet != null ? src.Pet.Name : string.Empty))
            .ForMember(dest => dest.VeterinarianName,
                opt => opt.MapFrom(src => src.Veterinarian != null ? src.Veterinarian.FullName : string.Empty));

        // ─── Appointment ───────────────────────────────────
        CreateMap<ScheduleAppointmentCommand, Appointment>();
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PetName,
                opt => opt.MapFrom(src => src.Pet != null ? src.Pet.Name : string.Empty))
            .ForMember(dest => dest.SpecialistName,
                opt => opt.MapFrom(src => src.Specialist != null ? src.Specialist.User.FullName : string.Empty))
            .ForMember(dest => dest.ServiceAreaName,
                opt => opt.MapFrom(src => src.ServiceArea != null ? src.ServiceArea.Name : string.Empty));

        // ─── VaccinationRecord ─────────────────────────────
        CreateMap<RegisterVaccinationCommand, VaccinationRecord>();
        CreateMap<VaccinationRecord, VaccinationRecordDto>()
            .ForMember(dest => dest.PetName,
                opt => opt.MapFrom(src => src.Pet != null ? src.Pet.Name : string.Empty))
            .ForMember(dest => dest.VaccineName,
                opt => opt.MapFrom(src => src.Vaccine != null ? src.Vaccine.Name : string.Empty))
            .ForMember(dest => dest.VeterinarianName,
                opt => opt.MapFrom(src => src.Veterinarian != null ? src.Veterinarian.FullName : string.Empty))
            .ForMember(dest => dest.DaysOverdue,
                opt => opt.MapFrom(src => src.NextBoosterDate.HasValue && src.NextBoosterDate < DateOnly.FromDateTime(DateTime.Today)
                    ? DateOnly.FromDateTime(DateTime.Today).DayNumber - src.NextBoosterDate.Value.DayNumber
                    : 0));

        // ─── Vaccine ───────────────────────────────────────
        CreateMap<AddVaccineCatalogCommand, Vaccine>();
        CreateMap<Vaccine, VaccineDto>();

        // ─── Grooming ──────────────────────────────────────
        CreateMap<ScheduleGroomingCommand, GroomingAppointment>();
        CreateMap<AddGroomingPackageCommand, GroomingPackage>();
        CreateMap<GroomingAppointment, GroomingAppointmentDto>()
            .ForMember(dest => dest.PetName,
                opt => opt.MapFrom(src => src.Appointment != null ? src.Appointment.Pet.Name : string.Empty))
            .ForMember(dest => dest.PackageName,
                opt => opt.MapFrom(src => src.GroomingPackage != null ? src.GroomingPackage.Name : string.Empty))
            .ForMember(dest => dest.AppointmentDate,
                opt => opt.MapFrom(src => src.Appointment != null ? src.Appointment.AppointmentDate : default));
        CreateMap<GroomingPackage, GroomingPackageDto>();
        CreateMap<GroomingService, GroomingServiceDto>();

        // ─── Auth ──────────────────────────────────────────
        CreateMap<RegisterUserCommand, User>();
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));
        CreateMap<User, AuthResponseDto>()
            .ForMember(dest => dest.Role,
                opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty))
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore());

        // ─── AuditLog ──────────────────────────────────────
        CreateMap<AuditLog, AuditLogDto>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty));
        
        // ─── ServiceArea ───────────────────────────────────
        CreateMap<ServiceArea, ServiceAreaDto>();

// ─── Specialist ────────────────────────────────────
        CreateMap<Specialist, SpecialistDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.User != null ? src.User.FullName : string.Empty))
            .ForMember(dest => dest.ServiceAreaName,
                opt => opt.MapFrom(src => src.ServiceArea != null ? src.ServiceArea.Name : string.Empty));
    }
}