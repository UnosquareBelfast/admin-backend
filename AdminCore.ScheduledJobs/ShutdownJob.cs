using Quartz;
using System.Threading.Tasks;

namespace AdminCore.ScheduledJobs
{
  internal class ShutdownJob : BaseDatabaseJob, IJob
  {
    public Task Execute(IJobExecutionContext context)
    {
      var resourceGroupName = context.JobDetail.JobDataMap.GetString("ResourceGroupName");
      var returnedValues = DatabaseContext.SchedulesRepository.GetSingle(x => x.IsActive);
      if (returnedValues != null)
      {
        // DO SOMETHING
      }

      return null;
    }
  }
}