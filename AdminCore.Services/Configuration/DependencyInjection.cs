using AdminCore.Common;
using AdminCore.Common.DependencyInjection;
using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using AdminCore.DataETL;
using AdminCore.LinkGenerator.Interfaces;
using AdminCore.LinkGenerator.LinkGenerators;
using AdminCore.MailClients.Interfaces;
using AdminCore.MailClients.SMTP;
using AdminCore.MailClients.SMTP.Adapters;
using AdminCore.MailClients.SMTP.Configuration;
using AdminCore.MailClients.SMTP.Interfaces;

namespace AdminCore.Services.Configuration
{
  [ExcludeFromCodeCoverage]
  public static class DependencyInjection
  {
    private static bool _registered;

    public static void RegisterWebDependencyInjection(IServiceCollection services = null)
    {
      if (!_registered)
      {
        if (services == null) services = new ServiceCollection();
        services.AddAutoMapper();
        services.AddSingleton<ILoggerFactory, LoggerFactory>();
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        services.AddDbContext<AdminCoreContext>();
        services.AddScoped<IDatabaseContext, EntityFrameworkContext>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IAuthenticatedUser, AuthenticatedUser>();
        services.AddTransient<IDataMigration, EvolveDataMigration>();
        services.AddTransient<IDateService, DateService>();
        services.AddSingleton<IConfiguration, ConfigurationProvider>();
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<IClientService, ClientService>();
        services.AddTransient<ITeamService, TeamService>();
        services.AddTransient<IEventService, EventService>();
        services.AddTransient<IDashboardService, DashboardService>();
        services.AddTransient<IContractService, ContractService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IEventMessageService, EventMessageService>();

        services.AddTransient<IFileTransformAdapter, CsvFileTransformAdapter>();
        services.AddTransient<ICsvService, CsvService>();

        services.AddScoped<ISmtpClient, SmtpMailKitClientAdapter>();
        services.AddScoped<IMailSender, SmtpMailSender>();
        services.AddSingleton<IMailServerConfiguration, SmtpServerConfiguration>();

        services.AddScoped<ILinkGeneratorFactory, EventRequestFactory>();

        ServiceLocator.Instance = new DependencyInjectionContainer(services.BuildServiceProvider());

        _registered = true;
      }
    }

    public static void RegisterJobsDependencyInjection(params ServiceDescriptor[] serviceDescriptors)
    {
      if (!_registered)
      {
        var services = new ServiceCollection().AddAutoMapper();
        services.AddDbContext<AdminCoreContext>();
        services.AddTransient<IDatabaseContext, EntityFrameworkContext>();
        services.AddSingleton<IConfiguration, ConfigurationProvider>();
        services.AddSingleton<ILoggerFactory, LoggerFactory>();
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        services.AddTransient<ISchedulesService, SchedulesService>();

        if (serviceDescriptors != null)
        {
          foreach (var serviceDescription in serviceDescriptors)
          {
            services.Add(serviceDescription);
          }
        }

        ServiceLocator.Instance = new DependencyInjectionContainer(services.BuildServiceProvider());

        _registered = true;
      }
    }

    [ExcludeFromCodeCoverage]
    private class DependencyInjectionContainer : IContainer
    {
      private readonly IServiceProvider _container;

      internal DependencyInjectionContainer(IServiceProvider container)
      {
        _container = container;
      }

      public T GetInstance<T>()
        where T : class
      {
        return _container.GetService<T>();
      }
    }
  }
}
