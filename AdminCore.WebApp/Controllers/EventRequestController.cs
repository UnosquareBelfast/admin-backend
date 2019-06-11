using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs.LinkGenerator;
using AdminCore.LinkGenerator.Interfaces;
using AdminCore.WebApi.Models.EventRequest;
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

        [HttpPost]
        [Route("{requestType:regex(\\b(0|1)\\b)}/{hashId:length(16)}")]
        public IActionResult PostEventRequestResponse(EventRequestResponseViewModel eventRequestResponse, int requestType, string hashId)
        {
            var requestResponse = eventRequestResponse.Comment;

            ObjectResult resultStatus;

            var eventRequest = _eventService.GetEventRequest(hashId);

            if (eventRequest == null)
            {
                return GenerateResponse(HttpStatusCode.NoContent, $"Invalid request: {hashId}.");
            }

            // TODO ~ provide event reference number (https://unosquarebelfast.atlassian.net/browse/LM-58)
            if (eventRequest.AutoApproved)
            {
                var lifeCycle = _eventService.GetEventRequestType(eventRequest.RequestTypeId).RequestLifeCycle;
                resultStatus = GenerateResponse(HttpStatusCode.BadRequest, $"Request was automatically approved within {lifeCycle} hours as no response was recorded.");
            }
            else
            {
                resultStatus = EvaluateEventRequest(eventRequest);
            }

            return resultStatus ?? GenerateResponse(HttpStatusCode.NoContent, $"Invalid request: {hashId}.");
        }

        private ObjectResult EvaluateEventRequest(EventRequestDto eventRequest)
        {
            ObjectResult resultStatus = null;

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
