using System.Net;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs.Employee;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdminCore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventRequestController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly EmployeeDto _employee;
        private readonly IDateService _dateService;

        public EventRequestController(IEventService wfhEventService, IMapper mapper, IAuthenticatedUser authenticatedUser, IDateService dateService)
            : base(mapper)
        {
            _eventService = wfhEventService;
            _mapper = mapper;
            _employee = authenticatedUser.RetrieveLoggedInUser();
            _dateService = dateService;
        }
        
        [HttpGet("event-response/{hashId}")]
        public IActionResult GetEventByEventId(string hashId)
        {
            var eventRequest = _eventService.GetEventRequest(hashId);

            if (eventRequest != null)
            {
               // todo evaluate request
            }

            return StatusCode((int)HttpStatusCode.NoContent, $"Invalid request: { hashId }");
        }
    }
}
