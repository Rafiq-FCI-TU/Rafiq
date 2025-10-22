using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Domain.Common;
using Rafiq.Persistence.Data;

namespace Rafiq.Persistence;
public class GenericRepository<T> (RafiqDbContext rafiqDbContext) : IGenericRepository<T> where T : BaseEntity
{
    public async Task<IReadOnlyList<T>> GetAllAsync() => await rafiqDbContext.Set<T>().ToListAsync();

    public async Task<T?> GetEntityAsync(int id) => await rafiqDbContext.Set<T>().FindAsync(id);

    public async Task AddAsync(T entity) => await rafiqDbContext.Set<T>().AddAsync(entity);

    public void Delete(T entity) => rafiqDbContext.Set<T>().Remove(entity);
    
    public void Update(T entity) => rafiqDbContext.Set<T>().Update(entity);
}