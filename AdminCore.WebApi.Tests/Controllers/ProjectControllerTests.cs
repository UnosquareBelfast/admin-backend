﻿using System;
using AdminCore.Common.Interfaces;
using AdminCore.WebApi.Controllers;
using NSubstitute;
using System.Collections.Generic;
using System.Net;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Project;
using AdminCore.WebApi.Models.Project;
using AdminCore.WebApi.Tests.ClassData;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class ProjectControllerTests : BaseControllerTest
  {

    #region GetProjects

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfSpecificIdProjectDtosViewModelsClassData))]
    public void GetProjects_ServiceContainsListOfProjects_ReturnsOkWithProjectsInBody(int projectId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns)
    {
      // Arrange
      GetMockedResourcesGetProjects(serviceReturns, controllerReturns, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjects();

      var resultList = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);

      // Assert
      projectServiceMock.Received(1).GetProjects();
      resultList.Should().BeEquivalentTo(controllerReturns);
    }

    [Fact]
    public void GetProjects_ServiceReturnsEmptyList_ReturnsOk()
    {
      // Arrange
      GetMockedResourcesGetProjects(new List<ProjectDto>(), new List<ProjectViewModel>(), out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjects();

      // Assert
      projectServiceMock.Received(1).GetProjects();
      Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public void GetProjects_ServiceReturnsNull_ReturnsOk()
    {
      // Arrange
      GetMockedResourcesGetProjects(null, null, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjects();

      // Assert
      projectServiceMock.Received(1).GetProjects();
      Assert.IsType<OkObjectResult>(response);
    }

    #endregion

    #region GetProjectsById

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfSpecificIdProjectDtosViewModelsClassData))]
    public void GetProjectsById_ServiceContainsListOfProjects_ReturnsOkWithProjectsInBody(int projectId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns)
    {
      // Arrange
      GetMockedResourcesGetProjectsById(projectId, serviceReturns, controllerReturns, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjectsById(projectId);

      var resultList = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);

      // Assert
      projectServiceMock.Received(1).GetProjectsById(Arg.Any<int>());
      resultList.Should().BeEquivalentTo(controllerReturns);
    }

    [Fact]
    public void GetProjectsById_ServiceReturnsEmptyList_ReturnsOk()
    {
      // Arrange
      GetMockedResourcesGetProjectsById(5, new List<ProjectDto>(), new List<ProjectViewModel>(), out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjectsById(5);

      // Assert
      projectServiceMock.Received(1).GetProjectsById(Arg.Any<int>());
      Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public void GetProjectsById_ServiceReturnsNull_ReturnsOk()
    {
      // Arrange
      GetMockedResourcesGetProjectsById(5, null, null, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjectsById(5);

      // Assert
      projectServiceMock.Received(1).GetProjectsById(Arg.Any<int>());
      Assert.IsType<OkObjectResult>(response);
    }

    #endregion

    #region GetProjectsByClientId

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfSpecificIdProjectDtosViewModelsClassData))]
    public void GetProjectsByClientId_ServiceContainsListOfProjects_ReturnsOkWithProjectsInBody(int projectId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns)
    {
      // Arrange
      GetMockedResourcesGetProjectsByClientId(projectId, serviceReturns, controllerReturns, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjectsByClientId(projectId);

      var resultList = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);

      // Assert
      projectServiceMock.Received(1).GetProjectsByClientId(Arg.Any<int>());
      resultList.Should().BeEquivalentTo(controllerReturns);
    }

    [Fact]
    public void GetProjectsByClientId_ServiceReturnsEmptyList_ReturnsOk()
    {
      // Arrange
      GetMockedResourcesGetProjectsByClientId(5, new List<ProjectDto>(), new List<ProjectViewModel>(), out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjectsByClientId(5);

      // Assert
      projectServiceMock.Received(1).GetProjectsByClientId(Arg.Any<int>());
      Assert.IsType<OkObjectResult>(response);
    }

    [Fact]
    public void GetProjectsByClientId_ServiceReturnsNull_ReturnsOk()
    {
      // Arrange
      GetMockedResourcesGetProjectsByClientId(5, null, null, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.GetProjectsByClientId(5);

      // Assert
      projectServiceMock.Received(1).GetProjectsByClientId(Arg.Any<int>());
      Assert.IsType<OkObjectResult>(response);
    }

    #endregion

    #region CreateProject

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfProjectDtosCreateViewModelsClassData))]
    public void CreateProject_ServiceCreatesProject_ReturnsOkWithCreatedProjectInBody(CreateProjectViewModel controllerInput, ProjectDto serviceReturns,
      ProjectViewModel controllerReturns)
    {
      // Arrange
      GetMockedResourcesCreateProject(controllerInput, serviceReturns, controllerReturns, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.CreateProject(controllerInput);

      var result = RetrieveValueFromActionResult<ProjectViewModel>(response, HttpStatusCode.Created);

      // Assert
      projectServiceMock.Received(1).CreateProject(Arg.Any<ProjectDto>());
      result.Should().BeEquivalentTo(controllerReturns);
    }

    #endregion

    #region UpdateProject

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfProjectDtosUpdateViewModelsClassData))]
    public void UpdateProject_ServiceUpdatesProject_ReturnsOkWithProjectInBody(UpdateProjectViewModel controllerInput, ProjectDto serviceReturns,
      ProjectViewModel controllerReturns)
    {
      // Arrange
      GetMockedResourcesUpdateProject(controllerInput, serviceReturns, controllerReturns, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.UpdateProject(controllerInput);

      var result = RetrieveValueFromActionResult<ProjectViewModel>(response);

      // Assert
      projectServiceMock.Received(1).UpdateProject(Arg.Any<ProjectDto>());
      result.Should().BeEquivalentTo(controllerReturns);
    }

    [Fact]
    public void UpdateProject_ServiceEncountersErrorUpdating_ReturnsBadRequest()
    {
      // Arrange
      var projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.UpdateProject(Arg.Any<ProjectDto>()).Throws<Exception>();

      var mapper = SetupMockedMapper(new UpdateProjectViewModel(), new ProjectDto());
      var projectController = new ProjectController(projectServiceMock, mapper);

      // Act
      var response = projectController.UpdateProject(new UpdateProjectViewModel());

      // Assert
      projectServiceMock.Received(1).UpdateProject(Arg.Any<ProjectDto>());
      response.Should().BeOfType<BadRequestResult>();
    }

    #endregion

    #region DeleteProject


    #endregion

    #region MockCreation

    private void GetMockedResourcesGetProjects(IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns,
      out IProjectService projectServiceMock, out ProjectController projectController)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.GetProjects().Returns(serviceReturns);

      projectController = GetMockedProjectController(serviceReturns, controllerReturns, projectServiceMock, out _);
    }

    private void GetMockedResourcesGetProjectsById(int projectId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns,
      out IProjectService projectServiceMock, out ProjectController projectController)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.GetProjectsById(projectId).Returns(serviceReturns);

      projectController = GetMockedProjectController(serviceReturns, controllerReturns, projectServiceMock, out _);
    }

    private void GetMockedResourcesGetProjectsByClientId(int projectId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns,
      out IProjectService projectServiceMock, out ProjectController projectController)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.GetProjectsByClientId(projectId).Returns(serviceReturns);

      projectController = GetMockedProjectController(serviceReturns, controllerReturns, projectServiceMock, out _);
    }

    private void GetMockedResourcesCreateProject(CreateProjectViewModel controllerInput, ProjectDto serviceReturns, ProjectViewModel controllerReturns,
      out IProjectService projectServiceMock, out ProjectController projectController)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.CreateProject(serviceReturns).Returns(serviceReturns);

      projectController = GetMockedProjectController(controllerInput, serviceReturns, projectServiceMock, out var mapper);
      mapper.Map<ProjectViewModel>(serviceReturns).Returns(controllerReturns);
    }

    private void GetMockedResourcesUpdateProject(UpdateProjectViewModel controllerInput, ProjectDto serviceReturns, ProjectViewModel controllerReturns,
      out IProjectService projectServiceMock, out ProjectController projectController)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.UpdateProject(serviceReturns).Returns(serviceReturns);

      projectController = GetMockedProjectController(controllerInput, serviceReturns, projectServiceMock, out var mapper);
      mapper.Map<ProjectViewModel>(serviceReturns).Returns(controllerReturns);
    }

    private ProjectController GetMockedProjectController<TMapFrom, TMapTo>(TMapFrom mapFrom, TMapTo mapTo, IProjectService projectServiceMock, out IMapper mapper)
    {
      mapper = SetupMockedMapper(mapFrom, mapTo);
      return new ProjectController(projectServiceMock, mapper);
    }

    #endregion
  }
}
