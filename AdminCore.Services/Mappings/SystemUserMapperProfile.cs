using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.SystemUser;
using AutoMapper;

namespace AdminCore.Services.Mappings
{
  public class SystemUserMapperProfile : Profile
  {
    public SystemUserMapperProfile()
    {
      CreateMap<SystemUserDto, SystemUser>().ReverseMap();
      CreateMap<SystemUserRoleDto, SystemUserRole>().ReverseMap();
    }
  }
}
