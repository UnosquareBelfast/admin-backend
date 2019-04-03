using AdminCore.DTOs;

namespace AdminCore.Common.Interfaces
{
  public interface IScheduleRunner
  {
    void StartSchedule(ScheduleDto schedule);
  }
}