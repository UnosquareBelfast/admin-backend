using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DTOs.Employee;
using AutoFixture;
using Microsoft.CodeAnalysis;

namespace AdminCore.WebApi.Tests.ClassData
{
    public class EventControllerClassData
    {
        public class DefaultAuthenticatedUser : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS projectId: int, dbReturns: IList<Project>, serviceReturns: IList<ProjectDto>, expectedReturnCount: int
                yield return new object[]
                {
                    new EmployeeDto
                    {
                        EmployeeId = fixture.Create<int>()
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
