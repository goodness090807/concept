namespace Concept.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        TRepo GetRepository<TRepo>() where TRepo : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();

        IUnitOfWorkTransaction BeginTransaction();
        Task<IUnitOfWorkTransaction> BeginTransactionAsync();
    }
}
