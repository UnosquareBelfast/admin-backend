using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AutoMapper;

namespace AdminCore.Services.Mappings
{
  public class ContractMapperProfile : Profile
  {
    public ContractMapperProfile()
    {
      CreateMap<Contract, ContractDto>()
        .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Team.Project.Client.ClientName))
        .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Team.Project.ProjectName))
        .ReverseMap();
    }
  }
}
