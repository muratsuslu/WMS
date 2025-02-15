using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Parameters;
using WMS.Domain.Common;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class Repository<T> : IRepository<T>
		where T : BaseEntity
	
	{
		private readonly ApplicationDbContext _context;
		public Repository(ApplicationDbContext context)
		{
			_context = context;
			_entitySet = _context.Set<T>();
		}
		protected readonly DbSet<T> _entitySet;
		public async Task<T> AddAsync(T entity)
		{
			try
			{
				await _entitySet.AddAsync(entity);
				await _context.SaveChangesAsync();
				return entity;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			try
			{
				int batchSize = 200;
				var entityList = entities.ToList();
				int total = entityList.Count;
				int processed = 0;

				while (processed < total)
				{
					var batch = entityList.Skip(processed).Take(batchSize);
					await _context.AddRangeAsync(batch);
					await _context.SaveChangesAsync();
					_context.ChangeTracker.Clear(); // Change tracker'ı temizle, belleği serbest bırak
					processed += batch.Count();
				}

				return;
			}
			catch (Exception ex)
			{
			}
		}

		public async Task DeleteAsync(T entity)
		{
			try
			{
				_entitySet.Remove(entity);
				return;
			}
			catch (Exception ex)
			{
			}
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params string[] tables)
		{
			try
			{
				var data = filter == null
					? await _entitySet.ToListAsync()
					: await _entitySet.Where(filter).ToListAsync();
				return data;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<IQueryable<T>> GetCustomizedListAsync(Expression<Func<T, bool>> filter = null, string orderBy = "Created", bool desc = false, int skip = 0, int take = 10, params string[] tables)
		{
			try
			{
				IQueryable<T> entities = null;
				IQueryable<T> extendedDb = _context.Set<T>();

				// Include navigation properties
				if (tables.Length > 0)
				{
					foreach (var table in tables)
					{
						extendedDb = extendedDb.Include(table);
					}
				}

				// Apply ordering
				extendedDb = CustomOrderBy(extendedDb, typeof(T).GetProperty(orderBy), desc);

				// Apply filter
				extendedDb = filter != null ? extendedDb.Where(filter) : extendedDb;

				// Apply pagination
				if (take > 0)
				{
					entities = extendedDb.Skip(skip).Take(take);
				}
				else
				{
					entities = extendedDb.Skip(skip);
				}

				// ToListAsync to execute the query
				var result = await entities.ToListAsync();
				return result.AsQueryable();
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null, params string[] tables)
		{
			try
			{
				IQueryable<T> query = _entitySet;

				// Include navigation properties if any tables are specified
				if (tables != null && tables.Length > 0)
				{
					foreach (var table in tables)
					{
						query = query.Include(table);
					}
				}

				// Apply the filter
				var data = await query.FirstOrDefaultAsync(filter);
				return data;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<TResult> GetOneByDtoAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector)
		{
			try
			{
				var query = _entitySet.AsQueryable();

				if (filter != null)
				{
					query = query.Where(filter);
				}

				if (selector == null)
				{
					return default(TResult);
				}

				var result = await query.Select(selector).FirstOrDefaultAsync();

				if (result == null)
				{
					return default(TResult);
				}

				return result;
			}
			catch (Exception ex)
			{
				return default(TResult);
			}
		}

		public async Task<T> UpdateAsync(T entity)
		{
			try
			{
				entity.Updated = DateTime.Now;
				_context.Entry(entity).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return entity;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task UpdateRangeAsync(IEnumerable<T> entities)
		{
			try
			{

				foreach (var entity in entities)
				{
					entity.Updated = DateTime.Now;
				}
				_context.UpdateRange(entities);

				// Değişiklikleri kaydet
				await _context.SaveChangesAsync();
				return;
			}
			catch (Exception ex)
			{
			}
		}

		private IQueryable<T> CustomOrderBy(IQueryable<T> source, PropertyInfo propertyInfo, bool desc)
		{
			if (propertyInfo == null)
			{
				propertyInfo = typeof(T).GetProperty("Created");
			}

			var type = typeof(T);
			var parameter = Expression.Parameter(type, "p");
			var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
			var orderByExpression = Expression.Lambda(propertyAccess, parameter);
			var order = desc ? "OrderByDescending" : "OrderBy";
			var resultExpression = Expression.Call(typeof(Queryable), order, new Type[] { type, propertyInfo.PropertyType },
										  source.Expression, Expression.Quote(orderByExpression));
			return source.Provider.CreateQuery<T>(resultExpression);
		}
	}
}
