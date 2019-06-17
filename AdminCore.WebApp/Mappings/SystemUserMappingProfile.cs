using AdminCore.DTOs.SystemUser;
using AdminCore.WebApi.Models.SystemUser;
using AutoMapper;

namespace AdminCore.WebApi.Mappings
{
    public class SystemUserMappingProfile : Profile
    {
        public SystemUserMappingProfile()
        {
            CreateMap<SystemUserDto, SystemUserViewModel>().ReverseMap();

            CreateMap<SystemUserRoleDto, SystemUserRoleViewModel>().ReverseMap();
        }
    }
}
