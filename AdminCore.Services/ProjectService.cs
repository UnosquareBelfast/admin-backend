using System;
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
        private readonly IMapper _mapper;
        public ProjectService(IMapper mapper, IDatabaseContext databaseContext) : base(databaseContext)
        {
            _mapper = mapper;
        }

        public bool CreateProject(ProjectDto projectToSave, out ProjectDto createdProject)
        {
            try
            {
                var project = _mapper.Map<Project>(projectToSave);
                var savedProject = DatabaseContext.ProjectRepository.Insert(project);
                DatabaseContext.SaveChanges();

                createdProject = _mapper.Map<ProjectDto>(savedProject);
                return true;
            }
            catch (Exception e)
            {
                createdProject = null;
                return false;
            }
        }

        public bool UpdateProject(ProjectDto projectToUpdate, out ProjectDto updatedProject)
        {
            try
            {
                var project = _mapper.Map<Project>(projectToUpdate);
                var updateProject = DatabaseContext.ProjectRepository.Update(project);
                DatabaseContext.SaveChanges();

                updatedProject = _mapper.Map<ProjectDto>(updateProject);
                return true;
            }
            catch (Exception e)
            {
                updatedProject = null;
                return false;
            }
        }

        public bool DeleteProject(int projectId)
        {
            try
            {
                DatabaseContext.ProjectRepository.Delete(projectId);
                DatabaseContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IList<ProjectDto> GetProjects()
        {
            var projectList = DatabaseContext.ProjectRepository.Get(null, null,
                project => project.Client,
                project => project.Teams,
                project => project.ParentProject);
            return _mapper.Map<IList<ProjectDto>>(projectList ?? new List<Project>());
        }

        public IList<ProjectDto> GetProjectsById(int projectId)
        {
            var projectList = DatabaseContext.ProjectRepository.Get(project => project.ProjectId == projectId, null,
                project => project.Client,
                project => project.Teams,
                project => project.ParentProject);
            return _mapper.Map<IList<ProjectDto>>(projectList ?? new List<Project>());
        }

        public IList<ProjectDto> GetProjectsByClientId(int clientId)
        {
            var projectList = DatabaseContext.ProjectRepository.Get(project => project.ClientId == clientId, null,
                project => project.Teams,
                project => project.ParentProject);
            return _mapper.Map<IList<ProjectDto>>(projectList ?? new List<Project>());
        }
    }
}
