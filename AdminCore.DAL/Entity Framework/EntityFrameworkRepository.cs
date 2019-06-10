using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdminCore.DAL.Entity_Framework
{
  public class EntityFrameworkRepository<T> : IRepository<T>
    where T : class
  {
    private readonly IDatabaseContext _context;
    private readonly DbSet<T> _dbSet;

    public EntityFrameworkRepository(IDatabaseContext databaseContext)
    {
      _context = databaseContext;
      _dbSet = ((EntityFrameworkContext)databaseContext).Set<T>();
    }

    public void Delete(object id)
    {
      var entityToDelete = _dbSet.Find(id);
      Delete(entityToDelete);
    }

    public virtual void Delete(T entityToDelete)
    {
      if (((EntityFrameworkContext)_context).Entry(entityToDelete).State == EntityState.Detached)
      {
        _dbSet.Attach(entityToDelete);
      }

      _dbSet.Remove(entityToDelete);
    }

    public IList<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
    {
      var newIncludes = includeProperties.Select(x => (includeProperty: x,  thenIncludes: (Expression<Func<object, object>>[])null)).ToArray();

      return GetAsQueryable(filter, orderBy, newIncludes).ToList();
    }

    public bool Exists(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
    {
      var newIncludes = includeProperties.Select(x => (includeProperty: x,  thenIncludes: (Expression<Func<object, object>>[])null)).ToArray();
      return GetAsQueryable(filter, orderBy, newIncludes).Any();
    }

    public T GetSingle(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
    {
      var newIncludes = includes.Select(x => (includeProperty: x,  thenIncludes: (Expression<Func<object, object>>[])null)).ToArray();

      var query = GetAsQueryable(filter, null, newIncludes);

      return query.SingleOrDefault();
    }

    public T GetSingleThenIncludes(Expression<Func<T, bool>> filter = null, params (Expression<Func<T, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[] includeDatas)
    {
      var query = GetAsQueryable(filter, null, includeDatas);

      return query.SingleOrDefault();
    }

    public IList<T> GetThenIncludes(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params (Expression<Func<T, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[] includeDatas)
    {
      return GetAsQueryable(filter, orderBy, includeDatas).ToList();
    }

    public T Insert(T entity)
    {
      return _dbSet.Add(entity)?.Entity;
    }

    public IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
      params (Expression<Func<T, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[] includeDatas)
    {
      var queryableData = _dbSet.AsQueryable();

      if (filter != null)
      {
        queryableData = queryableData.Where(filter);
      }

      queryableData = IncludeEntities(queryableData, includeDatas);

      if (orderBy != null)
      {
        queryableData = queryableData.OrderBy(x => orderBy);
      }

      return queryableData;
    }

    public virtual T Update(T entityToUpdate)
    {
      var updatedEntity = _dbSet.Attach(entityToUpdate);
      ((EntityFrameworkContext)_context).Entry(entityToUpdate).State = EntityState.Modified;
      return updatedEntity.Entity;
    }

    private static IQueryable<T> IncludeEntities(IQueryable<T> query,
      params (Expression<Func<T, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[] includeDatas)
    {
      foreach (var (includeProperty, thenIncludes) in includeDatas)
      {
        if (includeProperty != null)
        {
          var newQuery = query.Include(includeProperty);
          if (thenIncludes != null && thenIncludes.Any())
          {
            foreach (var thenInclude in thenIncludes)
            {
              newQuery = newQuery.ThenInclude(thenInclude);
            }
          }

          query = newQuery;
        }
      }

      return query;
    }
  }
}
