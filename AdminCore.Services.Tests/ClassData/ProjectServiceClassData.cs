using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Client;
using AdminCore.DTOs.Project;
using AdminCore.DTOs.Team;
using AutoFixture;

namespace AdminCore.Services.Tests.ClassData
{
    public class ProjectServiceClassData
    {
        public class GetAllRandomProjectIdProjectsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                var projectId = fixture.Create<int>();

                // ARGS dbReturns: IList<Project>, serviceExpected: IList<ProjectDto>
                yield return new object[]
                {
                    new List<Project>
                    {
                        new Project{ProjectId = projectId}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId}
                    }
                };
                yield return new object[]
                {
                    new List<Project>
                    {
                        new Project{ProjectId = projectId},
                        new Project{ProjectId = projectId}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId},
                        new ProjectDto{ProjectId = projectId}
                    }
                };
                yield return new object[]
                {
                    new List<Project>
                    {
                        new Project{ProjectId = projectId},
                        new Project{ProjectId = projectId + 1}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId},
                        new ProjectDto{ProjectId = projectId + 1}
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class GetByProjectIdRandomProjectIdProjectsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                var projectId = fixture.Create<int>();

                // ARGS projectId: int, dbReturns: IList<Project>, serviceExpected: IList<ProjectDto>
                yield return new object[]
                {
                    projectId,
                    new List<Project>
                    {
                        new Project{ProjectId = projectId}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId}
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Project>
                    {
                        new Project{ProjectId = projectId},
                        new Project{ProjectId = projectId}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId},
                        new ProjectDto{ProjectId = projectId}
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Project>
                    {
                        new Project{ProjectId = projectId},
                        new Project{ProjectId = projectId + 1}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId}
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class GetByClientIdRandomProjectIdProjectsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                var clientId = fixture.Create<int>();

                // ARGS projectId: int, dbReturns: IList<Project>, serviceExpected: IList<ProjectDto>
                yield return new object[]
                {
                    clientId,
                    new List<Project>
                    {
                        new Project{ClientId = clientId}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ClientId = clientId}
                    }
                };
                yield return new object[]
                {
                    clientId,
                    new List<Project>
                    {
                        new Project{ClientId = clientId},
                        new Project{ClientId = clientId}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ClientId = clientId},
                        new ProjectDto{ClientId = clientId}
                    }
                };
                yield return new object[]
                {
                    clientId,
                    new List<Project>
                    {
                        new Project{ClientId = clientId},
                        new Project{ClientId = clientId + 1}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ClientId = clientId}
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class GetByProjectIdClientIdRandomProjectsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                var projectId = fixture.Create<int>();
                var clientId = fixture.Create<int>();

                // ARGS projectId: int, dbReturns: IList<Project>, serviceExpected: IList<ProjectDto>
                yield return new object[]
                {
                    projectId,
                    clientId,
                    new List<Project>
                    {
                        new Project{ProjectId = projectId, ClientId = clientId}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId, ClientId = clientId}
                    }
                };
                yield return new object[]
                {
                    projectId,
                    clientId,
                    new List<Project>
                    {
                        new Project{ProjectId = projectId, ClientId = clientId},
                        new Project{ProjectId = projectId, ClientId = clientId}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId, ClientId = clientId},
                        new ProjectDto{ProjectId = projectId, ClientId = clientId}
                    }
                };
                yield return new object[]
                {
                    projectId,
                    clientId,
                    new List<Project>
                    {
                        new Project{ProjectId = projectId, ClientId = clientId},
                        new Project{ProjectId = projectId + 2, ClientId = clientId + 2}
                    },
                    new List<ProjectDto>
                    {
                        new ProjectDto{ProjectId = projectId, ClientId = clientId}
                    }
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
                    new ProjectDto
                    {
                        ProjectId = 25,
                        ClientId = 20,
                        ProjectName = "Unique Project",
                        ProjectParentId = null,
                        Teams = new List<TeamDto>{new TeamDto{TeamId = 86}},
                        Client = new ClientDto{ClientId = 20, ClientName = "Unique Client"},
                        ParentProject = null
                    },
                    new Project
                    {
                        ProjectId = 25,
                        ClientId = 20,
                        ProjectName = "Unique Project",
                        ProjectParentId = null,
                        Teams = new List<Team>{new Team{TeamId = 86}},
                        Client = new Client{ClientId = 20, ClientName = "Unique Client"},
                        ParentProject = null
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
