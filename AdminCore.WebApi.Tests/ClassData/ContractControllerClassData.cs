using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DTOs;
using AutoFixture;

namespace AdminCore.WebApi.Tests.ClassData
{
    public class ContractControllerClassData
    {
        public class ProjectIdFixtureListDataGetContractByProjectId : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS: projectId: int, serviceReturns: IList<ContractViewModel>, expectedReturnCount: int
                yield return new object[]
                {
                    ProjectId,
                    fixture.CreateMany<ContractDto>(1).ToList(),
                    1
                };
                yield return new object[]
                {
                    ProjectId,
                    fixture.CreateMany<ContractDto>(2).ToList(),
                    2
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private const int ProjectId = 5;
    }
}
