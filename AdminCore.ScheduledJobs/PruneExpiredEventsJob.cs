using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminCore.ScheduledJobs
{
  internal class PruneExpiredEventsJob : BaseDatabaseJob, IJob
  {
    public Task Execute(IJobExecutionContext context)
    {
      var schedules = DatabaseContext.SchedulesRepository.Get(x => x.IsActive);
      if (schedules != null && schedules.Count > 0)
      {
        return PruneExpiredEvents();
      }

      return null;
    }

    private Task PruneExpiredEvents()
    {
      var expiredUnapprovedEvents = GetExpiredUnapprovedEvents();
      CancelExpiredUnapprovedEvents(expiredUnapprovedEvents);

      return Task.CompletedTask;
    }

    private void CancelExpiredUnapprovedEvents(List<EventDate> expiredUnapprovedEvents)
    {
      if (expiredUnapprovedEvents.Any())
      {
        SetExpiredUnapprovedEventsStatusToCancelled(expiredUnapprovedEvents);
        Console.WriteLine($"Successfully pruned {expiredUnapprovedEvents.Count} " +
                          $"expired events awaiting approval, status set to cancelled.");
      }
      else
      {
        Console.WriteLine("No expired events with awaiting approval status.");
      }
    }

    private void SetExpiredUnapprovedEventsStatusToCancelled(List<EventDate> expiredUnapprovedEvents)
    {
      foreach (var expiredUnapprovedEvent in expiredUnapprovedEvents)
      {
        DatabaseContext.EventRepository.Update(CancelUnapprovedEvent(expiredUnapprovedEvent));
      }

      DatabaseContext.SaveChanges();
    }

    private Event CancelUnapprovedEvent(EventDate expiredUnapprovedEvent)
    {
      var cancelledEventStatus = (int)EventStatuses.Cancelled;
      var unapprovedEvent =
        DatabaseContext.EventRepository.GetSingle(x => x.EventId == expiredUnapprovedEvent.EventId);
      unapprovedEvent.EventStatusId = cancelledEventStatus;
      return unapprovedEvent;
    }

    private List<EventDate> GetExpiredUnapprovedEvents()
    {
      var awaitingApproval = (int)EventStatuses.AwaitingApproval;
      return DatabaseContext.EventDatesRepository.GetAsQueryable(
          x => x.EndDate < DateTime.Today)
        .Where(x => x.Event.EventStatusId == awaitingApproval).ToList();
    }
  }
}