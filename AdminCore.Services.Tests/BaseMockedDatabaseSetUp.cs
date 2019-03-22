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

    protected virtual EntityFrameworkContext SetUpDatabase()
    {
      var mockClientRepository = GetMockedRepository(ClientRepository);
      var mockContractRepository = GetMockedRepository(ContractRepository);
      var mockCountryRepository = GetMockedRepository(CountryRepository);
      var mockEmployeeRepository = GetMockedRepository(EmployeeRepository);
      var mockEmployeeRoleRepository = GetMockedRepository(EmployeeRoleRepository);
      var mockEmployeeStatusRepository = GetMockedRepository(EmployeeStatusRepository);
      var mockEventDatesRepository = GetMockedRepository(EventDateRepository);
      var mockEventRepository = GetMockedRepository(EventRepository);
      var mockEventStatusRepository = GetMockedRepository(EventStatusRepository);
      var mockEventTypeRepository = GetMockedRepository(EventTypeRepository);
      var mockTeamRepository = GetMockedRepository(TeamRepository);

      DatabaseContext.Configure().ClientRepository.Returns(mockClientRepository);
      DatabaseContext.Configure().ContractRepository.Returns(mockContractRepository);
      DatabaseContext.Configure().CountryRepository.Returns(mockCountryRepository);
      DatabaseContext.Configure().EmployeeRepository.Returns(mockEmployeeRepository);
      DatabaseContext.Configure().EmployeeRoleRepository.Returns(mockEmployeeRoleRepository);
      DatabaseContext.Configure().EmployeeStatusRepository.Returns(mockEmployeeStatusRepository);
      DatabaseContext.Configure().EventDatesRepository.Returns(mockEventDatesRepository);
      DatabaseContext.Configure().EventRepository.Returns(mockEventRepository);
      DatabaseContext.Configure().EventStatusRepository.Returns(mockEventStatusRepository);
      DatabaseContext.Configure().EventTypeRepository.Returns(mockEventTypeRepository);
      DatabaseContext.Configure().TeamRepository.Returns(mockTeamRepository);

      return DatabaseContext;
    }

    protected virtual DbSet<T> GetQueryableMockDbSet<T>(IList<T> sourceList) where T : class
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

    protected virtual EntityFrameworkRepository<T> GetMockedEntityFrameworkRepository<T>(DbSet<T> sourceList) where T : class
    {
      DatabaseContext.When(x => x.Set<T>()).DoNotCallBase();
      DatabaseContext.Set<T>().Returns(sourceList);

      var mockedRepository = new EntityFrameworkRepository<T>(DatabaseContext);

      return mockedRepository;
    }

    protected virtual EntityFrameworkRepository<T> GetMockedRepository<T>(IList<T> sourceList) where T : class
    {
      var queryableMockDbSet = GetQueryableMockDbSet(sourceList);
      var mockedEntityFrameworkRepository = GetMockedEntityFrameworkRepository(queryableMockDbSet);
      return mockedEntityFrameworkRepository;
    }
  }
}