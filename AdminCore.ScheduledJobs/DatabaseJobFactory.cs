using AdminCore.DAL;
using Quartz;
using Quartz.Spi;
using System;

namespace AdminCore.ScheduledJobs
{
  public class DatabaseJobFactory : IJobFactory
  {
    private readonly IDatabaseContext _databaseContext;

    public DatabaseJobFactory(IDatabaseContext databaseContext)
    {
      _databaseContext = databaseContext;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
      var job = (IJob)Activator.CreateInstance(bundle.JobDetail.JobType);
      if (job is BaseDatabaseJob)
      {
        ((BaseDatabaseJob)job).DatabaseContext = _databaseContext;
      }

      return job;
    }

    public void ReturnJob(IJob job)
    {
    }
  }
}