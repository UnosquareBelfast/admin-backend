using AdminCore.DTOs.Team;
using System.Collections.Generic;
using AdminCore.DTOs.Project;

namespace AdminCore.Common.Interfaces
{
  public interface IProjectService
  {
    ProjectDto CreateProject();
    ProjectDto UpdateProject();
    ProjectDto DeleteProject();

    IList<ProjectDto> GetProjects();
    IList<ProjectDto> GetProjectsById();
    IList<ProjectDto> GetProjectsByClientId();
  }
}
