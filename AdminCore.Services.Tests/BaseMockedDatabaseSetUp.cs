﻿using AdminCore.Common.Interfaces;
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

    public BaseMockedDatabaseSetUp()
    {
      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();
    }
    
    protected virtual EntityFrameworkContext SetUpEventRepository(EntityFrameworkContext databaseContext, IList<Event> eventList)
    {
      var mockEventRepository = GetMockedRepository(eventList);
      databaseContext.Configure().EventRepository.Returns(mockEventRepository);
      databaseContext.When(x => x.RetrieveRepository<Event>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
    }

    protected virtual EntityFrameworkContext SetUpMandatoryEventRepository(EntityFrameworkContext databaseContext, IList<MandatoryEvent> mandatoryEventList)
    {
      var mockMandatoryEventRepository = GetMockedRepository(mandatoryEventList);
      databaseContext.Configure().MandatoryEventRepository.Returns(mockMandatoryEventRepository);
      databaseContext.When(x => x.RetrieveRepository<MandatoryEvent>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
    }

    protected virtual EntityFrameworkContext SetUpEventDateRepository(EntityFrameworkContext databaseContext, IList<EventDate> eventDateList)
    {
      var mockEventDateRepository = GetMockedRepository(eventDateList);
      databaseContext.Configure().EventDatesRepository.Returns(mockEventDateRepository);
      databaseContext.When(x => x.RetrieveRepository<EventDate>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
    }

    protected virtual EntityFrameworkContext SetUpEventTypeRepository(EntityFrameworkContext databaseContext, IList<EventType> eventTypeList)
    {
      var mockEventTypeRepository = GetMockedRepository(eventTypeList);
      databaseContext.Configure().EventTypeRepository.Returns(mockEventTypeRepository);
      databaseContext.When(x => x.RetrieveRepository<EventType>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
    }

    protected virtual EntityFrameworkContext SetUpEmployeeRepository(EntityFrameworkContext databaseContext, IList<Employee> employeeList)
    {
      var mockEmployeeRepository = GetMockedRepository(employeeList);
      databaseContext.Configure().EmployeeRepository.Returns(mockEmployeeRepository);
      databaseContext.When(x => x.RetrieveRepository<Employee>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
    }

    protected virtual EntityFrameworkContext SetUpEventTypeDaysNoticeRepository(EntityFrameworkContext databaseContext, IList<EventTypeDaysNotice> eventTypeDaysNoticeList)
    {
      var mockEventTypeDaysNoticeRepository = GetMockedRepository(eventTypeDaysNoticeList);
      databaseContext.Configure().EventTypeDaysNoticeRepository.Returns(mockEventTypeDaysNoticeRepository);
      databaseContext.When(x => x.RetrieveRepository<EventTypeDaysNotice>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
    }
    
    protected virtual EntityFrameworkContext SetUpEventWorkflowRepository(EntityFrameworkContext databaseContext, IList<EventWorkflow> eventWorkflowList)
    {
      var mockEventWorkflowRepository = GetMockedRepository(eventWorkflowList);
      databaseContext.Configure().EventWorkflowRepository.Returns(mockEventWorkflowRepository);
      databaseContext.When(x => x.RetrieveRepository<EventWorkflow>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
    }
    
    protected virtual EntityFrameworkContext SetUpEventTypeRequiredRespondersRepository(EntityFrameworkContext databaseContext, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
    {
      var mockRepository = GetMockedRepository(eventTypeRequiredRespondersList);
      databaseContext.Configure().EventTypeRequiredRespondersRepository.Returns(mockRepository);
      databaseContext.When(x => x.RetrieveRepository<EventTypeRequiredResponders>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
    }
    
    protected virtual EntityFrameworkContext SetUpEmployeeApprovalResponseRepository(EntityFrameworkContext databaseContext, IList<EmployeeApprovalResponse> employeeApprovalResponseList)
    {
      var mockRepository = GetMockedRepository(employeeApprovalResponseList);
      databaseContext.Configure().EmployeeApprovalResponsesRepository.Returns(mockRepository);
      databaseContext.When(x => x.RetrieveRepository<EmployeeApprovalResponse>()).DoNotCallBase();

      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      return databaseContext;
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