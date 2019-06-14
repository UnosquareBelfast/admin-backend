using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdminCore.DAL
{
  public interface IRepository<T>
  {
    void Delete(object id);

    void Delete(T entityToDelete);

    IList<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includeProperties);

    bool Exists(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includeProperties);

    T GetSingle(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);

    T GetSingleThenIncludes(Expression<Func<T, bool>> filter = null,
        params (Expression<Func<T, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[]
            includeDatas);

    IList<T> GetThenIncludes(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params (Expression<Func<T, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[]
            includeDatas);
    
    IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params (Expression<Func<T, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[] includeDatas);

    T Insert(T entity);

    T Update(T entityToUpdate);
  }
}
