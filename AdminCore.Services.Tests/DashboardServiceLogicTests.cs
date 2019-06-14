using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Client;
using AdminCore.DTOs.Dashboard;
using AdminCore.Services.Mappings;
using AdminCore.Services.Tests.ClassData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using Xunit;

namespace AdminCore.Services.Tests
{
  public class DashboardServiceLogicTests : BaseMockedDatabaseSetUp
  {

    private readonly MockDatabase _mockDatabase;

    public DashboardServiceLogicTests()
    {
      _mockDatabase = new MockDatabase();
    }

    [Fact]
    public void TestEmployeeDashboardEventsLogic_Check1ResultComesBackForEmployeeId1ForDecember()
    {
      // Employee 1 has 2 December 2018 events. One Awaiting approval and one cancelled. This query should only return the one that is awaiting approved.
      var resultList = _mockDatabase.EventRepository.Where(evnt => DashboardService.EmployeeDashboardEventsQuery(1, new DateTime(2018, 12, 5), evnt)).ToList();
      Assert.Single(resultList);
      Assert.Equal((int)EventStatuses.AwaitingApproval, resultList.First().EventStatusId);
    }

    [Fact]
    public void TestEmployeeDashboardEventsLogic_Check2ResultsComesBackForEmployeeId1ForFebruary()
    {
      // Employee ID 1 has 2 events for February 2019
      var resultList = _mockDatabase.EventRepository.Where(evnt => DashboardService.EmployeeDashboardEventsQuery(1, new DateTime(2019, 02, 5), evnt)).ToList();
      Assert.Equal(2, resultList.Count);
    }

    [Fact]
    public void TestEmployeeDashboardEventsLogic_Check0ResultsComesBackForEmployeeId2ForMarch()
    {
      // Employee ID 2 has 1 event for March 2019 but no active contract during this time.
      // Query should return nothing.
      var resultList = _mockDatabase.EventRepository.Where(evnt => DashboardService.EmployeeDashboardEventsQuery(2, new DateTime(2019, 03, 5), evnt)).ToList();
      Assert.Empty(resultList);
    }

    [Fact]
    public void TestEmployeeDashboardEventsLogic_Check1ResultsComesBackForEmployeeId3ForApril()
    {
      // Employee ID 4 has 1 event in April 2019 and an open-ended (end date = null) contract starting December 2018.
      // The 1 event should be returned
      var resultList = _mockDatabase.EventRepository.Where(evnt => DashboardService.EmployeeDashboardEventsQuery(4, new DateTime(2019, 04, 5), evnt)).ToList();
      Assert.Single(resultList);
    }

    [Fact]
    public void TestGetEmployeeSnapshotByEmployeeIdLogic_Check1ResultComesBackForEmployeeId1ForDecember()
    {
      // Employee ID 1 has one contract active during december and holidays booked during this month.
      // So one contract should come back
      var resultList = _mockDatabase.ContractRepository.Where(contract => DashboardService.GetEmployeeSnapshotByEmployeeIdQuery(1, contract, new DateTime(2018, 12, 22))).ToList();
      Assert.Single(resultList);
    }

    [Fact]
    public void TestGetEmployeeSnapshotByEmployeeIdLogic_Check2ResultsComeBackForEmployeeId3ForDecember()
    {
      // Employee ID 3 has two contracts active during December and holidays booked during this month.
      // So two contracts should come back
      var resultList = _mockDatabase.ContractRepository.Where(contract => DashboardService.GetEmployeeSnapshotByEmployeeIdQuery(3, contract, new DateTime(2018, 12, 22))).ToList();
      Assert.Equal(2, resultList.Count);
    }

    [Theory]
    [ClassData(typeof(DashboardServiceClassData.DashboardEventsClassData))]
    public void GetTeamDashboardEvents_ReposReturnEvents_ValidSnapshotReturned(int employeeId, DateTime dateToGet, IList<Team> teamRepoOut, IList<Client> clientRepoOut,
      ClientSnapshotDto clientSnapshotMapOut)
    {
      // Arrange
      GetMockedResourcesGetTeamDashboardEvents(teamRepoOut, clientRepoOut, out var mapper, out var dashboardService, out var ormContext);

      // Act
      var clientSnapShotActual = dashboardService.GetTeamDashboardEvents(employeeId, dateToGet);

      // Assert
      ormContext.Received(1).TeamRepository.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      ormContext.Received(1).ClientRepository.GetAsQueryable(Arg.Any<Expression<Func<Client, bool>>>(),
        Arg.Any<Func<IQueryable<Client>, IOrderedQueryable<Client>>>(),
        Arg.Any<(Expression<Func<Client, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[]>());

      clientSnapShotActual[0].Should().BeEquivalentTo(clientSnapshotMapOut);
    }

    [Fact]
    public void GetTeamDashboardEvents_ReposReturnEmptyList_EmptySnapShotReturned()
    {
      // Arrange
      GetMockedResourcesGetTeamDashboardEvents(new List<Team>(), new List<Client>(), out var mapper, out var dashboardService, out var ormContext);

      // Act
      var clientSnapShotActual = dashboardService.GetTeamDashboardEvents(6, new DateTime(2019, 5, 6));

      // Assert
      ormContext.Received(1).TeamRepository.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      ormContext.Received(1).ClientRepository.GetAsQueryable(Arg.Any<Expression<Func<Client, bool>>>(),
        Arg.Any<Func<IQueryable<Client>, IOrderedQueryable<Client>>>(),
        Arg.Any<(Expression<Func<Client, object>> includeProperty, Expression<Func<object, object>>[] thenIncludes)[]>());

      clientSnapShotActual.Should().BeEmpty();
    }

    private void GetMockedResourcesGetTeamDashboardEvents(IList<Team> teamRepoOut, IList<Client> clientRepoOut,
      out IMapper mapper, out DashboardService dashboardService, out EntityFrameworkContext ormContext)
    {
      var databaseContext = SetupMockedOrmContext(out var dbContext);
      ormContext = SetUpGenericRepository(databaseContext, teamRepoOut,
        repository => { databaseContext.Configure().TeamRepository.Returns(repository); }, dbContext);
      ormContext = SetUpGenericRepository(databaseContext, clientRepoOut,
        repository => { databaseContext.Configure().ClientRepository.Returns(repository); }, dbContext);

      mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg =>
      {
        cfg.AddProfile(new ClientMapperProfile());
        cfg.AddProfile(new TeamMapperProfile());
        cfg.AddProfile(new DashboardMapperProfile());
      })).Configure();

      dashboardService = new DashboardService(ormContext, mapper);
    }
  }
}
