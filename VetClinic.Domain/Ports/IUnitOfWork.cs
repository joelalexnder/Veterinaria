namespace VetClinic.Domain.Ports;

public interface IUnitOfWork
{
    IPetRepository Pets { get; }
    Task<int> SaveChangesAsync();
}