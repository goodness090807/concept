using System.Data;

namespace Concept.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IDbTransaction BeginTransaction();
    }
}
