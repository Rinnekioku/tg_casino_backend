using System.Linq.Expressions;

namespace Common.Utils.Data.Interfaces;

public interface IRepository<T>
{
    public Task<bool> ContainsByPredicateAsync(Expression<Func<T, bool>> predicate);

    public Task<T?> GetByPredicateAsync(Expression<Func<T, bool>> predicate);

    public Task<T?> GetByIdAsync(int id);

    public Task<IEnumerable<T>> GetAllAsync();

    public Task AddAsync(T entity);

    public Task UpdateAsync(T entity);

    public void Delete(T entity);
}