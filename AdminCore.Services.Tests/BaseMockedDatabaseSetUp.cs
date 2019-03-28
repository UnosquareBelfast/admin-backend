using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AdminCore.Services.Tests
{
  public class BaseMockedDatabaseSetUp : MockDatabase
  {
    private static readonly IConfiguration Configuration = Substitute.For<IConfiguration>();
    private static readonly AdminCoreContext AdminCoreContext = Substitute.For<AdminCoreContext>(Configuration);
    private static readonly EntityFrameworkContext DatabaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);

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
      var mockMandatoryEventRepository = GetMockedRepository(MandatoryEventRepository);

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
      DatabaseContext.Configure().MandatoryEventRepository.Returns(mockMandatoryEventRepository);

      DatabaseContext.When(x => x.RetrieveRepository<Event>()).DoNotCallBase();
      DatabaseContext.When(x => x.RetrieveRepository<EventDate>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

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

      dbSet.Find(Arg.Any<object>()).Returns(x => sourceList.ElementAt(0));

      return dbSet;
    }

    protected virtual IRepository<T> GetMockedEntityFrameworkRepository<T>(DbSet<T> sourceList) where T : class
    {
      DatabaseContext.When(x => x.Set<T>()).DoNotCallBase();
      DatabaseContext.Set<T>().Returns(sourceList);

      var mockedRepository = Substitute.ForPartsOf<EntityFrameworkRepository<T>>(DatabaseContext);

      mockedRepository.When(x => x.Update(Arg.Any<T>())).DoNotCallBase();
      mockedRepository.When(x => x.Delete(Arg.Any<T>())).DoNotCallBase();
      mockedRepository.When(x => x.Delete(Arg.Any<object>())).DoNotCallBase();

      return mockedRepository;
    }

    protected virtual IRepository<T> GetMockedRepository<T>(IList<T> sourceList) where T : class
    {
      var queryableMockDbSet = GetQueryableMockDbSet(sourceList);
      var mockedEntityFrameworkRepository = GetMockedEntityFrameworkRepository(queryableMockDbSet);
      return mockedEntityFrameworkRepository;
    }
  }
}