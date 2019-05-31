using System.Threading.Tasks;
using Quartz;

namespace AdminCore.ScheduledJobs
{
    /// <summary>
    /// - Determines if event request has expired by evaluating time_created and time_expires values.
    /// - If event request expired, "expired" flag is set to true. Then auto-approves the event & sets "auto_approved" flag to true.
    /// - This job skips already expired events, runs recurrently every 10 minutes.
    /// </summary>
    public class ValidateEventRequestsJob : BaseDatabaseJob, IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        private Task ValidateEventRequests()
        {
            throw new System.NotImplementedException();
        }
    }
}
