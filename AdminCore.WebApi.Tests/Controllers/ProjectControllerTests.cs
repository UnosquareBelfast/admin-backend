using System;
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
using Microsoft.EntityFrameworkCore;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class ProjectControllerTests : BaseControllerTest
  {

    #region GetProjects

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfSpecificIdProjectDtosViewModelsClassData))]
    public void GetProjects_ServiceContainsProjects_ReturnsOkWithExpectedNumberItemsInBodyWithAllCallsReceived(int projectId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns,
      int expectedReturnCount)
    {
      // Arrange
      GetMockedResourcesGetProjects(null, null, serviceReturns, controllerReturns, out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(null, null);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().NotBeNull().And.HaveCount(expectedReturnCount);
      projectServiceMock.Received(1).GetProjects();

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == expectedReturnCount));
    }

    [Fact]
    public void GetProjects_ServiceReturnsEmptyList_ReturnsOkWithEmptyListInBodyWithAllCallsReceived()
    {
      // Arrange
      GetMockedResourcesGetProjects(null, null,new List<ProjectDto>(), new List<ProjectViewModel>(), out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(null, null);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().BeEmpty();
      projectServiceMock.Received(1).GetProjects();

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == 0));
    }

    [Fact]
    public void GetProjects_ServiceReturnsNull_ReturnsOkWithEmptyListInBodyWithAllCallsReceived()
    {
      // Arrange
      GetMockedResourcesGetProjects(null, null, null, new List<ProjectViewModel>(), out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(null, null);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().BeEmpty();
      projectServiceMock.Received(1).GetProjects();

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == 0));
    }

    #endregion

    #region GetProjectsById

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfSpecificIdProjectDtosViewModelsClassData))]
    public void GetProjectsById_ServiceContainsListOfProjects_ReturnsOkWithExpectedNumberItemsInBodyWithAllCallsReceived(int projectId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns,
      int expectedReturnCount)
    {
      // Arrange
      GetMockedResourcesGetProjects(projectId, null, serviceReturns, controllerReturns, out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(projectId, null);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().NotBeNull().And.HaveCount(expectedReturnCount);
      projectServiceMock.Received(1).GetProjects(projectId);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == expectedReturnCount));
    }

    [Fact]
    public void GetProjectsById_ServiceReturnsEmptyList_ReturnsOkWithEmptyListInBodyWithAllCallsReceived()
    {
      // Arrange
      GetMockedResourcesGetProjects(546, null, new List<ProjectDto>(), new List<ProjectViewModel>(), out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(546, null);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().BeEmpty();
      projectServiceMock.Received(1).GetProjects(546);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == 0));
    }

    [Fact]
    public void GetProjectsById_ServiceReturnsNull_ReturnsOkWithEmptyListInBodyWithAllCallsReceived()
    {
      // Arrange
      GetMockedResourcesGetProjects(234, null,null, new List<ProjectViewModel>(), out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(234, null);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().BeEmpty();
      projectServiceMock.Received(1).GetProjects(234);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == 0));
    }

    #endregion

    #region GetProjectsByClientId

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfSpecificIdProjectDtosViewModelsClassData))]
    public void GetProjectsByClientId_ServiceContainsListOfProjects_ReturnsOkWithExpectedNumberItemsInBodyWithAllCallsReceived(int clientId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns,
      int expectedReturnCount)
    {
      // Arrange
      GetMockedResourcesGetProjects(null, clientId, serviceReturns, controllerReturns, out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(null, clientId);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().NotBeNull().And.HaveCount(expectedReturnCount);
      projectServiceMock.Received(1).GetProjects(null, clientId);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == expectedReturnCount));
    }

    [Fact]
    public void GetProjectsByClientId_ServiceReturnsEmptyList_ReturnsOkWithEmptyListInBodyWithAllCallsReceived()
    {
      // Arrange
      GetMockedResourcesGetProjects(null, 232, new List<ProjectDto>(), new List<ProjectViewModel>(), out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(null, 232);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().BeEmpty();
      projectServiceMock.Received(1).GetProjects(null, 232);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == 0));
    }

    [Fact]
    public void GetProjectsByClientId_ServiceReturnsNull_ReturnsOkWithEmptyListInBodyWithAllCallsReceived()
    {
      // Arrange
      GetMockedResourcesGetProjects(null, 647,null, new List<ProjectViewModel>(), out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(null, 647);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().BeEmpty();
      projectServiceMock.Received(1).GetProjects(null, 647);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == 0));
    }

    #endregion

    #region GetProjectsByProjectIdAndCliendId

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfSpecificIdProjectDtosViewModelsClassData))]
    public void GetProjectsByProjectIdClientId_ServiceContainsListOfProjects_ReturnsOkWithExpectedNumberItemsInBodyWithAllCallsReceived(int clientId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns,
      int expectedReturnCount)
    {
      // Arrange
      GetMockedResourcesGetProjects(null, clientId, serviceReturns, controllerReturns, out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(null, clientId);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().NotBeNull().And.HaveCount(expectedReturnCount);
      projectServiceMock.Received(1).GetProjects(null, clientId);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == expectedReturnCount));
    }

    [Fact]
    public void GetProjectsByProjectIdClientId_ServiceReturnsEmptyList_ReturnsOkWithEmptyListInBodyWithAllCallsReceived()
    {
      // Arrange
      GetMockedResourcesGetProjects(555, 232, new List<ProjectDto>(), new List<ProjectViewModel>(), out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(555, 232);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().BeEmpty();
      projectServiceMock.Received(1).GetProjects(555, 232);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == 0));
    }

    [Fact]
    public void GetProjectsByProjectIdClientId_ServiceReturnsNull_ReturnsOkWithEmptyListInBodyWithAllCallsReceived()
    {
      // Arrange
      GetMockedResourcesGetProjects(123, 647,null, new List<ProjectViewModel>(), out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.GetProjects(123, 647);

      // Assert
      var result = RetrieveValueFromActionResult<IList<ProjectViewModel>>(response);
      result.Should().BeEmpty();
      projectServiceMock.Received(1).GetProjects(123, 647);

      mapper.Received(1).Map<IList<ProjectViewModel>>(Arg.Is<List<ProjectDto>>(x => x != null && x.Count == 0));
    }

    #endregion

    #region CreateProject

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfProjectDtosCreateViewModelsClassData))]
    public void CreateProject_ServiceCreatesProject_ReturnsOkWithCreatedProjectInBodyAndAllCalls(CreateProjectViewModel controllerInput, ProjectDto serviceReturns)
    {
      // Arrange
      GetMockedResourcesCreateProject(serviceReturns, out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.CreateProject(controllerInput);

      // Assert
      var result = RetrieveValueFromActionResult<ProjectViewModel>(response, HttpStatusCode.Created);
      projectServiceMock.Received(1).CreateProject(Arg.Any<ProjectDto>());
      result.Should().BeOfType<ProjectViewModel>();
      mapper.Received(1).Map<ProjectDto>(Arg.Any<CreateProjectViewModel>());
      mapper.Received(1).Map<ProjectViewModel>(Arg.Any<ProjectDto>());
    }

    [Fact]
    public void CreateProject_ServiceEncountersDbUpdateException_ReturnsBadRequestAndAllCallsReceived()
    {
      // Arrange
      var projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.CreateProject(Arg.Any<ProjectDto>()).Throws(info => new DbUpdateException("", (Exception)null));

      var mapper = Substitute.For<IMapper>();
      mapper.Map<ProjectDto>(Arg.Any<CreateProjectViewModel>()).Returns(new ProjectDto());

      var projectController = new ProjectController(projectServiceMock, mapper);

      // Act
      var response = projectController.CreateProject(new CreateProjectViewModel());

      // Assert
      response.Should().BeOfType<BadRequestResult>();
      projectServiceMock.Received(1).CreateProject(Arg.Any<ProjectDto>());
      mapper.Received(1).Map<ProjectDto>(Arg.Any<CreateProjectViewModel>());
    }

    #endregion

    #region UpdateProject

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.ListOfProjectDtosUpdateViewModelsClassData))]
    public void UpdateProject_ServiceUpdatesProject_ReturnsOkWithUpdatedProjectInBodyAndAllCalls(UpdateProjectViewModel controllerInput, ProjectDto serviceReturns)
    {
      // Arrange
      GetMockedResourcesUpdateProject(serviceReturns, out var projectServiceMock, out var projectController, out var mapper);

      // Act
      var response = projectController.UpdateProject(controllerInput);

      // Assert
      var result = RetrieveValueFromActionResult<ProjectViewModel>(response);
      projectServiceMock.Received(1).UpdateProject(Arg.Any<ProjectDto>());
      result.Should().BeOfType<ProjectViewModel>();
      mapper.Received(1).Map<ProjectDto>(Arg.Any<UpdateProjectViewModel>());
      mapper.Received(1).Map<ProjectViewModel>(Arg.Any<ProjectDto>());
    }

    [Fact]
    public void UpdateProject_ServiceEncountersDbUpdateException_ReturnsBadRequestAndAllCallsReceived()
    {
      // Arrange
      var projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.UpdateProject(Arg.Any<ProjectDto>()).Throws(info => new DbUpdateException("", (Exception)null));

      var mapper = Substitute.For<IMapper>();
      mapper.Map<ProjectDto>(Arg.Any<UpdateProjectViewModel>()).Returns(new ProjectDto());

      var projectController = new ProjectController(projectServiceMock, mapper);

      // Act
      var response = projectController.UpdateProject(new UpdateProjectViewModel());

      // Assert
      response.Should().BeOfType<BadRequestResult>();
      projectServiceMock.Received(1).UpdateProject(Arg.Any<ProjectDto>());
      mapper.Received(1).Map<ProjectDto>(Arg.Any<UpdateProjectViewModel>());
    }

    #endregion

    #region DeleteProject

    [Theory]
    [ClassData(typeof(ProjectControllerClassData.RandomProjectIdClassData))]
    public void DeleteProject_ServiceDeletesProject_ReturnsOk(int projectId)
    {
      // Arrange
      GetMockedResourcesDeleteProject(projectId, out var projectServiceMock, out var projectController);

      // Act
      var response = projectController.DeleteProject(projectId);

      // Assert
      response.Should().BeOfType<OkResult>();
      projectServiceMock.Received(1).DeleteProject(Arg.Any<int>());
    }

    [Fact]
    public void DeleteProject_ServiceEncountersDbUpdateException_ReturnsBadRequest()
    {
      // Arrange
      var projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.When(x => x.DeleteProject(2832)).Do(x => throw new DbUpdateException("", (Exception)null));

      var projectController = new ProjectController(projectServiceMock, null);

      // Act
      var response = projectController.DeleteProject(2832);

      // Assert
      response.Should().BeOfType<BadRequestResult>();
      projectServiceMock.Received(1).DeleteProject(Arg.Any<int>());
    }

    #endregion

    #region MockCreation

    private void GetMockedResourcesGetProjects(int? projectId, int? clientId, IList<ProjectDto> serviceReturns, IList<ProjectViewModel> controllerReturns,
      out IProjectService projectServiceMock, out ProjectController projectController, out IMapper mapper)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.GetProjects(projectId, clientId).Returns(serviceReturns);

      projectController = GetMockedProjectController<IList<ProjectDto>, IList<ProjectViewModel>>(controllerReturns, projectServiceMock, out mapper);
    }

    private void GetMockedResourcesCreateProject(ProjectDto serviceReturns,
      out IProjectService projectServiceMock, out ProjectController projectController, out IMapper mapper)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.CreateProject(Arg.Any<ProjectDto>()).Returns(serviceReturns);

      projectController = GetMockedProjectController<CreateProjectViewModel, ProjectDto>(serviceReturns, projectServiceMock, out mapper);
      mapper.Map<ProjectViewModel>(Arg.Any<ProjectDto>()).Returns(new ProjectViewModel());
    }

    private void GetMockedResourcesUpdateProject(ProjectDto serviceReturns,
      out IProjectService projectServiceMock, out ProjectController projectController, out IMapper mapper)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.UpdateProject(serviceReturns).Returns(serviceReturns);

      projectController = GetMockedProjectController<UpdateProjectViewModel, ProjectDto>(serviceReturns, projectServiceMock, out mapper);
      mapper.Map<ProjectViewModel>(Arg.Any<ProjectDto>()).Returns(new ProjectViewModel());
    }

    private void GetMockedResourcesDeleteProject(int projectId,
      out IProjectService projectServiceMock, out ProjectController projectController)
    {
      projectServiceMock = Substitute.For<IProjectService>();
      projectServiceMock.When(x => x.DeleteProject(projectId)).Do(x => { });

      projectController = new ProjectController(projectServiceMock, null);
    }

    private ProjectController GetMockedProjectController<TMapFrom, TMapTo>(TMapTo mapTo, IProjectService projectServiceMock, out IMapper mapper)
    {
      mapper = Substitute.For<IMapper>();
      mapper.Map<TMapTo>(Arg.Any<TMapFrom>()).Returns(mapTo);
      return new ProjectController(projectServiceMock, mapper);
    }

    #endregion
  }
}
