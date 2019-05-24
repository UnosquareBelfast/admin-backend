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
using AutoFixture;
using Xunit;

namespace AdminCore.Services.Tests
{
  public sealed class TeamServiceTests : BaseMockedDatabaseSetUp
  {
    private static readonly IMapper Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new TeamMapperProfile())));

    private static readonly IConfiguration Configuration = Substitute.For<IConfiguration>();
    private static readonly AdminCoreContext AdminCoreContext = Substitute.For<AdminCoreContext>(Configuration);

    private readonly Fixture _fixture;

    public TeamServiceTests()
    {
      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();

      _fixture = new Fixture();
      _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => _fixture.Behaviors.Remove(b));
      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(65)]
    public void GetByProjectId_DatabaseContainsOneTeamWithProjectId_ReturnsOneProject(int projectIdExpected)
    {
      var teamExpectedFixture = _fixture.Create<Team>();
      teamExpectedFixture.ProjectId = projectIdExpected;
      var teamListFixture = new List<Team>{ teamExpectedFixture };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpTeamRepository(databaseContext, teamListFixture);

      var teamService = new TeamService(Mapper, databaseContext);

      // Act
      var teamActual = teamService.GetByProjectId(projectIdExpected);

      // Assert
      databaseContext.Received().TeamRepository.Get();
      Assert.Equal(teamActual.Count, 1);
      Assert.All(teamActual, dto => Assert.Equal(projectIdExpected, dto.ProjectId));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(65)]
    public void GetByProjectId_DatabaseContainsMultipleTeamsOneWithProjectId_ReturnsOneProject(int projectIdExpected)
    {
      var teamListFixture = new List<Team>();
      var rand = new Random();
      _fixture.AddManyTo(teamListFixture, () => CreateNewTeamWithRandomProjectIdExcludingSpecified(rand, projectIdExpected));

      teamListFixture.Add(CreateNewTeamFixtureWithSpecifiedProjectId(projectIdExpected));

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpTeamRepository(databaseContext, teamListFixture);

      var teamService = new TeamService(Mapper, databaseContext);

      // Act
      var teamActual = teamService.GetByProjectId(projectIdExpected);

      // Assert
      databaseContext.Received().TeamRepository.Get();
      Assert.Equal(teamActual.Count, 1);
      Assert.All(teamActual, dto => Assert.Equal(projectIdExpected, dto.ProjectId));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(65)]
    public void GetByProjectId_DatabaseContainsMultipleTeamsTwoWithProjectId_ReturnsTwoProjects(int projectIdExpected)
    {
      var teamListFixture = new List<Team>();
      var rand = new Random();
      _fixture.AddManyTo(teamListFixture, () => CreateNewTeamWithRandomProjectIdExcludingSpecified(rand, projectIdExpected));

      teamListFixture.Add(CreateNewTeamFixtureWithSpecifiedProjectId(projectIdExpected));
      teamListFixture.Add(CreateNewTeamFixtureWithSpecifiedProjectId(projectIdExpected));

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpTeamRepository(databaseContext, teamListFixture);

      var teamService = new TeamService(Mapper, databaseContext);

      // Act
      var teamActual = teamService.GetByProjectId(projectIdExpected);

      // Assert
      databaseContext.Received().TeamRepository.Get();
      Assert.Equal(teamActual.Count, 2);
      Assert.All(teamActual, dto => Assert.Equal(projectIdExpected, dto.ProjectId));
    }

    [Fact]
    public void GetByProjectId_DatabaseContainsMultipleTeamsNoneWithProjectId_ReturnsEmptyList()
    {
      int projectIdNotPresentInDb = 5;

      var teamListFixture = new List<Team>();
      var rand = new Random();
      _fixture.AddManyTo(teamListFixture, () => CreateNewTeamWithRandomProjectIdExcludingSpecified(rand, projectIdNotPresentInDb));

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpTeamRepository(databaseContext, teamListFixture);

      var teamService = new TeamService(Mapper, databaseContext);

      // Act
      var teamActual = teamService.GetByProjectId(projectIdNotPresentInDb);

      // Assert
      databaseContext.Received().TeamRepository.Get();
      Assert.Equal(teamActual.Count, 0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(65)]
    public void GetByProjectId_DatabaseContainsZeroTeamsWithProjectId_ReturnsEmptyList(int projectId)
    {
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpTeamRepository(databaseContext, new List<Team>());

      var teamService = new TeamService(Mapper, databaseContext);

      // Act
      var teamActual = teamService.GetByProjectId(projectId);

      // Assert
      databaseContext.Received().TeamRepository.Get();
      Assert.Empty(teamActual);
    }

    private Team CreateNewTeamFixtureWithSpecifiedProjectId(int projectId)
    {
      var teamFixture = _fixture.Create<Team>();
      teamFixture.ProjectId = projectId;

      return teamFixture;
    }

    private Team CreateNewTeamWithRandomProjectIdExcludingSpecified(Random rand, int excludeProjectId)
    {
      var randInt = rand.Next();
      return new Team
      {
        ProjectId = randInt != excludeProjectId ? randInt : randInt + 1
      };
    }
  }
}
