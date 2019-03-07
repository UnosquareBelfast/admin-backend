﻿using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace AdminCore.ScheduledJobs
{
  public class SchedulingManager

  {
    private static void Main()
    {
      LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

      RunProgramRunExample().GetAwaiter().GetResult();

      Console.WriteLine("Press any key to close the application");
      Console.ReadKey();
    }

    private static async Task RunProgramRunExample()
    {
      try
      {
        // Grab the Scheduler instance from the Factory
        NameValueCollection props = new NameValueCollection
        {
          {"quartz.serializer.type", "binary"}
        };
        StdSchedulerFactory factory = new StdSchedulerFactory(props);
        IScheduler scheduler = await factory.GetScheduler();

        // and start it off
        await scheduler.Start();

        // define the job and tie it to our HelloJob class
        IJobDetail job = JobBuilder.Create<PruneEventJob>()
          .WithIdentity("pruneEventJob", "EventManagementGroup")
          .Build();

        // Trigger the job to run now, and then repeat every 10 seconds
        ITrigger trigger = TriggerBuilder.Create()
          .WithIdentity("trigger1", "EventManagementGroup")
          .StartNow()
          .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10)
            .RepeatForever())
          .Build();

        /*        CRON EXPRESSION
                  TriggerBuilder.Create()
                  .WithIdentity("pruneEventTrigger", "EventManagementGroup")
                  .WithCronSchedule(string.Format("0 {0} {1} ? * *", 0, 13))
                  .Build();
        */

        // Tell quartz to schedule the job using our trigger
        await scheduler.ScheduleJob(job, trigger);

        // some sleep to show what's happening
        await Task.Delay(TimeSpan.FromSeconds(60));

        // and last shut down the scheduler when you are ready to close your program
        await scheduler.Shutdown();
      }
      catch (SchedulerException se)
      {
        Console.WriteLine(se);
      }
    }

    // simple log provider to get something to the console
    private class ConsoleLogProvider : ILogProvider
    {
      public Logger GetLogger(string name)
      {
        return (level, func, exception, parameters) =>
        {
          if (level >= LogLevel.Info && func != null)
          {
            Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
          }

          return true;
        };
      }

      public IDisposable OpenNestedContext(string message)
      {
        throw new NotImplementedException();
      }

      public IDisposable OpenMappedContext(string key, string value)
      {
        throw new NotImplementedException();
      }
    }
  }

  public class PruneEventJob : IJob
  {
    /* THIS IS CAUSING ISSUES
        private readonly IJobService _jobService;

        public PruneEventJob(IJobService jobService)
        {
          _jobService = jobService;
        }
    */
    //private readonly IJobService _jobService;

    /*    public PruneEventJob(IDatabaseContext databaseContext, IJobService jobService) : base(databaseContext)
        {
          _jobService = jobService;
        }*/

    public async Task Execute(IJobExecutionContext context)
    {
      await Console.Out.WriteLineAsync("Pruning");
      //_jobService.PruneCancelledEvents();
      //await Console.Out.WriteLineAsync("Pruned");
    }

    /*    public PruneEventJob(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }*/
  }
}

/*  public class PruneEventJob : IJob
  {
    public async Task Execute(IJobExecutionContext context)
    {
      var events = DatabaseContext.EventRepository.Get(x => eventIds.Contains(x.EventId)
                                                            && x.EventTypeId == eventTypeId,
    }
*/