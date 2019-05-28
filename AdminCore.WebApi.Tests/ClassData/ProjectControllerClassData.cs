
using System.Collections;
using System.Collections.Generic;
using AdminCore.DTOs.Project;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Models.Project;
using AdminCore.WebApi.Models.Team;
using AutoFixture;

namespace AdminCore.WebApi.Tests.ClassData
{
    public class ProjectControllerClassData
    {
        public class GetProjectById_ServiceContainsListOfTeams_ReturnsOkWithTeamsInBodyClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS: projectId: int, serviceReturns: IList<ProjectDto>, controllerReturns: IList<ProjectViewModel>
                yield return new object[]
                {
                    ProjectId,
                    new List<ProjectDto>
                    {
                        fixture.Build<ProjectDto>().With(project => project.ProjectId, ProjectId).Create()
                    },
                    new List<ProjectViewModel>
                    {
                        fixture.Build<ProjectViewModel>().With(project => project.ProjectId, ProjectId).Create()
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private const int ProjectId = 5;
    }
}
