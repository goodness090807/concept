using Concept.Core.Interfaces;
using Concept.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Concept.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _repositories = new Dictionary<Type, object>();
        }

        public TRepo GetRepository<TRepo>() where TRepo : class
        {
            if (_repositories.ContainsKey(typeof(TRepo)))
            {
                return (TRepo)_repositories[typeof(TRepo)];
            }

            var repository = _serviceProvider.GetRequiredService<TRepo>();
            _repositories.Add(typeof(TRepo), repository);
            return repository;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IUnitOfWorkTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return new UnitOfWorkTransaction(this, transaction);
        }

        public async Task<IUnitOfWorkTransaction> BeginTransactionAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            return new UnitOfWorkTransaction(this, transaction);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
