using AdminCore.DTOs.Project;
using AdminCore.WebApi.Models.Project;
using AutoMapper;

namespace AdminCore.WebApi.Mappings
{
    public class ProjectMappingProfileProfile : Profile
    {
        public ProjectMappingProfileProfile()
        {
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
            CreateMap<ProjectDto, CreateProjectViewModel>().ReverseMap();
            CreateMap<ProjectDto, UpdateProjectViewModel>().ReverseMap();
        }
    }
}
