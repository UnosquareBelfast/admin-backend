using AdminCore.Common.Interfaces;
using AdminCore.DTOs;
using Quartz;
using Quartz.Impl;
using System;

namespace AdminCore.ScheduledJobs
{
  public class ScheduleRunner : IScheduleRunner, IDisposable
  {
    private readonly IScheduler _scheduler;

    public ScheduleRunner()
    {
      var factory = new StdSchedulerFactory();
      _scheduler = factory.GetScheduler().Result;
      _scheduler.Start();

      _scheduler.JobFactory = new DatabaseJobFactory();
    }

    public void Dispose()
    {
      _scheduler.Shutdown(true);
      GC.SuppressFinalize(this);
    }

    public void StartSchedule(ScheduleDto schedule)
    {
      if (schedule.IsActive)
      {
        var trigger = GetTriggerForScheduler(schedule);
        var job = GetJob(schedule);
        _scheduler.ScheduleJob(job, trigger);
      }
    }

    private static IJobDetail GetJob(ScheduleDto schedule)
    {
      if (schedule.IsActive)
      {
        return GetJobForScheduler<PruneExpiredEventsJob>(schedule);
      }

      throw new Exception("No active schedule");
    }

    private static IJobDetail GetJobForScheduler<T>(ScheduleDto schedule) where T : IJob
    {
      var job = JobBuilder.Create<T>()
              .WithIdentity(GetJobKeyForSchedule(schedule))
              .Build();

      return job;
    }

    private static JobKey GetJobKeyForSchedule(ScheduleDto schedule)
    {
      return JobKey.Create(nameof(schedule.ScheduleId), schedule.ScheduleId.ToString());
    }

    private static ITrigger GetTriggerForScheduler(ScheduleDto schedule)
    {
      var trigger = TriggerBuilder.Create()
        .WithIdentity(GetTriggerKeyForSchedule(schedule))
        .WithCronSchedule(string.Format(schedule.ScheduleCronExpression))
        .Build();

      return trigger;
    }

    private static TriggerKey GetTriggerKeyForSchedule(ScheduleDto schedule)
    {
      return new TriggerKey(nameof(schedule.ScheduleId), schedule.ScheduleId.ToString());
    }
  }
}