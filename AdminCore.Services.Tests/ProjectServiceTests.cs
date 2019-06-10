using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Project;
using AdminCore.Services.Mappings;
using AdminCore.Services.Tests.ClassData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using Xunit;

namespace AdminCore.Services.Tests
{
  public class ProjectServiceTests : BaseMockedDatabaseSetUp
  {
    #region GetProjects

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.GetAllRandomProjectIdProjectsClassData))]
    private void GetProjects_RepositoryReturnsListOfAllProjects_ReturnsListOfProjectsWithExpectedCountAndCallsReceived(IList<Project> dbReturns,
      IList<ProjectDto> serviceExpected)
    {
      // Arrange
      GetMockedResourcesProjectRepo(dbReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      var serviceActual = projectService.GetProjects();

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == expectedReturnCount));
      serviceActual.Should().BeEquivalentTo(serviceExpected);
    }

    [Fact]
    private void GetProjects_RepositoryReturnsEmptyList_ReturnsEmptyListAndCallsReceived()
    {
      // Arrange
      GetMockedResourcesProjectRepo(new List<Project>(), out var mapper, out var projectService, out var ormContext);

      // Act
      var serviceActual = projectService.GetProjects();

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == 0));
      serviceActual.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    #endregion

    #region GetProjectsById

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.GetByProjectIdRandomProjectIdProjectsClassData))]
    private void GetProjectsById_RepositoryReturnsListOfProjectsWithId_ReturnsListOfProjectsWithExpectedCountAndCallsReceived(int projectId, IList<Project> dbReturns,
      IList<ProjectDto> serviceExpected)
    {
      // Arrange
      GetMockedResourcesProjectRepo(dbReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      var serviceActual = projectService.GetProjects(projectId);

      // Assert
//      ormContext.Received(1).ProjectRepository.Get(x => x.ProjectId == projectId,
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == expectedReturnCount));
      serviceActual.Should().BeEquivalentTo(serviceExpected);
    }

    [Fact]
    private void GetProjectsById_DatabaseContextReturnsEmptyList_ReturnsEmptyListAndCallsReceived()
    {
      // Arrange
      GetMockedResourcesProjectRepo(new List<Project>(), out var mapper, out var projectService, out var ormContext);

      // Act
      var serviceActual = projectService.GetProjects(6);

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == 0));
      serviceActual.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    [Fact]
    private void GetProjectsById_DatabaseContextReturnsNull_ReturnsEmptyListAndCallsReceived()
    {
      // Arrange
      var ormContext = GetMockedProjectRepoGet(null);

      var mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg => cfg.AddProfile(new ProjectMapperProfile()))).Configure();

      var projectService = new ProjectService(mapper, ormContext);

      // Act
      var serviceActual = projectService.GetProjects(6);

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == 0));
      serviceActual.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    #endregion

    #region GetProjectsByClientId

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.GetByClientIdRandomProjectIdProjectsClassData))]
    private void GetProjectsByClientId_RepositoryReturnsListOfProjectsWithClientId_ReturnsListOfProjectsWithExpectedCountAndCallsReceived(int clientId, IList<Project> dbReturns,
      IList<ProjectDto> serviceExpected)
    {
      // Arrange
      GetMockedResourcesProjectRepo(dbReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      var serviceActual = projectService.GetProjects(null, clientId);

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == expectedReturnCount));
      serviceActual.Should().BeEquivalentTo(serviceExpected);
    }

    [Fact]
    private void GetProjectsByClientId_DatabaseContextReturnsEmptyList_ReturnsEmptyListAndCallsReceived()
    {
      // Arrange
      GetMockedResourcesProjectRepo(new List<Project>(), out var mapper, out var projectService, out var ormContext);

      // Act
      var serviceActual = projectService.GetProjects(null, 3262);

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == 0));
      serviceActual.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    [Fact]
    private void GetProjectsByClientId_DatabaseContextReturnsNull_ReturnsEmptyListAndCallsReceived()
    {
      // Arrange
      var ormContext = GetMockedProjectRepoGet(null);

      var mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg => cfg.AddProfile(new ProjectMapperProfile()))).Configure();

      var projectService = new ProjectService(mapper, ormContext);

      // Act
      var serviceActual = projectService.GetProjects(null, 6);

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == 0));
      serviceActual.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    #endregion

    #region GetProjectsByProjectIdClientId

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.GetByProjectIdClientIdRandomProjectsClassData))]
    private void GetProjectsByProjectIdClientId_RepositoryReturnsListOfProjectsWithClientId_ReturnsListOfProjectsWithExpectedCountAndCallsReceived(int projectId, int clientId, IList<Project> dbReturns,
      IList<ProjectDto> serviceExpected)
    {
      // Arrange
      GetMockedResourcesProjectRepo(dbReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      var serviceActual = projectService.GetProjects(projectId, clientId);

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == expectedReturnCount));
      serviceActual.Should().BeEquivalentTo(serviceExpected);
    }

    [Fact]
    private void GetProjectsByProjectIdClientId_DatabaseContextReturnsEmptyList_ReturnsEmptyListAndCallsReceived()
    {
      // Arrange
      GetMockedResourcesProjectRepo(new List<Project>(), out var mapper, out var projectService, out var ormContext);

      // Act
      var serviceActual = projectService.GetProjects(12, 3262);

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == 0));
      serviceActual.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    [Fact]
    private void GetProjectsByProjectIdClientId_DatabaseContextReturnsNull_ReturnsEmptyListAndCallsReceived()
    {
      // Arrange
      var ormContext = GetMockedProjectRepoGet(null);

      var mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg => cfg.AddProfile(new ProjectMapperProfile()))).Configure();

      var projectService = new ProjectService(mapper, ormContext);

      // Act
      var serviceActual = projectService.GetProjects(21, 6);

      // Assert
      ormContext.Received(1).ProjectRepository.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>());
