using System.Collections;
using System.Collections.Generic;

namespace AdminCore.Services.Tests.ClassData
{
    public class FsmWorkflowHandlerData
    {       
        public class CreateEventWorkflow_SaveChangesToDbContextIsTrue_NewEventWorkflowIsInsertedIntoDb : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // Last date to book by is 7 days in advance at midnight.
                yield return new object[] { TestClassBuilder.AnnualLeaveEventType().EventTypeId };
                yield return new object[] { TestClassBuilder.WorkingFromHomeEventType().EventTypeId };

            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class CreateEventWorkflow_EventTypesProvidedDoNotHaveAssociatedWorkflow_ThrowsWorkflowException : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // Last date to book by is 7 days in advance at midnight.
                yield return new object[] { 46 };
                yield return new object[] { 99 };
                yield return new object[] { 0 };

            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}