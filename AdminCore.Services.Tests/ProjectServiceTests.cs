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
    private void GetProjects_DatabaseContextContainsProjectsWithId_ReturnsListOfProjects(int projectId, IList<Project> dbReturns, IList<ProjectDto> serviceReturns)
    {
      // Arrange
      GetMockedResourcesGetProjects(dbReturns, serviceReturns, out var projectService, out var databaseContext);

      // Act
      var projectDtoList = projectService.GetProjects();

      // Assert
      databaseContext.Received(1).ProjectRepository.Get();
      projectDtoList.Should().BeEquivalentTo(serviceReturns);
    }

    [Fact]
    private void GetProjects_DatabaseContextReturnsNull_ReturnsEmptyList()
    {
      // Arrange
      GetMockedResourcesGetProjects(null, new List<ProjectDto>(), out var projectService, out var databaseContext);

      // Act
      var projectDtoList = projectService.GetProjects();

      // Assert
      databaseContext.Received(1).ProjectRepository.Get();
      projectDtoList.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    [Fact]
    private void GetProjects_DatabaseContextEncountersException_ReturnsEmptyList()
    {
      // Arrange
      var databaseContext = Substitute.For<EntityFrameworkContext>(Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>()));
      databaseContext.ProjectRepository.Throws<Exception>();

      var projectService = new ProjectService(null, databaseContext);

      // Act
      var projectDtoList = projectService.GetProjects();

      // Assert
      databaseContext.Received(1).ProjectRepository.Get();
      projectDtoList.Should().BeEquivalentTo(new List<ProjectDto>());
    }

    #endregion

    #region GetProjectsById



    #endregion

    #region GetProjectsByClientId



    #endregion

    #region CreateProject



    #endregion

    #region UpdateProject



    #endregion

    #region DeleteProject



    #endregion

    #region MockCreation

    private void GetMockedResourcesGetProjects(IList<Project> dbReturns, IList<ProjectDto> serviceReturns,
      out ProjectService projectService, out EntityFrameworkContext databaseContext)
    {
      databaseContext = Substitute.For<EntityFrameworkContext>(Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>()));
      var projectRepoMock = Substitute.For<IRepository<Project>>();
      projectRepoMock.Get(Arg.Any<Expression<Func<Project, bool>>>(),
        Arg.Any<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
        Arg.Any<Expression<Func<Project, object>>[]>()).Returns(dbReturns);

      databaseContext.ProjectRepository.Returns(projectRepoMock);

      var mapper = Substitute.For<IMapper>();
      mapper.Map<IList<ProjectDto>>(dbReturns).Returns(serviceReturns);

      projectService = new ProjectService(mapper, databaseContext);
    }

    #endregion
  }
}
