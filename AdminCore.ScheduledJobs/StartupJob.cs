using Quartz;
using System;
using System.Threading.Tasks;

namespace AdminCore.ScheduledJobs
{
  internal class StartupJob : BaseDatabaseJob, IJob
  {
    public Task Execute(IJobExecutionContext context)
    {
      var returnedValues = DatabaseContext.SchedulesRepository.GetSingle(x => x.IsActive);
      if (returnedValues != null)
      {
        //TODO Implement Job functionality
        Console.WriteLine("Run Job");
      }

      return null;
    }
  }
}