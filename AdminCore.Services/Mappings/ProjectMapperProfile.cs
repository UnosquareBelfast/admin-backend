using AdminCore.DAL.Models;
using AdminCore.DTOs.Client;
using AdminCore.DTOs.Project;
using AutoMapper;

namespace AdminCore.Services.Mappings
{
  public class ProjectMapperProfile : Profile
  {
    public ProjectMapperProfile()
    {
      AllowNullCollections = true;
      AllowNullDestinationValues = true;

      CreateMap<ProjectDto, Project>();
      CreateMap<Project, ProjectDto>();
    }
  }
}
