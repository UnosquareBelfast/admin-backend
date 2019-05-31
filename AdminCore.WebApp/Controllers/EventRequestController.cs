using System;
using System.Net;
using AdminCore.Common.Interfaces;
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

            if (eventRequest.Expired)
            {
                resultStatus = GenerateResponse(HttpStatusCode.BadRequest, $"Request expired: {hashId}");
            }
            else if (eventRequest.AutoApproved)
            {
                var lifeCycle = _eventService.GetEventRequestType(eventRequest.RequestTypeId).RequestLifeCycle;
                resultStatus = GenerateResponse(HttpStatusCode.BadRequest, $"Request was automatically approved within {lifeCycle} hours as no response was recorded.");

                // TODO ~ provide event reference number
            }
            else
            {
                try
                {
                    var eventId = _eventRequestLinkGenerator.Decode(eventRequest.Salt, eventRequest.Hash);
                    var eventDto = _eventService.GetEvent(eventId);
                    // TODO ~ approve by client (use fs workflow)
                }
                catch (Exception e)
                {
                    resultStatus = GenerateResponse(HttpStatusCode.NotFound, $"Invalid request: {e.Message}.");
                }
            }

            return resultStatus ?? GenerateResponse(HttpStatusCode.NoContent, $"Invalid request: {hashId}.");
        }

        private ObjectResult GenerateResponse(HttpStatusCode statusCode, string message)
        {
            return StatusCode((int) statusCode, message);
        }
    }
}
