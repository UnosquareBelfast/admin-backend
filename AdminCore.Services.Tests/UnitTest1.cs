using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DAL;
using Xunit;

namespace AdminCore.Services.Tests
{
  public class UnitTest1 : BaseContextSetUp
  {
    [Fact]
    public void Test1()
    {
      const int eventId = 1;
      const int eventDateId = 1;

      var eventDate = new EventDate
      {
        EventDateId = eventDateId,
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05)
      };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>();
      var eventDates = GetQueryableMockDbSet(new List<EventDate> { eventDate });

      databaseContext.When(x => x.Set<EventDate>()).DoNotCallBase();
      databaseContext.Set<EventDate>().Returns(eventDates);

      var eventDateRepository = new EntityFrameworkRepository<EventDate>(databaseContext);
      IRepository<EventDate> eventDateRepo = eventDateRepository;
      databaseContext.EventDatesRepository.Returns(eventDateRepo);

      var resultOne = databaseContext.EventDatesRepository.GetSingle(x => x.EventDateId == eventId);
      var resultTne = databaseContext.EventDatesRepository.GetSingle(x => x.EventDateId == 0);

      Assert.NotNull(resultOne);
      Assert.Null(resultTne);
    }

    protected new static DbSet<T> GetQueryableMockDbSet<T>(IList<T> sourceList) where T : class
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