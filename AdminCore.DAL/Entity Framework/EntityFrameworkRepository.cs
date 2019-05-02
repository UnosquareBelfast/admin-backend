using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

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

    public IList<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
    {
      return GetAsQueryable(filter, orderBy, includes).ToList();
    }

    public T GetSingle(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
    {
      var query = GetAsQueryable(filter, null, includes);

      return query.SingleOrDefault();
    }

    public T Insert(T entity)
    {
      return _dbSet.Add(entity)?.Entity;
    }

    public IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
    {
      var queryableData = _dbSet.AsQueryable();

      if (filter != null)
      {
        queryableData = queryableData.Where(filter);
      }

//      queryableData = IncludeEntities(queryableData, includes);
      if (includes != null)
      {
        queryableData = includes(queryableData);
      }

      if (orderBy != null)
      {
        queryableData = queryableData.OrderBy(x => orderBy);
      }

      return queryableData;
    }

    public virtual void Update(T entityToUpdate)
    {
      _dbSet.Attach(entityToUpdate);
      ((EntityFrameworkContext)_context).Entry(entityToUpdate).State = EntityState.Modified;
    }

    public IQueryable<T> IncludeEntities(IQueryable<T> query, Expression<Func<T, object>>[] includeProperties)
    {
      if (includeProperties != null)
      {
        foreach (var includeProperty in includeProperties)
        {
          query = query.Include(includeProperty);
        }
      }

      return query;
    }
  }
}