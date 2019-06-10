using System;
using System.Net;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.WebApi.Controllers;
using AdminCore.WebApi.Models.Event;
using AdminCore.WebApi.Tests.ClassData;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class EventControllerTests : BaseControllerTest
  {
    private readonly Fixture _fixture = new Fixture();

    public EventControllerTests()
    {
      _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    #region CreateEvent_UsingLoggedInUser

    [Theory]
    [ClassData(typeof(EventControllerClassData.DefaultAuthenticatedUser))]
    public void CreateEvent_ServiceCreatesEvent_ReturnOk(EmployeeDto employeeDto)
    {
      // Arrange
      GetMockedResourcesCreateEvent(employeeDto, out var eventService, out _, out _, out _, out _, out var eventController,  out var mapper);

      eventService.When(x => x.CreateEvent(Arg.Any<EventDateDto>(), Arg.Any<EventTypes>(), employeeDto.EmployeeId)).Do(x => { });
      eventService.When(x => x.IsEventValid(Arg.Any<EventDateDto>(), employeeDto.EmployeeId)).Do(x => { });

      // Act
      var response = eventController.CreateEvent(new CreateEventViewModel{EventTypeId = 1});
      var responseCast = response as ObjectResult;

      // Assert
      responseCast.StatusCode.Should().Be((int)HttpStatusCode.OK);
      mapper.Received(1).Map<EventDateDto>(Arg.Any<CreateEventViewModel>());
      eventService.Received(1).IsEventValid(Arg.Any<EventDateDto>(), employeeDto.EmployeeId);
      eventService.Received(1).CreateEvent(Arg.Any<EventDateDto>(), Arg.Any<EventTypes>(), employeeDto.EmployeeId);
    }

    [Theory]
    [ClassData(typeof(EventControllerClassData.DefaultAuthenticatedUser))]
    public void CreateEvent_PublicHolidayIsBooked_ReturnInternalServerError(EmployeeDto employeeDto)
    {
      // Arrange
      GetMockedResourcesCreateEvent(employeeDto, out _, out _, out _, out _, out _, out var eventController,  out var mapper);

      // Act
      var response = eventController.CreateEvent(new CreateEventViewModel{EventTypeId = 0});
      var responseCast = response as ObjectResult;

      // Assert
      responseCast.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
      mapper.Received(1).Map<EventDateDto>(Arg.Any<CreateEventViewModel>());
    }

    [Theory]
    [ClassData(typeof(EventControllerClassData.DefaultAuthenticatedUser))]
    public void CreateEvent_ServiceCreateEventEncountersGeneralException_ReturnInternalServerError(EmployeeDto employeeDto)
    {
      // Arrange
      GetMockedResourcesCreateEvent(employeeDto, out var eventService, out _, out _, out _, out _, out var eventController,  out var mapper);

      eventService.When(x => x.CreateEvent(Arg.Any<EventDateDto>(), Arg.Any<EventTypes>(), employeeDto.EmployeeId)).Throw(new Exception());
      eventService.When(x => x.IsEventValid(Arg.Any<EventDateDto>(), employeeDto.EmployeeId)).Do(x => { });

      // Act
      var response = eventController.CreateEvent(new CreateEventViewModel{EventTypeId = 1});
      var responseCast = response as ObjectResult;

      // Assert
      responseCast.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
      mapper.Received(1).Map<EventDateDto>(Arg.Any<CreateEventViewModel>());
      eventService.Received(1).IsEventValid(Arg.Any<EventDateDto>(), employeeDto.EmployeeId);
      eventService.Received(1).CreateEvent(Arg.Any<EventDateDto>(), Arg.Any<EventTypes>(), employeeDto.EmployeeId);
    }

    [Theory]
    [ClassData(typeof(EventControllerClassData.DefaultAuthenticatedUser))]
    public void CreateEvent_EventValidationFails_ReturnInternalServerError(EmployeeDto employeeDto)
    {
      // Arrange
      GetMockedResourcesCreateEvent(employeeDto, out var eventService, out _, out _, out _, out _, out var eventController,  out var mapper);

      eventService.When(x => x.IsEventValid(Arg.Any<EventDateDto>(), employeeDto.EmployeeId)).Throw(new Exception());

      // Act
      var response = eventController.CreateEvent(new CreateEventViewModel{EventTypeId = 1});
      var responseCast = response as ObjectResult;

      // Assert
      responseCast.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
      mapper.Received(1).Map<EventDateDto>(Arg.Any<CreateEventViewModel>());
      eventService.Received(1).IsEventValid(Arg.Any<EventDateDto>(), employeeDto.EmployeeId);
    }

    #endregion

    #region CreateEvent_ForAnotherUser

    [Theory]
    [ClassData(typeof(EventControllerClassData.DefaultAuthenticatedUser))]
    public void CreateEventForAnotherUser_ServiceCreatesEvent_ReturnOk(EmployeeDto employeeDto)
    {
      // Arrange
      int employeeId = _fixture.Create<int>();

      GetMockedResourcesCreateEvent(employeeDto, out var eventService, out _, out _, out _, out _, out var eventController,  out var mapper);

      eventService.When(x => x.CreateEvent(Arg.Any<EventDateDto>(), Arg.Any<EventTypes>(), employeeId)).Do(x => { });
      eventService.When(x => x.IsEventValid(Arg.Any<EventDateDto>(), employeeId)).Do(x => { });

      // Act
      var response = eventController.CreateEvent(new CreateEventViewModel{EventTypeId = 1}, employeeId);
      var responseCast = response as ObjectResult;

      // Assert
      responseCast.StatusCode.Should().Be((int)HttpStatusCode.OK);
      mapper.Received(1).Map<EventDateDto>(Arg.Any<CreateEventViewModel>());
      eventService.Received(1).IsEventValid(Arg.Any<EventDateDto>(), employeeId);
      eventService.Received(1).CreateEvent(Arg.Any<EventDateDto>(), Arg.Any<EventTypes>(), employeeId);
    }

    [Theory]
    [ClassData(typeof(EventControllerClassData.DefaultAuthenticatedUser))]
    public void CreateEventForAnotherUser_PublicHolidayIsBooked_ReturnInternalServerError(EmployeeDto employeeDto)
    {
      // Arrange
      int employeeId = _fixture.Create<int>();

      GetMockedResourcesCreateEvent(employeeDto, out _, out _, out _, out _, out _, out var eventController,  out var mapper);

      // Act
      var response = eventController.CreateEvent(new CreateEventViewModel{EventTypeId = 0}, employeeId);
      var responseCast = response as ObjectResult;

      // Assert
      responseCast.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
      mapper.Received(1).Map<EventDateDto>(Arg.Any<CreateEventViewModel>());
    }

    [Theory]
    [ClassData(typeof(EventControllerClassData.DefaultAuthenticatedUser))]
    public void CreateEventForAnotherUser_ServiceCreateEventEncountersGeneralException_ReturnInternalServerError(EmployeeDto employeeDto)
    {
      // Arrange
      int employeeId = _fixture.Create<int>();

      GetMockedResourcesCreateEvent(employeeDto, out var eventService, out _, out _, out _, out _, out var eventController,  out var mapper);

      eventService.When(x => x.CreateEvent(Arg.Any<EventDateDto>(), Arg.Any<EventTypes>(), employeeId)).Throw(new Exception());
      eventService.When(x => x.IsEventValid(Arg.Any<EventDateDto>(), employeeId)).Do(x => { });

      // Act
      var response = eventController.CreateEvent(new CreateEventViewModel{EventTypeId = 1}, employeeId);
      var responseCast = response as ObjectResult;

      // Assert
      responseCast.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
      mapper.Received(1).Map<EventDateDto>(Arg.Any<CreateEventViewModel>());
      eventService.Received(1).IsEventValid(Arg.Any<EventDateDto>(), employeeId);
      eventService.Received(1).CreateEvent(Arg.Any<EventDateDto>(), Arg.Any<EventTypes>(), employeeId);
    }

    [Theory]
    [ClassData(typeof(EventControllerClassData.DefaultAuthenticatedUser))]
    public void CreateEventForAnotherUser_EventValidationFails_ReturnInternalServerError(EmployeeDto employeeDto)
    {
      // Arrange
      int employeeId = _fixture.Create<int>();

      GetMockedResourcesCreateEvent(employeeDto, out var eventService, out _, out _, out _, out _, out var eventController,  out var mapper);

      eventService.When(x => x.IsEventValid(Arg.Any<EventDateDto>(), employeeId)).Throw(new Exception());

      // Act
      var response = eventController.CreateEvent(new CreateEventViewModel{EventTypeId = 1}, employeeId);
      var responseCast = response as ObjectResult;

      // Assert
      responseCast.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
      mapper.Received(1).Map<EventDateDto>(Arg.Any<CreateEventViewModel>());
      eventService.Received(1).IsEventValid(Arg.Any<EventDateDto>(), employeeId);
    }

    #endregion

    #region GetMockedResources

    private void GetMockedResourcesCreateEvent(EmployeeDto employeeDto, out IEventService eventService, out IEventMessageService eventMessageService, out IAuthenticatedUser authenticatedUser,
      out ICsvService csvService, out IDateService dateService, out EventController eventController, out IMapper mapper)
    {
      eventService = Substitute.For<IEventService>();

      eventMessageService = Substitute.For<IEventMessageService>();

      authenticatedUser = Substitute.For<IAuthenticatedUser>();
      authenticatedUser.RetrieveLoggedInUser().Returns(employeeDto);

      csvService = Substitute.For<ICsvService>();

      dateService = Substitute.For<IDateService>();

      mapper = Substitute.For<IMapper>();
      mapper.Map<EventDateDto>(Arg.Any<CreateEventViewModel>()).Returns(new EventDateDto());

      eventController = GetMockedEventController(eventService, eventMessageService, authenticatedUser, csvService, dateService, mapper);
    }

    private EventController GetMockedEventController(IEventService eventServiceMock, IEventMessageService eventMessageService,
      IAuthenticatedUser authenticatedUser, ICsvService csvService, IDateService dateService, IMapper mapper)
    {
      return new EventController(eventServiceMock, eventMessageService, mapper, authenticatedUser, csvService, dateService);
    }

    #endregion
  }
}
