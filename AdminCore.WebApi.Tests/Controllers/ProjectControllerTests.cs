using System;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Controllers;
using AdminCore.WebApi.Models.Team;
using AutoFixture;
using AutoMapper;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AdminCore.DTOs.Project;
using AdminCore.WebApi.Models.Project;
using AdminCore.WebApi.Tests.ClassData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class ProjectControllerTests : BaseControllerTest
  {
    private readonly Fixture _fixture;
    private readonly IMapper _mapper;
    private readonly ProjectController _projectController;
    private readonly IProjectService _projectService;

    public ProjectControllerTests()
    {
      _projectService = Substitute.For<IProjectService>();
      _mapper = Substitute.For<IMapper>();
      _fixture = new Fixture();
      _projectController = new ProjectController(_projectService, _mapper);
    }

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.GetProjectById_ServiceContainsListOfTeams_ReturnsOkWithTeamsInBodyClassData))]
    public void GetProjectById_ServiceContainsListOfTeams_ReturnsOkWithTeamsInBody(int projectId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns)
    {
      // Arrange
      var projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.GetProjectsById(projectId).Returns(serviceReturns);

      var mapper = SetupMockedMapper(serviceReturns, controllerReturns);

      var teamController = new ProjectController(projectServiceMock, mapper);

      // Act
      var response = teamController.GetProjectById(projectId);

      var resultList = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);

      // Assert
      projectServiceMock.Received(1).GetProjectsById(Arg.Any<int>());
      resultList.Should().BeEquivalentTo(controllerReturns);
    }

    [Fact]
    public void GetProjectById_ServiceReturnsNoTeamsEmptyList_ReturnsNoContent()
    {
      // Arrange
      var projectServiceMock = Substitute.For<IProjectService>();

      var serviceReturns = new List<ProjectDto>();
      projectServiceMock.GetProjectsById(Arg.Any<int>()).Returns(serviceReturns);

      var mapper = SetupMockedMapper<IList<ProjectDto>, IList<ProjectViewModel>>(serviceReturns, new List<ProjectViewModel>());
      var projectController = new ProjectController(projectServiceMock, mapper);

      // Act
      var response = projectController.GetProjectById(5);

      // Assert
      projectServiceMock.Received(1).GetProjectsById(Arg.Any<int>());
      Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public void GetProjectById_ServiceReturnsNullTeams_ReturnsNoContent()
    {
      // Arrange
      var projectServiceMock = Substitute.For<IProjectService>();

      projectServiceMock.GetProjectsById(Arg.Any<int>()).Returns(x => null);

      var mapper = SetupMockedMapper<IList<TeamDto>, IList<TeamViewModel>>(null, null);
      var projectController = new ProjectController(projectServiceMock, mapper);

      // Act
      var response = projectController.GetProjectById(5);

      // Assert
      projectServiceMock.Received(1).GetProjectsById(Arg.Any<int>());
      Assert.IsType<OkObjectResult>(response);
    }
  }
}
