using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
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

                // ARGS: projectId: int, ormReturns: IList<Contract>, expectedReturnCount: int
                yield return new object[]
                {
                    ProjectId,
                    fixture.CreateMany<Contract>(1).ToList(),
                    1
                };
                yield return new object[]
                {
                    ProjectId,
                    fixture.CreateMany<Contract>(2).ToList(),
                    2
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private const int ProjectId = 5;
    }
}