//      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Is<IList<Project>>(x => x != null && x.Count == 0));
      serviceActual.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    #endregion

    #region CreateProject

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.InsertUpdateProjectRandomProjectDtoProject))]
    private void CreateProject_ValidProjectDto_AllCallsReceived(ProjectDto projectToCreate, Project dbReturns)
    {
      // Arrange
      GetMockedResourcesProjectRepo(dbReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      projectService.CreateProject(projectToCreate);

      // Assert
      ormContext.Received(1).ProjectRepository.Insert(Arg.Is<Project>(x => x.ProjectId == projectToCreate.ProjectId));
      ormContext.Received(1).SaveChanges();
//      mapper.Received(1).Map<ProjectDto>(Arg.Any<Project>());
//      mapper.Received(1).Map<Project>(Arg.Any<ProjectDto>());
    }

    #endregion

    #region UpdateProject

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.InsertUpdateProjectRandomProjectDtoProject))]
    private void UpdateProject_ValidProjectDto_AllCallsReceived(ProjectDto projectToUpdate, Project dbReturns)
    {
      // Arrange
      GetMockedResourcesProjectRepo(dbReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      projectService.UpdateProject(projectToUpdate);

      // Assert
      ormContext.Received(1).ProjectRepository.Update(Arg.Is<Project>(x => x.ProjectId == projectToUpdate.ProjectId));
      ormContext.Received(1).SaveChanges();
//      mapper.Received(1).Map<ProjectDto>(Arg.Any<Project>());
//      mapper.Received(1).Map<Project>(Arg.Any<ProjectDto>());
    }

    #endregion

    #region DeleteProject

    [Fact]
    private void DeleteProject_ValidProjectDto_AllCallsReceived()
    {
      // Arrange
      GetMockedResourcesProjectRepo(new List<Project>{new Project{ProjectId = 892}}, out var mapper, out var projectService, out var ormContext);

      // Act
      projectService.DeleteProject(892);

      // Assert
      ormContext.Received(1).ProjectRepository.Delete(892);
      ormContext.Received(1).SaveChanges();
    }

    #endregion

    #region MockCreation

    private void GetMockedResourcesProjectRepo(Project dbReturns,
      out IMapper mapper, out ProjectService projectService, out EntityFrameworkContext ormContext)
    {
      GetMockedResourcesProjectRepo(new List<Project>{dbReturns}, out mapper, out projectService, out ormContext);
    }

    private void GetMockedResourcesProjectRepo(IList<Project> dbReturns,
      out IMapper mapper, out ProjectService projectService, out EntityFrameworkContext ormContext)
    {
      var databaseContext = SetupMockedOrmContext(out var dbContext);
      ormContext = SetUpGenericRepository(databaseContext, dbReturns,
        repository => { databaseContext.Configure().ProjectRepository.Returns(repository); }, dbContext);

      mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg =>
      {
        cfg.AddProfile(new ProjectMapperProfile());
        cfg.AddProfile(new ClientMapperProfile());
        cfg.AddProfile(new TeamMapperProfile());
      })).Configure();

      projectService = new ProjectService(mapper, ormContext);
    }

    private EntityFrameworkContext GetMockedProjectRepoGet(IList<Project> dbReturns)
    {
      var ormContext = Substitute.For<EntityFrameworkContext>(Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>()));
      var projectRepoMock = Substitute.For<IRepository<Project>>();
      projectRepoMock.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>()).Returns(dbReturns);

      ormContext.ProjectRepository.Returns(projectRepoMock);
      return ormContext;
    }

    #endregion
  }
}
