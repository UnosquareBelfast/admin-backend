
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DTOs.Project;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Models.Project;
using AdminCore.WebApi.Models.Team;
using AutoFixture;

namespace AdminCore.WebApi.Tests.ClassData
{
    public class ProjectControllerClassData
    {
        public class ListOfSpecificIdProjectDtosViewModelsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS: projectId: int, serviceReturns: IList<ProjectDto>, expectedReturnCount: int
                yield return new object[]
                {
                    ProjectId,
                    fixture.CreateMany<ProjectDto>(1).ToList(),
                    fixture.CreateMany<ProjectViewModel>(1).ToList(),
                    1
                };
                yield return new object[]
                {
                    ProjectId,
                    fixture.CreateMany<ProjectDto>(2).ToList(),
                    fixture.CreateMany<ProjectViewModel>(2).ToList(),
                    2
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class ListOfProjectDtosCreateViewModelsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS: controllerInput: CreateProjectViewModel, serviceReturns: ProjectDto
                yield return new object[]
                {
                    fixture.Create<CreateProjectViewModel>(),
                    fixture.Create<ProjectDto>()
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class ListOfProjectDtosUpdateViewModelsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS: controllerInput: UpdateProjectViewModel, serviceReturns: ProjectDto
                yield return new object[]
                {
                    fixture.Create<UpdateProjectViewModel>(),
                    fixture.Create<ProjectDto>()
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class RandomProjectIdClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS: projectId: int
                yield return new object[]
                {
                    fixture.Create<int>()
                };
                yield return new object[]
                {
                    fixture.Create<int>()
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private const int ProjectId = 5;
    }
}
