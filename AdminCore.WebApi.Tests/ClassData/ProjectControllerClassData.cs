
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
        public class ListOfSpecificIdProjectDtosViewModelsClassData : IEnumerable<object[]>
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
                        fixture.Create<ProjectDto>()
                    },
                    new List<ProjectViewModel>
                    {
                        fixture.Create<ProjectViewModel>()
                    }
                };
                yield return new object[]
                {
                    ProjectId,
                    new List<ProjectDto>
                    {
                        fixture.Create<ProjectDto>(),
                        fixture.Create<ProjectDto>()
                    },
                    new List<ProjectViewModel>
                    {
                        fixture.Create<ProjectViewModel>(),
                        fixture.Create<ProjectViewModel>()
                    }
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

                // ARGS: controllerInput: CreateProjectViewModel, serviceReturns: ProjectDto, controllerReturns: ProjectViewModel
                yield return new object[]
                {
                    fixture.Create<CreateProjectViewModel>(),
                    fixture.Create<ProjectDto>(),
                    fixture.Create<ProjectViewModel>()
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

                // ARGS: controllerInput: UpdateProjectViewModel, serviceReturns: ProjectDto, controllerReturns: ProjectViewModel
                yield return new object[]
                {
                    fixture.Create<UpdateProjectViewModel>(),
                    fixture.Create<ProjectDto>(),
                    fixture.Create<ProjectViewModel>()
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
