using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.Team;
using AutoFixture;

namespace AdminCore.Services.Tests.ClassData
{
    public class ContractServiceClassData
    {
        public class ProjectIdFixtureListDataGetContractByProjectId : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                var projectId = fixture.Create<int>();

                // ARGS: projectId: int, ormReturns: IList<Contract>, expectedReturnCount: int
                yield return new object[]
                {
                    projectId,
                    new List<Contract>
                    {
                        new Contract{Team = new Team{ProjectId = projectId}}
                    },
                    new List<ContractDto>
                    {
                        new ContractDto{Team = new TeamDto{ProjectId = projectId}}
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Contract>
                    {
                        new Contract{Team = new Team{ProjectId = projectId}},
                        new Contract{Team = new Team{ProjectId = projectId}}
                    },
                    new List<ContractDto>
                    {
                        new ContractDto{Team = new TeamDto{ProjectId = projectId}},
                        new ContractDto{Team = new TeamDto{ProjectId = projectId}}
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Contract>
                    {
                        new Contract{Team = new Team{ProjectId = projectId}},
                        new Contract{Team = new Team{ProjectId = projectId + 1}}
                    },
                    new List<ContractDto>
                    {
                        new ContractDto{Team = new TeamDto{ProjectId = projectId}}
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
