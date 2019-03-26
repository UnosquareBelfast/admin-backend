using AdminCore.DAL;
using Quartz;
using Quartz.Spi;
using System;
using AdminCore.Common;

namespace AdminCore.ScheduledJobs
{
  public class DatabaseJobFactory : IJobFactory
  {
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
      var job = (IJob)Activator.CreateInstance(bundle.JobDetail.JobType);
      if (job is BaseDatabaseJob)
      {
        ((BaseDatabaseJob)job).DatabaseContext = ServiceLocator.Instance.GetInstance<IDatabaseContext>();
      }

      return job;
    }

    public void ReturnJob(IJob job)
    {
    }
  }
}