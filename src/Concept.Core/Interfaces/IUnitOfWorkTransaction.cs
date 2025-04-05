namespace Concept.Core.Interfaces
{
    public interface IUnitOfWorkTransaction : IDisposable, IAsyncDisposable
    {
        void Commit();
        Task CommitAsync();
    }
}
