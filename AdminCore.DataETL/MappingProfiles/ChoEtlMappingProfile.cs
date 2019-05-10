using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DataETL.Models;
using AdminCore.DAL.Models;
using AutoMapper;

namespace AdminCore.DataETL.MappingProfiles
{
    public class ChoEtlMappingProfile : Profile
    {
        public ChoEtlMappingProfile()
        {
            CreateMap<Event, EventChoEtl>()
                .ForMember(dest => dest.EventDateStart, opt => opt.MapFrom(src => GetStartDateFromCollection(src.EventDates)))
                .ForMember(dest => dest.EventDateEnd, opt => opt.MapFrom(src => GetEndDateFromCollection(src.EventDates)));
            CreateMap<EventMessageChoEtl, EventMessage>();
            CreateMap<EventDateChoEtl, EventDate>();
            CreateMap<EmployeeChoEtl, Employee>();
            CreateMap<Employee, string>().ConvertUsing(x => $"{x.Forename} {x.Surname}");
        }

        private DateTime GetStartDateFromCollection(ICollection<EventDate> col)
        {
            return col.FirstOrDefault().StartDate;
        }
        
        private DateTime GetEndDateFromCollection(ICollection<EventDate> col)
        {
            return col.LastOrDefault().EndDate;
        }
    }
}