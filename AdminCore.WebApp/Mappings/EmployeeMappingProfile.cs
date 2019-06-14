using AdminCore.DTOs;
using AdminCore.DTOs.Client;
using AdminCore.DTOs.Dashboard;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.DTOs.EventMessage;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Models;
using AdminCore.WebApi.Models.Client;
using AdminCore.WebApi.Models.Contract;
using AdminCore.WebApi.Models.Dashboard;
using AdminCore.WebApi.Models.Employee;
using AdminCore.WebApi.Models.Event;
using AdminCore.WebApi.Models.EventMessage;
using AdminCore.WebApi.Models.Team;
using AutoMapper;

namespace AdminCore.WebApi.Mappings
{
  public class EmployeeMappingProfile : Profile
  {
    public EmployeeMappingProfile()
    {
      CreateMap<EmployeeViewModel, EmployeeDto>().ReverseMap();
      CreateMap<EmployeeViewModel, EmployeeDto>().ReverseMap();
      CreateMap<UpdateEmployeeViewModel, EmployeeDto>();
      CreateMap<UpdateEmployeeViewModel, EmployeeDto>().ReverseMap();
      CreateMap<RegisterEmployeeViewModel, EmployeeDto>();
    }
  }
}
