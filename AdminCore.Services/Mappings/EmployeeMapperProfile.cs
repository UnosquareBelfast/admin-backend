using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.SystemUser;
using AutoMapper;

namespace AdminCore.Services.Mappings
{
  public class EmployeeMapperProfile : Profile
  {
    public EmployeeMapperProfile()
    {
      CreateMap<CountryDto, Country>().ReverseMap();
      CreateMap<Employee, EmployeeDto>();
      CreateMap<EmployeeDto, Employee>();
      CreateMap<EmployeeStatusDto, EmployeeStatus>().ReverseMap();
    }
  }
}
