using AdminCore.DAL.Models;
using AdminCore.DTOs.EventWorkflow;
using AutoMapper;

namespace AdminCore.Services.Mappings
{
    public class EventWorkflowMapperProfile : Profile
    {
        public EventWorkflowMapperProfile()
        {
            CreateMap<EventWorkflow, EventWorkflowDto>().ReverseMap();
            CreateMap<SystemUserApprovalResponse, SystemUserApprovalResponseDto>().ReverseMap();
        }
    }
}