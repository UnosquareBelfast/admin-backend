using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Event;
using AdminCore.Services.Mappings;
using AutoMapper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AdminCore.DTOs.Team;
using AdminCore.Services.Tests.ClassData;
using AutoFixture;
using FluentAssertions;
using NSubstitute.Extensions;
using Xunit;

namespace AdminCore.Services.Tests
{
  public sealed class TeamServiceTests : BaseMockedDatabaseSetUp
  {
    [Theory]
    [ClassData(typeof(TeamServiceClassData.TeamTeamDtosWithProjectIdClassData))]
    public void GetByProjectId_DbReturnsListOfTeams_RepoAndMapperReceiveCalls(int projectId, List<Team> dbReturns, List<TeamDto> serviceExpected)
    {
      // Arrange
      var teamService = GetMockedResourcesGetByProjectId(dbReturns, out var ormContext, out var mapper);

      // Act
      var serviceActual = teamService.GetByProjectId(projectId);

      // Assert
      ormContext.Received(1).TeamRepository.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      serviceActual.Should().BeEquivalentTo(serviceExpected);
    }

    [Fact]
    public void GetByProjectId_DbReturnsEmptyList_RepoAndMapperReceiveCalls()
    {
      // Arrange
      var teamService = GetMockedResourcesGetByProjectId(new List<Team>(), out var ormContext, out var mapper);

      // Act
      var serviceActual = teamService.GetByProjectId(78);

      // Assert
      ormContext.Received(1).TeamRepository.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      serviceActual.Should().BeEquivalentTo(new List<TeamDto>());
    }

    private TeamService GetMockedResourcesGetByProjectId(IList<Team> dbReturns, out EntityFrameworkContext ormContext, out IMapper mapper)
    {
      var databaseContext = SetupMockedOrmContext(out var dbContext);
      ormContext = SetUpGenericRepository(databaseContext, dbReturns,
        repository => { databaseContext.Configure().TeamRepository.Returns(repository); }, dbContext);

      mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg => cfg.AddProfile(new TeamMapperProfile()))).Configure();

      return new TeamService(mapper, ormContext);
    }
  }
}
