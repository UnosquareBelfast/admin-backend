using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DTOs.EventWorkflow;
using AdminCore.Services.Base;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AutoMapper;
using FsmTestApp.FsmMachines;

namespace AdminCore.Services
{
    public class EventWorkflowService : IEventWorkflowService
    {
        private IMapper _mapper;
        public EventWorkflowService(IMapper mapper)
        {
            _mapper = mapper;
        }
        
        public EventWorkflowDto CreateEventWorkflow(int eventId, EventTypes eventType)
        {
            var workflow = new EventWorkflow
            {
                EventId = eventId,
                WorkflowSerializedState = GetSerializedWorkflow(eventType)
            };
            
            return _mapper.Map<EventWorkflowDto>(workflow);
        }

        private string GetSerializedWorkflow(EventTypes eventType)
        {
            switch (eventType)
            {
                case EventTypes.AnnualLeave:
                    return new WorkflowFsmPto().ToJson();
            }
        }
    }
}