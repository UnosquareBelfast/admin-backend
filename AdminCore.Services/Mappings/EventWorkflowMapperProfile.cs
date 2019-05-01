using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.EventWorkflow;
using AutoMapper;

namespace AdminCore.Services.Mappings
{
    public class EventWorkflowMapperProfile : Profile
    {
        public EventWorkflowMapperProfile()
        {
            CreateMap<EventWorkflow, EventWorkflowDto>().ReverseMap();
            CreateMap<ApprovalStatus, ApprovalStatusDto>().ReverseMap();
            CreateMap<EmployeeApprovalResponse, EmployeeApprovalResponseDto>().ReverseMap();
        }
    }
}