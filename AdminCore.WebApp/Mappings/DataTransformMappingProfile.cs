using System;
using System.Collections.Generic;
using System.Linq;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Event;
using AdminCore.WebApi.Models;
using AdminCore.WebApi.Models.DataTransform;
using AutoMapper;

namespace AdminCore.WebApi.Mappings
{
  public class DataTransformMappingProfile : Profile
  {
    public DataTransformMappingProfile()
    {
      CreateMap<EventDto, EventDataTransformModel>()
        .ForMember(dest => dest.EventDateStart, opt => opt.MapFrom(src => GetStartDateFromCollection(src.EventDates)))
        .ForMember(dest => dest.EventDateEnd, opt => opt.MapFrom(src => GetEndDateFromCollection(src.EventDates)))
        .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => $"{src.Employee.Forename} {src.Employee.Surname}"))
        .ForMember(dest => dest.EventMessages, 
          opt => opt.MapFrom(src => src.EventMessages.Select(
            x => $"{(EventMessageTypes)x.EventMessageTypeId}: {x.Message} by {src.Employee.Forename} {src.Employee.Surname}").ToList()));
    }
    
    private DateTime GetStartDateFromCollection(ICollection<EventDateDto> col)
    {
      return col.FirstOrDefault().StartDate;
    }
        
    private DateTime GetEndDateFromCollection(ICollection<EventDateDto> col)
    {
      return col.LastOrDefault().EndDate;
    }
  }
}