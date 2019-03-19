using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using AdminCore.Services.Mappings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DAL.Database;
using Xunit;

namespace AdminCore.Services.Tests
{
  public class UnitTest1
  {
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

    //protected DbSet<T> SetupRepositoryForContext<T>(IList<T> sourceList) where T : class
    //{
    //  var dbSet = GetQueryableMockDbSet(sourceList);
    //  ((EntityFrameworkContext)DatabaseContext).Set<T>().Returns(x => dbSet);
    //  DatabaseContext.Attach(Arg.Any<T>()).Returns(x => (T)x[0]);

    //  dbSet.Include(Arg.Any<string>()).Returns(dbSet);

    //  return dbSet;
    //}
    [Fact]
    public void Test1()
    {
      const int eventId = 1;

      var eventDate = new EventDate
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05)
      };

      var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new EventMapperProfile())));

      var databaseContext = Substitute.For<IDatabaseContext, EntityFrameworkContext>();
      var dateService = Substitute.For<IDateService>();

      ((EntityFrameworkContext)databaseContext).Set<EventDate>()
        .Returns(GetQueryableMockDbSet(new List<EventDate> { eventDate }));

      var eventRepository = new EntityFrameworkRepository<Event>(databaseContext);

      var eventService = new EventService(databaseContext, mapper, dateService);

      databaseContext.EventRepository.Returns(eventRepository);

      // eventRepository.GetSingle()

      // Assert
      var eventDateDto = eventService.GetEvent(1);

      Mock<IRepository<EventDate>> mockEventDateRepository = new Mock<IRepository<EventDate>>();
      mockEventDateRepository.Setup(m => m.GetSingle(x => x.EventDateId == 1)).Returns(eventDate);
    }
  }
}