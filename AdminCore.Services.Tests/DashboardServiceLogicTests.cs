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
using AdminCore.Services.Tests.ClassData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AdminCore.Services.Tests
{
  public class DashboardServiceLogicTests
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
    public void GetTeamDashboardEvents_ReposReturnEvents_ValidSnapshotReturned(int employeeId, DateTime dateToGet, IList<Team> teamRepoOut, IQueryable<Client> clientRepoOut,
      ClientSnapshotDto clientSnapshotMapOut, ProjectSnapshotDto projectSnapshotMapOut, TeamSnapshotDto teamSnapshotMapOut, EmployeeSnapshotDto employeeSnapshotMapOut)
    {
      // Arrange
      GetMockedResourcesGetTeamDashboardEvents(teamRepoOut, clientRepoOut, clientSnapshotMapOut, projectSnapshotMapOut,
        teamSnapshotMapOut, employeeSnapshotMapOut, out var mapper, out var dashboardService, out var ormContext);

      // Act
      var clientSnapShotActual = dashboardService.GetTeamDashboardEvents(employeeId, dateToGet);

      // Assert
      ormContext.TeamRepository.Received(1).Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      ormContext.ClientRepository.Received(1).GetAsQueryable(Arg.Any<Expression<Func<Client, bool>>>(),
        Arg.Any<Func<IQueryable<Client>, IOrderedQueryable<Client>>>(),
        Arg.Any<Expression<Func<Client, object>>[]>());
      mapper.Received(1).Map<ClientSnapshotDto>(Arg.Any<Client>());
      mapper.Received(1).Map<ProjectSnapshotDto>(Arg.Any<Project>());
      mapper.Received(1).Map<TeamSnapshotDto>(Arg.Any<Team>());
      mapper.Received(1).Map<EmployeeSnapshotDto>(Arg.Any<Employee>());

      clientSnapShotActual[0].Should().BeEquivalentTo(clientSnapshotMapOut);
    }

    [Fact]
    public void GetTeamDashboardEvents_ReposReturnEmptyList_EmptySnapShotReturned()
    {
      // Arrange
      GetMockedResourcesGetTeamDashboardEvents(new List<Team>(), new EnumerableQuery<Client>(new List<Client>()), new ClientSnapshotDto(), new ProjectSnapshotDto(),
        new TeamSnapshotDto(), new EmployeeSnapshotDto(), out var mapper, out var dashboardService, out var ormContext);

      // Act
      var clientSnapShotActual = dashboardService.GetTeamDashboardEvents(6, new DateTime(2019, 5, 6));

      // Assert
      ormContext.TeamRepository.Received(1).Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      ormContext.ClientRepository.Received(1).GetAsQueryable(Arg.Any<Expression<Func<Client, bool>>>(),
        Arg.Any<Func<IQueryable<Client>, IOrderedQueryable<Client>>>(),
        Arg.Any<Expression<Func<Client, object>>[]>());

      clientSnapShotActual.Should().BeEmpty();
    }

    [Fact]
    public void GetTeamDashboardEvents_ReposReturnNull_EmptySnapShotReturned()
    {
      // Arrange
      GetMockedResourcesGetTeamDashboardEvents(null, null, null, null,
        null, null, out var mapper, out var dashboardService, out var ormContext);

      // Act
      var clientSnapShotActual = dashboardService.GetTeamDashboardEvents(6, new DateTime(2019, 5, 6));

      // Assert
      ormContext.TeamRepository.Received(1).Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      ormContext.ClientRepository.Received(1).GetAsQueryable(Arg.Any<Expression<Func<Client, bool>>>(),
        Arg.Any<Func<IQueryable<Client>, IOrderedQueryable<Client>>>(),
        Arg.Any<Expression<Func<Client, object>>[]>());

      clientSnapShotActual.Should().BeEmpty();
    }

    private void GetMockedResourcesGetTeamDashboardEvents(IList<Team> teamRepoOut, IQueryable<Client> clientRepoOut,
      ClientSnapshotDto clientSnapshotMapOut, ProjectSnapshotDto projectSnapshotMapOut, TeamSnapshotDto teamSnapshotMapOut, EmployeeSnapshotDto employeeSnapshotMapOut,
      out IMapper mapper, out DashboardService dashboardService, out EntityFrameworkContext ormContext)
    {
      ormContext = GetMockedOrmContext();
      ormContext = GetMockedTeamRepoGet(ormContext, teamRepoOut);
      ormContext = GetMockedClientRepoGetQueryable(ormContext, clientRepoOut);

      mapper = Substitute.For<IMapper>();
      mapper.Map<ClientSnapshotDto>(Arg.Any<Client>()).Returns(clientSnapshotMapOut);
      mapper.Map<ProjectSnapshotDto>(Arg.Any<Project>()).Returns(projectSnapshotMapOut);
      mapper.Map<TeamSnapshotDto>(Arg.Any<Team>()).Returns(teamSnapshotMapOut);
      mapper.Map<EmployeeSnapshotDto>(Arg.Any<Employee>()).Returns(employeeSnapshotMapOut);

      dashboardService = new DashboardService(ormContext, mapper);
    }

    private EntityFrameworkContext GetMockedOrmContext()
    {
      return Substitute.For<EntityFrameworkContext>(Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>()));
    }

    private EntityFrameworkContext GetMockedTeamRepoGet(EntityFrameworkContext ormContext, IList<Team> dbReturns)
    {
      var teamRepoMock = Substitute.For<IRepository<Team>>();
      teamRepoMock.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>()).Returns(dbReturns);

      ormContext.TeamRepository.Returns(teamRepoMock);
      return ormContext;
    }

    private EntityFrameworkContext GetMockedClientRepoGetQueryable(EntityFrameworkContext ormContext, IQueryable<Client> dbReturns)
    {
      var clientRepoMock = Substitute.For<IRepository<Client>>();
      clientRepoMock.GetAsQueryable(Arg.Any<Expression<Func<Client, bool>>>(),
        Arg.Any<Func<IQueryable<Client>, IOrderedQueryable<Client>>>(),
        Arg.Any<Expression<Func<Client, object>>[]>()).Returns(dbReturns);

      ormContext.ClientRepository.Returns(clientRepoMock);
      return ormContext;
    }
  }
}
