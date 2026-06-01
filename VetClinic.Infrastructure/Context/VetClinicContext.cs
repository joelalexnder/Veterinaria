using VetClinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace VetClinic.Infrastructure.Context;

public partial class VetClinicContext : DbContext
{
    public VetClinicContext()
    {
    }

    public VetClinicContext(DbContextOptions<VetClinicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<GroomingAppointment> GroomingAppointments { get; set; }

    public virtual DbSet<GroomingPackage> GroomingPackages { get; set; }

    public virtual DbSet<GroomingService> GroomingServices { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ServiceArea> ServiceAreas { get; set; }

    public virtual DbSet<Specialist> Specialists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VaccinationRecord> VaccinationRecords { get; set; }

    public virtual DbSet<Vaccine> Vaccines { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appointments_pkey");

            entity.ToTable("appointments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentDate).HasColumnName("appointment_date");
            entity.Property(e => e.CancellationReason).HasColumnName("cancellation_reason");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.ServiceAreaId).HasColumnName("service_area_id");
            entity.Property(e => e.SpecialistId).HasColumnName("specialist_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Pet).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("appointments_pet_id_fkey");

            entity.HasOne(d => d.ServiceArea).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointments_service_area_id_fkey");

            entity.HasOne(d => d.Specialist).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.SpecialistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointments_specialist_id_fkey");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_logs_pkey");

            entity.ToTable("audit_logs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .HasColumnName("action");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.ResourceId)
                .HasMaxLength(50)
                .HasColumnName("resource_id");
            entity.Property(e => e.ResourceName)
                .HasMaxLength(100)
                .HasColumnName("resource_name");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("audit_logs_user_id_fkey");
        });

        modelBuilder.Entity<GroomingAppointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("grooming_appointments_pkey");

            entity.ToTable("grooming_appointments");

            entity.HasIndex(e => e.AppointmentId, "grooming_appointments_appointment_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.FinalPrice)
                .HasPrecision(8, 2)
                .HasColumnName("final_price");
            entity.Property(e => e.GroomingPackageId).HasColumnName("grooming_package_id");
            entity.Property(e => e.ServiceStatus)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("service_status");
            entity.Property(e => e.StylistNotes).HasColumnName("stylist_notes");

            entity.HasOne(d => d.Appointment).WithOne(p => p.GroomingAppointment)
                .HasForeignKey<GroomingAppointment>(d => d.AppointmentId)
                .HasConstraintName("grooming_appointments_appointment_id_fkey");

            entity.HasOne(d => d.GroomingPackage).WithMany(p => p.GroomingAppointments)
                .HasForeignKey(d => d.GroomingPackageId)
                .HasConstraintName("grooming_appointments_grooming_package_id_fkey");
        });

        modelBuilder.Entity<GroomingPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("grooming_packages_pkey");

            entity.ToTable("grooming_packages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.SpecialPrice)
                .HasPrecision(8, 2)
                .HasColumnName("special_price");
        });

        modelBuilder.Entity<GroomingService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("grooming_services_pkey");

            entity.ToTable("grooming_services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasePrice)
                .HasPrecision(8, 2)
                .HasColumnName("base_price");
            entity.Property(e => e.CoatType)
                .HasMaxLength(50)
                .HasColumnName("coat_type");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PetSize)
                .HasMaxLength(20)
                .HasColumnName("pet_size");
            entity.Property(e => e.Species)
                .HasMaxLength(50)
                .HasColumnName("species");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("medical_records_pkey");

            entity.ToTable("medical_records");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConsultDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("consult_date");
            entity.Property(e => e.Diagnosis).HasColumnName("diagnosis");
            entity.Property(e => e.Observations).HasColumnName("observations");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");
            entity.Property(e => e.RegisteredWeight)
                .HasPrecision(5, 2)
                .HasColumnName("registered_weight");
            entity.Property(e => e.Treatment).HasColumnName("treatment");
            entity.Property(e => e.VeterinarianId).HasColumnName("veterinarian_id");

            entity.HasOne(d => d.Pet).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("medical_records_pet_id_fkey");

            entity.HasOne(d => d.Veterinarian).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.VeterinarianId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medical_records_veterinarian_id_fkey");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("owners_pkey");

            entity.ToTable("owners");

            entity.HasIndex(e => e.Dni, "owners_dni_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Dni)
                .HasMaxLength(15)
                .HasColumnName("dni");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pets_pkey");

            entity.ToTable("pets");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Breed)
                .HasMaxLength(100)
                .HasColumnName("breed");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.GeneralHealthStatus).HasColumnName("general_health_status");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Sex)
                .HasMaxLength(20)
                .HasColumnName("sex");
            entity.Property(e => e.Species)
                .HasMaxLength(50)
                .HasColumnName("species");
            entity.Property(e => e.Weight)
                .HasPrecision(5, 2)
                .HasColumnName("weight");

            entity.HasOne(d => d.Owner).WithMany(p => p.Pets)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("pets_owner_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ServiceArea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_areas_pkey");

            entity.ToTable("service_areas");

            entity.HasIndex(e => e.Name, "service_areas_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Specialist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("specialists_pkey");

            entity.ToTable("specialists");

            entity.HasIndex(e => e.UserId, "specialists_user_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ServiceAreaId).HasColumnName("service_area_id");
            entity.Property(e => e.SpecialtyName)
                .HasMaxLength(100)
                .HasColumnName("specialty_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ServiceArea).WithMany(p => p.Specialists)
                .HasForeignKey(d => d.ServiceAreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialists_service_area_id_fkey");

            entity.HasOne(d => d.User).WithOne(p => p.Specialist)
                .HasForeignKey<Specialist>(d => d.UserId)
                .HasConstraintName("specialists_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<VaccinationRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vaccination_records_pkey");

            entity.ToTable("vaccination_records");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApplicationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("application_date");
            entity.Property(e => e.BatchNumber)
                .HasMaxLength(50)
                .HasColumnName("batch_number");
            entity.Property(e => e.NextBoosterDate).HasColumnName("next_booster_date");
            entity.Property(e => e.PetId).HasColumnName("pet_id");
            entity.Property(e => e.VaccineId).HasColumnName("vaccine_id");
            entity.Property(e => e.VeterinarianId).HasColumnName("veterinarian_id");

            entity.HasOne(d => d.Pet).WithMany(p => p.VaccinationRecords)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("vaccination_records_pet_id_fkey");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccinationRecords)
                .HasForeignKey(d => d.VaccineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vaccination_records_vaccine_id_fkey");

            entity.HasOne(d => d.Veterinarian).WithMany(p => p.VaccinationRecords)
                .HasForeignKey(d => d.VeterinarianId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vaccination_records_veterinarian_id_fkey");
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vaccines_pkey");

            entity.ToTable("vaccines");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BoosterIntervalDays).HasColumnName("booster_interval_days");
            entity.Property(e => e.MinAgeDays).HasColumnName("min_age_days");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Species)
                .HasMaxLength(50)
                .HasColumnName("species");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
