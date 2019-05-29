using AdminCore.DTOs.Team;
using System.Collections.Generic;
using AdminCore.DTOs.Project;

namespace AdminCore.Common.Interfaces
{
  public interface IProjectService
  {
    bool CreateProject(ProjectDto projectToSave, out ProjectDto createdProject);
    bool UpdateProject(ProjectDto projectToUpdate, out ProjectDto updatedProject);
    bool DeleteProject(int projectId);

    IList<ProjectDto> GetProjects();
    IList<ProjectDto> GetProjectsById(int projectId);
    IList<ProjectDto> GetProjectsByClientId(int clientId);
  }
}
