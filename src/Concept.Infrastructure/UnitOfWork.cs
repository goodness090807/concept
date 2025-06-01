using Concept.Core.Interfaces;
using Concept.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Concept.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }
    }
}
