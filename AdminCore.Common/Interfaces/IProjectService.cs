using AdminCore.DTOs.Team;
using System.Collections.Generic;
using AdminCore.DTOs.Project;

namespace AdminCore.Common.Interfaces
{
  public interface IProjectService
  {
    ProjectDto CreateProject(ProjectDto projectToCreate);
    ProjectDto UpdateProject(ProjectDto projectToUpdate);
    void DeleteProject(int projectId);

    IList<ProjectDto> GetProjects();
    IList<ProjectDto> GetProjectsById(int projectId);
    IList<ProjectDto> GetProjectsByClientId(int clientId);
  }
}
