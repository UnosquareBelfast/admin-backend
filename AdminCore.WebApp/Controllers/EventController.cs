using AdminCore.Common.Exceptions;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.WebApi.Models.Event;
using AdminCore.WebApi.Models.EventMessage;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using AdminCore.Common.Authorization;
using AdminCore.WebApi.Models;
using AdminCore.WebApi.Models.DataTransform;
using ChoETL;

namespace AdminCore.WebApi.Controllers
{
  [ApiController]
  [Authorize]
  [Route("[controller]")]
  public class EventController : BaseController
  {
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;
    private readonly EmployeeDto _employee;
    private readonly IEventMessageService _eventMessageService;
    private readonly ICsvService _csvService;
    private readonly IDateService _dateService;

    public EventController(IEventService wfhEventService, IEventMessageService eventMessageService, IMapper mapper, IAuthenticatedUser authenticatedUser,
      ICsvService csvService, IDateService dateService)
      : base(mapper)
    {
      _eventService = wfhEventService;
      _eventMessageService = eventMessageService;
      _mapper = mapper;
      _employee = authenticatedUser.RetrieveLoggedInUser();
      _csvService = csvService;
      _dateService = dateService;
    }

    [HttpGet]
    public IActionResult GetAllHolidays()
    {
      var returnedEvents = _eventService.GetEmployeeEvents(EventTypes.AnnualLeave);
      if (returnedEvents != null)
      {
        return Ok(_mapper.Map<IList<EventViewModel>>(returnedEvents));
      }

      return StatusCode((int)HttpStatusCode.NoContent, "No Event exists");
    }

    [HttpGet("findEventsByTypeId/{eventTypeId}")]
    public IActionResult GetAllEventsByEventType(int eventTypeId)
    {
      var returnedEvents = _eventService.GetEmployeeEvents((EventTypes)eventTypeId); //TODO
      if (returnedEvents != null)
      {
        return Ok(_mapper.Map<IList<EventViewModel>>(returnedEvents));
      }

      return StatusCode((int)HttpStatusCode.NoContent, "No Event exists");
    }

    [HttpGet("{eventId}")]
    public IActionResult GetEventByEventId(int eventId)
    {
      var returnedEvent = _eventService.GetEvent(eventId);
      if (returnedEvent != null)
      {
        return Ok(_mapper.Map<EventViewModel>(returnedEvent));
      }

      return StatusCode((int)HttpStatusCode.NoContent, $"No event found for event ID: { eventId.ToString() }");
    }

    [HttpGet("findByEmployeeId/{employeeId}/{eventTypeId}")]
    public IActionResult GetEventByEmployeeId(int employeeId, int eventTypeId)
    {
      var events = _eventService.GetEventsByEmployeeId(employeeId, (EventTypes)eventTypeId);
      if (events != null)
      {
        return Ok(_mapper.Map<IList<EventViewModel>>(events));
      }

      return StatusCode((int)HttpStatusCode.NoContent, $"No event found for employee ID: {employeeId}");
    }

    [HttpGet("findEmployeeHolidayStats")]
    public IActionResult GetEmployeeHolidayStats()
    {
      var holidayStats = _eventService.GetHolidayStatsForUser(_employee.EmployeeId);
      if (holidayStats != null)
      {
        return Ok(_mapper.Map<HolidayStatsViewModel>(holidayStats));
      }

      return StatusCode((int)HttpStatusCode.NoContent, "No Holiday exists");
    }

    [HttpGet("findByDateBetween/{startDate}/{endDate}/{eventTypeId}")]
    public IActionResult GetHolidayByDateBetween(string startDate, string endDate, int eventTypeId)
    {
      if (IsDatesValid(startDate, endDate))
      {
        return Ok(GetEventsBetweenDates(startDate, endDate, eventTypeId));
      }
      return BadRequest("Please use a valid date format for start and/or end dates");
    }

    [HttpGet("findByEventStatus/{eventStatusId}/{eventTypeId}")]
    public IActionResult GetEventByStatusType(int eventStatusId, int eventTypeId)
    {
      var events = _eventService.GetEventByStatus((EventStatuses)eventStatusId, (EventTypes)eventTypeId);
      if (events != null)
      {
        return Ok(_mapper.Map<IList<EventViewModel>>(events));
      }
      return StatusCode((int)HttpStatusCode.NoContent, "No Event exists");
    }

