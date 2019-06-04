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
using AutoFixture;
using Xunit;

namespace AdminCore.Services.Tests
{
  public sealed class TeamServiceTests : BaseMockedDatabaseSetUp
  {
    private readonly Fixture _fixture;

    public TeamServiceTests()
    {
      _fixture = new Fixture();
      _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => _fixture.Behaviors.Remove(b));
      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void GetByProjectId_OrmReturnsListOfTeams_RepoAndMapperReceiveCalls()
    {
      // Arrange
      var dbReturns = _fixture.CreateMany<Team>(1).ToList();
      var teamService = GetMockedResourcesGetByProjectId(dbReturns, out var ormContext, out var mapper);

      // Act
      teamService.GetByProjectId(7);

      // Assert
      ormContext.Received().TeamRepository.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      mapper.Received(1).Map<IList<TeamDto>>(Arg.Is<IList<Team>>(x => x != null && x.Count == 1));
    }

    [Fact]
    public void GetByProjectId_OrmReturnsEmptyList_RepoAndMapperReceiveCalls()
    {
      // Arrange
      var teamService = GetMockedResourcesGetByProjectId(new List<Team>(), out var ormContext, out var mapper);

      // Act
      teamService.GetByProjectId(78);

      // Assert
      ormContext.Received().TeamRepository.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      mapper.Received(1).Map<IList<TeamDto>>(Arg.Is<IList<Team>>(x => x != null && x.Count == 0));
    }

    [Fact]
    public void GetByProjectId_OrmReturnsNull_RepoAndMapperReceiveCalls()
    {
      // Arrange
      var teamService = GetMockedResourcesGetByProjectId(null, out var ormContext, out var mapper);

      // Act
      teamService.GetByProjectId(93);

      // Assert
      ormContext.Received().TeamRepository.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>());
      mapper.Received(1).Map<IList<TeamDto>>(Arg.Is<IList<Team>>(x => x != null));
    }

    private TeamService GetMockedResourcesGetByProjectId(IList<Team> dbReturns, out EntityFrameworkContext ormContext, out IMapper mapper)
    {
      ormContext = GetMockedProjectRepoGet(dbReturns);

      mapper = Substitute.For<IMapper>();
      mapper.Map<IList<TeamDto>>(Arg.Any<List<Team>>()).Returns(new List<TeamDto>());

      return new TeamService(mapper, ormContext);
    }

    private EntityFrameworkContext GetMockedProjectRepoGet(IList<Team> dbReturns)
    {
      var ormContext = Substitute.For<EntityFrameworkContext>(Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>()));
      var teamRepoMock = Substitute.For<IRepository<Team>>();
      teamRepoMock.Get(Arg.Any<Expression<Func<Team, bool>>>(),
        Arg.Any<Func<IQueryable<Team>, IOrderedQueryable<Team>>>(),
        Arg.Any<Expression<Func<Team, object>>[]>()).Returns(dbReturns);

      ormContext.TeamRepository.Returns(teamRepoMock);
      return ormContext;
    }
  }
}
