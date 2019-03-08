﻿using System;
using AdminCore.Common;
using AdminCore.Common.Interfaces;
using AdminCore.ScheduledJobs;
using AdminCore.Services.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminCore.JobExecutor
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      DependencyInjection.RegisterDependencyInjection(null,
        new ServiceDescriptor(typeof(IScheduleRunner), typeof(ScheduleRunner), ServiceLifetime.Singleton)
      );

      var scheduleRunner = ServiceLocator.Instance.GetInstance<ISchedulesService>();

      scheduleRunner.QueueAllExistingSchedules();

      Console.ReadKey();
    }
  }
}