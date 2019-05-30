using System.Collections;
using System.Collections.Generic;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Project;
using AutoFixture;

namespace AdminCore.Services.Tests.ClassData
{
    public class ProjectServiceClassData
    {
        public class RandomProjectIdProjectsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                var projectId = fixture.Create<int>();

                // ARGS projectId: int, dbReturns: IList<Project>, serviceReturns: IList<ProjectDto>
                yield return new object[]
                {
                    projectId,
                    new List<Project>
                    {
                        fixture.Build<Project>().With(x => x.ProjectId, projectId).Create()
                    },
                    new List<ProjectDto>
                    {
                        fixture.Build<ProjectDto>().With(x => x.ProjectId, projectId).Create()
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Project>
                    {
                        fixture.Build<Project>().With(x => x.ProjectId, projectId).Create(),
                        fixture.Build<Project>().With(x => x.ProjectId, projectId).Create()
                    },
                    new List<ProjectDto>
                    {
                        fixture.Build<ProjectDto>().With(x => x.ProjectId, projectId).Create(),
                        fixture.Build<ProjectDto>().With(x => x.ProjectId, projectId).Create()
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Project>
                    {
                        fixture.Build<Project>().With(x => x.ProjectId, projectId).Create(),
                        fixture.Build<Project>().With(x => x.ProjectId, projectId + 1).Create()
                    },
                    new List<ProjectDto>
                    {
                        fixture.Build<ProjectDto>().With(x => x.ProjectId, projectId).Create()
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Project>(),
                    new List<ProjectDto>()
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class InsertUpdateProjectRandomProjectDtoProject : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS projectId: int, dbReturns: IList<Project>, serviceReturns: IList<ProjectDto>
                yield return new object[]
                {
                    fixture.Create<ProjectDto>(),
                    fixture.Create<Project>()
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
