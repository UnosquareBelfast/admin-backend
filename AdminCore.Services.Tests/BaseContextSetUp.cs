using AdminCore.DAL.Entity_Framework;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;

namespace AdminCore.Services.Tests
{
  public class BaseContextSetUp
  {
    public EntityFrameworkContext SetUpContext()
    {
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>();
      return databaseContext;
    }

    public DbSet<T> SetUpMockSet<T>(T type) where T : class
    {
      var mockDbSet = GetQueryableMockDbSet(new List<T> { type });
      return mockDbSet;
    }

    protected static DbSet<T> GetQueryableMockDbSet<T>(IList<T> sourceList) where T : class
    {
      var queryable = sourceList.AsQueryable();
      var dbSet = Substitute.For<DbSet<T>, IQueryable<T>>();
      var enumerator = queryable.GetEnumerator();

      ((IQueryable<T>)dbSet).Provider.Returns(queryable.Provider);
      ((IQueryable<T>)dbSet).Expression.Returns(queryable.Expression);
      ((IQueryable<T>)dbSet).ElementType.Returns(queryable.ElementType);
      ((IQueryable<T>)dbSet).GetEnumerator().Returns(enumerator).AndDoes(x => enumerator.Reset());

      return dbSet;
    }
  }
}