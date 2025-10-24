using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Domain.Common;
using Rafiq.Persistence.Data;
using System.Collections.Concurrent;

namespace Rafiq.Persistence;
public class UnitOfWork(RafiqDbContext rafiqDbContext) : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repositories = new();

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        var key = typeof(T).Name;

        return (IGenericRepository<T>)_repositories.GetOrAdd(key, _ => new GenericRepository<T>(rafiqDbContext));
    }

    public async Task<int> CompleteAsync() => await rafiqDbContext.SaveChangesAsync();

    public void Dispose() => rafiqDbContext.DisposeAsync();

    public async ValueTask DisposeAsync() => await rafiqDbContext.DisposeAsync();
}