using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AutoMapper;

namespace AdminCore.Services.Mappings
{
  public class ScheduleMapperProfile : Profile
  {
    public ScheduleMapperProfile()
    {
      CreateMap<Schedule, ScheduleDto>().ReverseMap();
    }
  }
}