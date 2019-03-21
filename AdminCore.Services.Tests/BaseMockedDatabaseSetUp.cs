using AdminCore.DAL.Entity_Framework;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AdminCore.Services.Tests
{
  public class BaseMockedDatabaseSetUp : MockDatabase
  {
    private static readonly EntityFrameworkContext DatabaseContext = Substitute.ForPartsOf<EntityFrameworkContext>();

    protected EntityFrameworkContext SetUpDatabase()
    {
      DatabaseContext.Configure().ClientRepository.Returns(GetMockedRepository(ClientRepository));
      DatabaseContext.Configure().ContractRepository.Returns(GetMockedRepository(ContractRepository));
      DatabaseContext.Configure().CountryRepository.Returns(GetMockedRepository(CountryRepository));
      DatabaseContext.Configure().EmployeeRepository.Returns(GetMockedRepository(EmployeeRepository));
      DatabaseContext.Configure().EmployeeRoleRepository.Returns(GetMockedRepository(EmployeeRoleRepository));
      DatabaseContext.Configure().EmployeeStatusRepository.Returns(GetMockedRepository(EmployeeStatusRepository));
      DatabaseContext.Configure().EventDatesRepository.Returns(GetMockedRepository(EventDateRepository));
      DatabaseContext.Configure().EventRepository.Returns(GetMockedRepository(EventRepository));
      DatabaseContext.Configure().EventStatusRepository.Returns(GetMockedRepository(EventStatusRepository));
      DatabaseContext.Configure().EventTypeRepository.Returns(GetMockedRepository(EventTypeRepository));
      DatabaseContext.Configure().TeamRepository.Returns(GetMockedRepository(TeamRepository));

      return DatabaseContext;
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

    protected static EntityFrameworkRepository<T> GetMockedEntityFrameworkRepository<T>(DbSet<T> sourceList) where T : class
    {
      DatabaseContext.When(x => x.Set<T>()).DoNotCallBase();
      DatabaseContext.Set<T>().Returns(sourceList);

      var mockedRepository = new EntityFrameworkRepository<T>(DatabaseContext);

      return mockedRepository;
    }

    private static EntityFrameworkRepository<T> GetMockedRepository<T>(IList<T> sourceList) where T : class
    {
      var queryableMockDbSet = GetQueryableMockDbSet(sourceList);
      var mockedEntityFrameworkRepository = GetMockedEntityFrameworkRepository(queryableMockDbSet);
      return mockedEntityFrameworkRepository;
    }
  }
}