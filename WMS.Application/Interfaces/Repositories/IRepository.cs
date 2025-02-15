using System.Linq.Expressions;
using WMS.Domain.Common;

namespace WMS.Application.Interfaces.Repositories
{
	public interface IRepository<T> where T : BaseEntity
	{
		Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params string[] tables);
		Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null, params string[] tables);
		Task<T> UpdateAsync(T entity);
		Task<T> AddAsync(T entity);
		Task DeleteAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);
		Task UpdateRangeAsync(IEnumerable<T> entities);
		Task<IQueryable<T>> GetCustomizedListAsync(Expression<Func<T, bool>> filter = null, string orderBy = "Created", bool desc = false, int skip = 0, int take = 10, params string[] tables);
		Task<TResult> GetOneByDtoAsync<TResult>(Expression<Func<T, bool>> filter = null, Expression<Func<T, TResult>> selector = null);
	}
}
