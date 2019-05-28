using System.Collections.Generic;
using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Project;
using AdminCore.Services.Base;
using AutoMapper;

namespace AdminCore.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private IMapper _mapper;
        public ProjectService(IMapper mapper, IDatabaseContext databaseContext) : base(databaseContext)
        {
            _mapper = mapper;
        }

        public ProjectDto CreateProject(ProjectDto projectToSave)
        {
            var project = _mapper.Map<Project>(projectToSave);
            var savedProject = DatabaseContext.ProjectRepository.Insert(project);
            return _mapper.Map<ProjectDto>(savedProject);
        }

        public void UpdateProject(ProjectDto projectToUpdate)
        {
            var project = _mapper.Map<Project>(projectToUpdate);
            DatabaseContext.ProjectRepository.Update(project);
        }

        public void DeleteProject(int projectId)
        {
            DatabaseContext.ProjectRepository.Delete(projectId);
        }

        public IList<ProjectDto> GetProjects()
        {
            var projectList = DatabaseContext.ProjectRepository.Get(null, null,
                project => project.Client,
                project => project.Teams,
                project => project.ParentProject);
            return _mapper.Map<IList<ProjectDto>>(projectList);
        }

        public IList<ProjectDto> GetProjectsById(int projectId)
        {
            var projectList = DatabaseContext.ProjectRepository.Get(project => project.ProjectId == projectId, null,
                project => project.Client,
                project => project.Teams,
                project => project.ParentProject);
            return _mapper.Map<IList<ProjectDto>>(projectList);
        }

        public IList<ProjectDto> GetProjectsByClientId(int clientId)
        {
            var projectList = DatabaseContext.ProjectRepository.Get(project => project.ClientId == clientId, null,
                project => project.Client,
                project => project.Teams,
                project => project.ParentProject);
            return _mapper.Map<IList<ProjectDto>>(projectList);
        }
    }
}
