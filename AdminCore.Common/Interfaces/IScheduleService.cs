using AdminCore.DTOs;
using System.Collections.Generic;

namespace AdminCore.Common.Interfaces
{
  public interface ISchedulesService
  {
    IList<ScheduleDto> GetAllSchedules(bool? active = null);

    ScheduleDto GetSchedule(int scheduleId);

    void QueueAllExistingSchedules();
  }
}