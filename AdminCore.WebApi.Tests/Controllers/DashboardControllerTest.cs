using AdminCore.Common.Interfaces;
using AdminCore.DTOs.Dashboard;
using AdminCore.DTOs.Event;
using AdminCore.WebApi.Controllers;
using AdminCore.WebApi.Models.Dashboard;
using AutoFixture;
using AutoMapper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.EventMessage;
using AdminCore.WebApi.Mappings;
using AdminCore.WebApi.Models.Event;
using AdminCore.WebApi.Models.EventMessage;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class DashboardControllerTest : BaseControllerTest
  {
    private readonly DashboardController _dashboardController;
    private readonly IDashboardService _dashboardService;
    private readonly Fixture _fixture;

    private const int TestEmployeeId = 1;
    private static readonly DateTime TestDate = new DateTime(2019,01,01,0,0,0);

    public DashboardControllerTest()
    {
      _fixture = new Fixture();
      _dashboardService = Substitute.For<IDashboardService>();
      IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new WebMappingProfile())));
      var authenticatedUser = Substitute.For<IAuthenticatedUser>();
      authenticatedUser.RetrieveLoggedInUser().Returns(Builder.BuildTestEmployee(TestEmployeeId));
      _dashboardController = new DashboardController(_dashboardService, mapper, authenticatedUser);
    }

    [Fact]
    public void GetDashboardSnapshotReturnsOkResultWithEventsWhenEventsAreReturned()
    {
      // Arrange
      const int numberOfEvents = 4;
      var eventsReturnedFromService = Builder.BuildListOfEvents(numberOfEvents, TestDate, TestEmployeeId);
      _dashboardService.GetEmployeeDashboardEvents(TestEmployeeId, Arg.Any<DateTime>()).Returns(eventsReturnedFromService);

      // Act
      var result = _dashboardController.GetDashboardSnapshot();

      // Assert
      var resultValue = RetrieveValueFromActionResult<List<DashboardEventViewModel>>(result);
      Assert.Equal(resultValue.Count(), numberOfEvents);
      _dashboardService.Received(1).GetEmployeeDashboardEvents(TestEmployeeId, Arg.Any<DateTime>());
    }

    [Fact]
    public void GetDashboardSnapshotReturnsNoContentResultWhenNoEventsAreReturned()
    {
      // Arrange
      _dashboardService.GetEmployeeDashboardEvents(TestEmployeeId, Arg.Any<DateTime>()).Returns(new List<EventDto>());

      // Act
      var result = _dashboardController.GetDashboardSnapshot();

      // Assert
      AssertObjectResultIsNull<IList<DashboardEventViewModel>>(result, HttpStatusCode.NoContent);
      _dashboardService.Received(1).GetEmployeeDashboardEvents(TestEmployeeId, Arg.Any<DateTime>());
    }

    [Fact]
    public void GetEmployeeEventsReturnsOkResultWithEmployeeEventsWhenEmployeeEventsAreReturned()
    {
      // Arrange
      const int numberOfEvents = 5;
      var eventsReturnedFromService = Builder.BuildListOfEvents(numberOfEvents, TestDate, TestEmployeeId);
      _dashboardService.GetEmployeeEventsForMonth(TestEmployeeId, TestDate).Returns(eventsReturnedFromService);

      // Act
      var result = _dashboardController.GetEmployeeEvents(TestDate);

      // Assert
      var resultValue = RetrieveValueFromActionResult<IList<EventViewModel>>(result);
      Assert.Equal(resultValue.Count, numberOfEvents);
      _dashboardService.Received(1).GetEmployeeEventsForMonth(TestEmployeeId, TestDate);
    }

    [Fact]
    public void GetEmployeeEventsReturnsOkResultWithNullObjectWhenNoEmployeeEventsAreReturned()
    {
      // Arrange
      _dashboardService.GetEmployeeEventsForMonth(TestEmployeeId, TestDate).ReturnsNull();

      // Act
      var result = _dashboardController.GetEmployeeEvents(TestDate);

      // Assert
      var resultValue = RetrieveValueFromActionResult<IList<EventViewModel>>(result);
      Assert.Null(resultValue);
      _dashboardService.Received(1).GetEmployeeEventsForMonth(TestEmployeeId, TestDate);
    }

    [Fact]
    public void GetEmployeeTeamSnapshot_ServiceReturnsSnapshotList_ReturnsOkResultWithClientSnapshots()
    {
      // Arrange
      var numberOfSnapshotModels = 6;
      var snapshotReturnedFromService = _fixture.CreateMany<ClientSnapshotDto>(numberOfSnapshotModels).ToList();
      _dashboardService.GetTeamDashboardEvents(TestEmployeeId, Arg.Any<DateTime>()).Returns(snapshotReturnedFromService);

      // Act
      var result = _dashboardController.GetEmployeeTeamSnapshot();

      // Assert
      var resultValue = RetrieveValueFromActionResult<List<ClientSnapshotViewModel>>(result);
      Assert.Equal(resultValue.Count(), numberOfSnapshotModels);
      _dashboardService.Received(1).GetTeamDashboardEvents(TestEmployeeId, Arg.Any<DateTime>());
    }

    [Fact]
    public void GetEmployeeTeamSnapshot_ServiceReturnsEmptySnapshot_ReturnsNoContentResult()
    {
      // Arrange
      _dashboardService.GetTeamDashboardEvents(TestEmployeeId, Arg.Any<DateTime>()).Returns(new List<ClientSnapshotDto>());

      // Act
      var result = _dashboardController.GetEmployeeTeamSnapshot();

      // Assert
      AssertObjectResultIsNull<List<ClientSnapshotViewModel>>(result, HttpStatusCode.NoContent);
      _dashboardService.Received(1).GetTeamDashboardEvents(TestEmployeeId, Arg.Any<DateTime>());
    }

    [Fact]
    public void GetEmployeeTeamSnapshot_ServiceReturnsNull_ReturnsNoContentResult()
    {
      // Arrange
      _dashboardService.GetTeamDashboardEvents(TestEmployeeId, Arg.Any<DateTime>()).Returns(x => null);

      // Act
      var result = _dashboardController.GetEmployeeTeamSnapshot();

      // Assert
      AssertObjectResultIsNull<List<ClientSnapshotViewModel>>(result, HttpStatusCode.NoContent);
      _dashboardService.Received(1).GetTeamDashboardEvents(TestEmployeeId, Arg.Any<DateTime>());
    }

    [Fact]
    public void GetTeamEventsReturnsOkResultWithEventsWhenServiceReturnsEvents()
    {
      // Arrange
      const int numberOfEvents = 22;
      var eventsReturnedFromService = Builder.BuildListOfEvents(numberOfEvents, TestDate, TestEmployeeId);
      _dashboardService.GetEmployeeTeamEvents(TestEmployeeId, TestDate).Returns(eventsReturnedFromService);

      // Act
      var result = _dashboardController.GetTeamEvents(TestDate);

      // Assert
      var resultValue = RetrieveValueFromActionResult<IList<EventViewModel>>(result);
      Assert.Equal(resultValue.Count(), numberOfEvents);
      _dashboardService.Received(1).GetEmployeeTeamEvents(TestEmployeeId, TestDate);
    }

    [Fact]
    public void GetTeamEventsReturnsNoContentResultWhenServiceReturnsNoEvents()
    {
      // Arrange
      _dashboardService.GetEmployeeTeamEvents(TestEmployeeId, TestDate).Returns(new List<EventDto>());

      // Act
      var result = _dashboardController.GetTeamEvents(TestDate);

      // Assert
      AssertObjectResultIsNull<IList<EventViewModel>>(result, HttpStatusCode.NoContent);
      _dashboardService.Received(1).GetEmployeeTeamEvents(TestEmployeeId, TestDate);
    }

  }
}
