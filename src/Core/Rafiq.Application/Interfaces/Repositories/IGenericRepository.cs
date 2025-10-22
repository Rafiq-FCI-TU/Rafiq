using Rafiq.Domain.Common;

namespace Rafiq.Application.Interfaces.Repositories;
public interface IGenericRepository<T> where T : BaseEntity
{
    public Task<IReadOnlyList<T>> GetAllAsync();
    public Task<T?> GetEntityAsync(int id);
    public Task AddAsync(T entity);
    public void Update(T entity);
    public void Delete(T entity);
}