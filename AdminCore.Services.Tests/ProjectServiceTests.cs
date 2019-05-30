using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.Project;
using AdminCore.DTOs.Team;
using AdminCore.Services.Mappings;
using AdminCore.Services.Tests.ClassData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace AdminCore.Services.Tests
{
  public class ProjectServiceTests : BaseMockedDatabaseSetUp
  {
    #region GetProjects

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.RandomProjectIdProjectsClassData))]
    private void GetProjects_RepositoryReturnsListOfProjectsWithId_ReturnsListOfProjects(int projectId, IList<Project> dbReturns, IList<ProjectDto> serviceReturns)
    {
      // Arrange
      GetMockedResourcesGetProjects(dbReturns, serviceReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      projectService.GetProjects();

      // Assert
      ormContext.Received(1).ProjectRepository.Get();
      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Any<IList<Project>>());
    }

    [Fact]
    private void GetProjects_DatabaseContextReturnsNull_ReturnsEmptyList()
    {
      // Arrange
      var ormContext = GetMockedProjectRepoGet(null);

      var mapper = Substitute.For<IMapper>();
      mapper.Map<IList<ProjectDto>>(new List<Project>()).Returns(new List<ProjectDto>());

      var projectService = new ProjectService(mapper, ormContext);

      // Act
      var projectDtoList = projectService.GetProjects();

      // Assert
      ormContext.Received(1).ProjectRepository.Get();
      mapper.Received(0).Map<IList<ProjectDto>>(null);
      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Any<IList<Project>>());
      projectDtoList.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    #endregion

    #region GetProjectsById

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.RandomProjectIdProjectsClassData))]
    private void GetProjectsById_RepositoryReturnsListOfProjectsWithId_ReturnsListOfProjects(int projectId, IList<Project> dbReturns, IList<ProjectDto> serviceReturns)
    {
      // Arrange
      GetMockedResourcesGetProjects(dbReturns, serviceReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      projectService.GetProjectsById(projectId);

      // Assert
      ormContext.Received(1).ProjectRepository.Get();
      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Any<IList<Project>>());
    }

    [Fact]
    private void GetProjectsById_DatabaseContextReturnsNull_ReturnsEmptyList()
    {
      // Arrange
      var ormContext = GetMockedProjectRepoGet(null);

      var mapper = Substitute.For<IMapper>();
      mapper.Map<IList<ProjectDto>>(new List<Project>()).Returns(new List<ProjectDto>());

      var projectService = new ProjectService(mapper, ormContext);

      // Act
      var projectDtoList = projectService.GetProjectsById(6);

      // Assert
      ormContext.Received(1).ProjectRepository.Get();
      mapper.Received(0).Map<IList<ProjectDto>>(null);
      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Any<IList<Project>>());
      projectDtoList.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    #endregion

    #region GetProjectsByClientId

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.RandomProjectIdProjectsClassData))]
    private void GetProjectsByClientId_RepositoryReturnsListOfProjectsWithId_ReturnsListOfProjects(int projectId, IList<Project> dbReturns, IList<ProjectDto> serviceReturns)
    {
      // Arrange
      GetMockedResourcesGetProjects(dbReturns, serviceReturns, out var mapper, out var projectService, out var ormContext);

      // Act
      projectService.GetProjectsByClientId(projectId);

      // Assert
      ormContext.Received(1).ProjectRepository.Get();
      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Any<IList<Project>>());
    }

    [Fact]
    private void GetProjectsByClientId_DatabaseContextReturnsNull_ReturnsEmptyList()
    {
      // Arrange
      var ormContext = GetMockedProjectRepoGet(null);

      var mapper = Substitute.For<IMapper>();
      mapper.Map<IList<ProjectDto>>(new List<Project>()).Returns(new List<ProjectDto>());

      var projectService = new ProjectService(mapper, ormContext);

      // Act
      var projectDtoList = projectService.GetProjectsByClientId(3987);

      // Assert
      ormContext.Received(1).ProjectRepository.Get();
      mapper.Received(0).Map<IList<ProjectDto>>(null);
      mapper.Received(1).Map<IList<ProjectDto>>(Arg.Any<IList<Project>>());
      projectDtoList.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    #endregion

    #region CreateProject

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.InsertUpdateProjectRandomProjectDtoProject))]
    private void CreateProject_ValidProjectDto_ReturnsSavedProjectDto(ProjectDto projectToCreate, Project dbOut)
    {
      // Arrange
      GetMockedResourcesCreateProject(projectToCreate, dbOut, dbOut, out var mapper, out var projectService, out var ormContext);

      // Act
      projectService.CreateProject(projectToCreate);

      // Assert
      ormContext.Received(1).ProjectRepository.Insert(Arg.Any<Project>());
      ormContext.Received(1).SaveChanges();
      mapper.Received(1).Map<ProjectDto>(Arg.Any<Project>());
      mapper.Received(1).Map<Project>(Arg.Any<ProjectDto>());
    }

    #endregion

    #region UpdateProject

    [Theory]
    [ClassData(typeof(ProjectServiceClassData.InsertUpdateProjectRandomProjectDtoProject))]
    private void UpdateProject_ValidProjectDto_ReturnsUpdatedProjectDto(ProjectDto projectToCreate, Project dbOut)
    {
      // Arrange
      GetMockedResourcesUpdateProject(projectToCreate, dbOut, dbOut, out var mapper, out var projectService, out var ormContext);

      // Act
      projectService.UpdateProject(projectToCreate);

      // Assert
      ormContext.Received(1).ProjectRepository.Update(Arg.Any<Project>());
      ormContext.Received(1).SaveChanges();
      mapper.Received(1).Map<ProjectDto>(Arg.Any<Project>());
      mapper.Received(1).Map<Project>(Arg.Any<ProjectDto>());
    }

    #endregion

    #region DeleteProject

    [Fact]
    private void DeleteProject_ValidProjectDto_ReturnsUpdatedProjectDto()
    {
      // Arrange
      GetMockedResourcesDeleteProject(out _, out var projectService, out var ormContext);

      // Act
      projectService.DeleteProject(892);

      // Assert
      ormContext.Received(1).ProjectRepository.Delete(Arg.Any<int>());
      ormContext.Received(1).SaveChanges();
    }

    #endregion

    #region MockCreation

    private void GetMockedResourcesGetProjects(IList<Project> dbReturns, IList<ProjectDto> serviceReturns,
      out IMapper mapper, out ProjectService projectService, out EntityFrameworkContext ormContext)
    {
      ormContext = GetMockedProjectRepoGet(dbReturns);

      mapper = Substitute.For<IMapper>();
      mapper.Map<IList<ProjectDto>>(Arg.Any<IList<Project>>()).Returns(serviceReturns);

      projectService = new ProjectService(mapper, ormContext);
    }

    private void GetMockedResourcesCreateProject(ProjectDto serviceOut, Project dbIn, Project dbOut,
      out IMapper mapper, out ProjectService projectService, out EntityFrameworkContext ormContext)
    {
      ormContext = GetMockedProjectRepoInsert(dbOut);

      mapper = Substitute.For<IMapper>();
      mapper.Map<Project>(Arg.Any<ProjectDto>()).Returns(dbIn);
      mapper.Map<ProjectDto>(Arg.Any<Project>()).Returns(serviceOut);

      projectService = new ProjectService(mapper, ormContext);
    }

    private void GetMockedResourcesUpdateProject(ProjectDto serviceOut, Project dbIn, Project dbOut,
      out IMapper mapper, out ProjectService projectService, out EntityFrameworkContext ormContext)
    {
      ormContext = GetMockedProjectRepoUpdate(dbOut);

      mapper = Substitute.For<IMapper>();
      mapper.Map<Project>(Arg.Any<ProjectDto>()).Returns(dbIn);
      mapper.Map<ProjectDto>(Arg.Any<Project>()).Returns(serviceOut);

      projectService = new ProjectService(mapper, ormContext);
    }

    private void GetMockedResourcesDeleteProject(out IMapper mapper, out ProjectService projectService, out EntityFrameworkContext ormContext)
    {
      ormContext = GetMockedProjectRepoDelete();

      mapper = Substitute.For<IMapper>();

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

    private EntityFrameworkContext GetMockedProjectRepoInsert(Project dbOut)
    {
      var ormContext = Substitute.For<EntityFrameworkContext>(Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>()));
      var projectRepoMock = Substitute.For<IRepository<Project>>();
      projectRepoMock.Insert(Arg.Any<Project>()).Returns(dbOut);

      ormContext.ProjectRepository.Returns(projectRepoMock);
      return ormContext;
    }

    private EntityFrameworkContext GetMockedProjectRepoUpdate(Project dbOut)
    {
      var ormContext = Substitute.For<EntityFrameworkContext>(Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>()));
      var projectRepoMock = Substitute.For<IRepository<Project>>();
      projectRepoMock.Update(Arg.Any<Project>()).Returns(dbOut);

      ormContext.ProjectRepository.Returns(projectRepoMock);
      return ormContext;
    }

    private EntityFrameworkContext GetMockedProjectRepoDelete()
    {
      var ormContext = Substitute.For<EntityFrameworkContext>(Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>()));
      var projectRepoMock = Substitute.For<IRepository<Project>>();
      projectRepoMock.When(x => x.Delete(Arg.Any<int>())).Do(x => { });

      ormContext.ProjectRepository.Returns(projectRepoMock);
      return ormContext;
    }

    #endregion
  }
}
