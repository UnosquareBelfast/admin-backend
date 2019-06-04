using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

                // ARGS projectId: int, dbReturns: IList<Project>, serviceReturns: IList<ProjectDto>, expectedReturnCount: int
                yield return new object[]
                {
                    projectId,
                    fixture.CreateMany<Project>(1).ToList(),
                    fixture.CreateMany<ProjectDto>(1).ToList(),
                    1
                };
                yield return new object[]
                {
                    projectId,
                    fixture.CreateMany<Project>(2).ToList(),
                    fixture.CreateMany<ProjectDto>(2).ToList(),
                    2
                };
                yield return new object[]
                {
                    projectId,
                    new List<Project>(),
                    new List<ProjectDto>(),
                    0
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
