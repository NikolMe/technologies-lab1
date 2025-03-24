using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Technologies.Models;

namespace Technologies.Database;

public interface IRepository<T> where T : BaseEntity
{
    Task<int> AddEntityAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setter);

    Task<T[]> GetArrayAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    Task RemoveAsync(Expression<Func<T, bool>> predicate);
}