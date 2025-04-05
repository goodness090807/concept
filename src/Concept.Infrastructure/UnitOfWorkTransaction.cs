using Concept.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Concept.Infrastructure
{
    public class UnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContextTransaction _transaction;
        private bool _committed;
        private bool _disposed;

        public UnitOfWorkTransaction(IUnitOfWork unitOfWork, IDbContextTransaction transaction)
        {
            _unitOfWork = unitOfWork;
            _transaction = transaction;
        }

        public void Commit()
        {
            if (_committed) return;

            _unitOfWork.SaveChanges();
            _transaction.Commit();
            _committed = true;
        }

        public async Task CommitAsync()
        {
            if (_committed) return;

            await _unitOfWork.SaveChangesAsync();
            await _transaction.CommitAsync();
            _committed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (!_committed)
                {
                    _transaction.Rollback();
                }

                _transaction.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_committed)
            {
                await _transaction.RollbackAsync();
            }

            await _transaction.DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(false);
            GC.SuppressFinalize(this);
        }
    }
}
