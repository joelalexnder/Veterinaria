namespace VetClinic.Domain.Ports.Repository;

public interface IUnitOfWork
{
    IPetRepository Pets { get; }
    Task<int> SaveChangesAsync();
}