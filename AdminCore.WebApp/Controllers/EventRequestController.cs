using System;
using System.Net;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs.LinkGenerator;
using AdminCore.LinkGenerator.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdminCore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventRequestController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly ILinkGenerator _eventRequestLinkGenerator;
        private readonly IMapper _mapper;
        private readonly IDateService _dateService;

        public EventRequestController(IEventService eventService, ILinkGenerator eventRequestLinkGenerator, IMapper mapper, IDateService dateService)
            : base(mapper)
        {
            _eventService = eventService;
            _eventRequestLinkGenerator = eventRequestLinkGenerator;
            _mapper = mapper;
            _dateService = dateService;
        }

        [HttpGet("event-response/{hashId}")]
        public IActionResult GetEventByEventId(string hashId)
        {
            ObjectResult resultStatus = null;

            var eventRequest = _eventService.GetEventRequest(hashId);

            if (eventRequest == null)
            {
                return GenerateResponse(HttpStatusCode.NoContent, $"Invalid request: {hashId}.");
            }

            if (eventRequest.Expired && eventRequest.AutoApproved)
            {
                var lifeCycle = _eventService.GetEventRequestType(eventRequest.RequestTypeId).RequestLifeCycle;
                resultStatus = GenerateResponse(HttpStatusCode.BadRequest, $"Request was automatically approved within {lifeCycle} hours as no response was recorded.");

                // TODO ~ provide event reference number (https://unosquarebelfast.atlassian.net/browse/LM-58)
            }
            else if (eventRequest.Expired && !eventRequest.AutoApproved)
            {
                resultStatus = GenerateResponse(HttpStatusCode.BadRequest, $"Request expired: {hashId}");
            }
            else
            {
                resultStatus = EvaluateEventRequest(resultStatus, eventRequest);
            }

            return resultStatus ?? GenerateResponse(HttpStatusCode.NoContent, $"Invalid request: {hashId}.");
        }

        private ObjectResult EvaluateEventRequest(ObjectResult resultStatus, EventRequestDto eventRequest)
        {
            try
            {
                var eventId = _eventRequestLinkGenerator.Decode(eventRequest.Salt, eventRequest.Hash);
                var eventDto = _eventService.GetEvent(eventId);
                // TODO ~ approve by client once workflow impl is completed (https://unosquarebelfast.atlassian.net/browse/LM-2)
            }
            catch (Exception e)
            {
                resultStatus = GenerateResponse(HttpStatusCode.NotFound, $"Invalid request: {e.Message}.");
            }
            return resultStatus;
        }

        private ObjectResult GenerateResponse(HttpStatusCode statusCode, string message)
        {
            return StatusCode((int) statusCode, message);
        }
    }
}