    [HttpGet("findByEventStatus/{eventStatusId}/{eventTypeId}/csv")]
    public IActionResult GetEventByStatusTypeCsv(int eventStatusId, int eventTypeId)
    {
      var events = _eventService.GetEventByStatus((EventStatuses)eventStatusId, (EventTypes)eventTypeId);
      var mappedEvents = _mapper.Map<IList<EventDataTransformModel>>(events);

      var stream = new MemoryStream(_csvService.Generate(mappedEvents));
      return File(stream, MediaTypeNames.Application.Octet,
        $"{(EventStatuses)eventStatusId}_{(EventTypes)eventTypeId}_Report{_dateService.GetCurrentDateTime():dd/MM/yyyy}.csv");
    }

    [HttpGet("findEventMessages/{eventId}")]
    public IActionResult GetEventMessages(int eventId)
    {
      var eventMessages = _eventMessageService.GetAllEventMessagesForEvent(eventId);
      if (eventMessages != null)
      {
        return Ok(_mapper.Map<IList<EventMessageViewModel>>(eventMessages));
      }

      return StatusCode((int)HttpStatusCode.NoContent, "No Messages exists");
    }

    [HttpGet("findEmployeeHolidayStats/{employeeId}")]
    public IActionResult GetEmployeeHolidayStats(int employeeId)
    {
      var holidayStats = _eventService.GetHolidayStatsForUser(employeeId);
      if (holidayStats != null)
      {
        return Ok(_mapper.Map<HolidayStatsViewModel>(holidayStats));
      }

      return StatusCode((int)HttpStatusCode.NoContent, "No Holiday exists");
    }

    [HttpPost]
    public IActionResult CreateEvent(CreateEventViewModel createEventViewModel)
    {
      return CreateEventForEmployeeId(createEventViewModel, _employee.EmployeeId);
    }

    [AdminCoreRoles(EmployeeRoles.SystemAdministrator)]
    [HttpPost("{employeeId}")]
    public IActionResult CreateEvent(CreateEventViewModel createEventViewModel, int employeeId)
    {
      return CreateEventForEmployeeId(createEventViewModel, employeeId);
    }

    private IActionResult CreateEventForEmployeeId(CreateEventViewModel createEventViewModel, int employeeId)
    {
      var eventDates = _mapper.Map<EventDateDto>(createEventViewModel);
      try
      {
        ValidateIfHolidayEvent(createEventViewModel, eventDates, employeeId);
        _eventService.CreateEvent(eventDates, (EventTypes) createEventViewModel.EventTypeId, employeeId);
        return Ok("Event has been created successfully");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Error creating event: " + ex.Message);
      }
    }

    private static void PublicHolidayValidation(CreateEventViewModel createEventViewModel)
    {
      if (IsPublicHoliday(createEventViewModel))
      {
        throw new ValidationException("You may not create new public holidays");
      }
    }

    [HttpPut]
    public IActionResult UpdateEvent(UpdateEventViewModel updateEventViewModel)
    {
      var eventDatesToUpdate = _mapper.Map<EventDateDto>(updateEventViewModel);
      try
      {
        _eventService.IsEventValid(eventDatesToUpdate, _employee.EmployeeId);
        _eventService.UpdateEvent(eventDatesToUpdate, updateEventViewModel.Message, _employee.EmployeeId);
        return Ok("Holiday has been successfully updated.");
      }
      catch (Exception ex)
      {
        var message = ex.Message;
        Logger.LogError(message);
        return StatusCode((int)HttpStatusCode.InternalServerError, $"Error updating holiday {message}.");
      }
    }

    [Authorize("Admin")]
    [HttpPut("approveEvent")]
    public IActionResult ApproveEvent(ApproveEventViewModel approveEventViewModel)
    {
      try
      {
        var eventToApprove = _eventService.GetEvent(approveEventViewModel.EventId);
        return ApproveIfEventDoesNotBelongToTheAdmin(approveEventViewModel, eventToApprove);
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong approving event");
      }
    }

    [HttpPut("cancelEvent")]
    public IActionResult CancelEvent(CancelEventViewModel cancelEventViewModel)
    {
      try
      {
        _eventService.UpdateEventStatus(cancelEventViewModel.EventId, EventStatuses.Cancelled);
        return Ok("Successfully Cancelled");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong cancelling event");
      }
    }

    [Authorize("Admin")]
    [HttpPut("rejectEvent")]
    public IActionResult RejectEvent(RejectEventViewModel rejectEventViewModel)
    {
      try
      {
        _eventService.RejectEvent(rejectEventViewModel.EventId, rejectEventViewModel.Message, _employee.EmployeeId);
        return Ok("Successfully Rejected");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong rejecting event");
      }
    }

    [HttpPut("addMessageToEvent")]
    public IActionResult AddMessageToEvent(CreateEventMessageViewModel eventMessageViewModel)
    {
      var returnedEvent = _eventService.GetEvent(eventMessageViewModel.EventId);
      if (returnedEvent != null)
      {
        _eventMessageService.CreateGeneralEventMessage(eventMessageViewModel.EventId, eventMessageViewModel.Message, _employee.EmployeeId);
        return Ok("Successfully Added Message");
      }

      return StatusCode((int)HttpStatusCode.NoContent, "No Messages exists");
    }

    [Authorize("Admin")]
    [HttpPost("MandatoryEvent")]
    public IActionResult CreateMandatoryEvent(CreateMandatoryEventViewModel createMandatoryEventViewModel)
    {
      try
      {
        _eventService.AddMandatoryEvent(createMandatoryEventViewModel.Date, createMandatoryEventViewModel.CountryId);
        return Ok("Successfully Added mandatory event");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong creating mandatory event");
      }
    }

    [Authorize("Admin")]
    [HttpPut("MandatoryEvent")]
    public IActionResult UpdateMandatoryEvent(UpdateMandatoryEventViewModel updateMandatoryEventViewModel)
    {
      try
      {
        _eventService.UpdateMandatoryEvent(updateMandatoryEventViewModel.MandatoryEventId, updateMandatoryEventViewModel.Date,
          updateMandatoryEventViewModel.CountryId);
        return Ok("Successfully Updated mandatory event");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong updating mandatory event");
      }
    }

    [Authorize("Admin")]
    [HttpGet("MandatoryEvent/{countryId}")]
    public IActionResult GetPublicHolidays(int countryId)
    {
      try
      {
        return Ok(_mapper.Map<IList<MandatoryEventViewModel>>(_eventService.GetMandatoryEvents(countryId)));
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong getting mandatory event");
      }
    }

    [Authorize("Admin")]
    [HttpPut("DeleteMandatoryEvent/{publicHolidayId}")]
    public IActionResult DeletePublicHoliday(int mandatoryEventId)
    {
      try
      {
        _eventService.DeleteMandatoryEvent(mandatoryEventId);
        return Ok("Successfully deleted Mandatory Event");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong deleting mandatory event");
      }
    }

    private bool ValidateDate(string date)
    {
      try
      {
        DateTime dt = DateTime.Parse(date);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private bool IsDatesValid(string startDate, string endDate)
    {
      return ValidateDate(startDate) && ValidateDate(endDate);
    }

    private IList<EventViewModel> GetEventsBetweenDates(string startDate, string endDate, int eventTypeId)
    {
      var holidays = _eventService.GetByDateBetween(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), (EventTypes)eventTypeId);
      return _mapper.Map<IList<EventViewModel>>(holidays);
    }

    private static bool IsHolidayEvent(int eventTypeId)
    {
      return eventTypeId != (int)EventTypes.WorkingFromHome;
    }

    private IActionResult ApproveIfEventDoesNotBelongToTheAdmin(ApproveEventViewModel approveEventViewModel,
      EventDto eventToApprove)
    {
      if (!EventIsBookedByCurrentAdmin(eventToApprove))
      {
        _eventService.UpdateEventStatus(approveEventViewModel.EventId, EventStatuses.Approved);
        return Ok("Successfully Approved");
      }

      return StatusCode((int)HttpStatusCode.Forbidden, "You may not approve your own Events");
    }

    private bool EventIsBookedByCurrentAdmin(EventDto eventToApprove)
    {
      return eventToApprove.EmployeeId == _employee.EmployeeId;
    }

    private void ValidateIfHolidayEvent(CreateEventViewModel createEventViewModel, EventDateDto eventDates, int employeeId)
    {
      PublicHolidayValidation(createEventViewModel);
      if (IsHolidayEvent(createEventViewModel.EventTypeId))
      {
        _eventService.IsEventValid(eventDates, employeeId);
      }
    }

    private static bool IsPublicHoliday(CreateEventViewModel createEventViewModel)
    {
      return createEventViewModel.EventTypeId == (int)EventTypes.PublicHoliday;
    }
  }
}
