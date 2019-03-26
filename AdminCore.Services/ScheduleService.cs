using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.Services.Base;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace AdminCore.Services
{
  public class SchedulesService : BaseService, ISchedulesService
  {
    private readonly IMapper _mapper;
    private readonly IScheduleRunner _scheduleRunner;

    public SchedulesService(IDatabaseContext database, IMapper mapper, IScheduleRunner scheduleRunner)
                : base(database)
    {
      _mapper = mapper;
      _scheduleRunner = scheduleRunner;
    }

    public IList<ScheduleDto> GetAllSchedules(bool? active)
    {
      var schedules = DatabaseContext.SchedulesRepository.Get(x => x.IsActive).ToList();

      return _mapper.Map<IList<Schedule>, List<ScheduleDto>>(schedules);
    }

    public ScheduleDto GetSchedule(int scheduleId)
    {
      var schedule = DatabaseContext.SchedulesRepository.GetSingle(x => x.ScheduleJobId == scheduleId);

      return _mapper.Map<Schedule, ScheduleDto>(schedule);
    }

    public void QueueAllExistingSchedules()
    {
      foreach (var schedule in GetAllSchedules(true))
      {
        _scheduleRunner.StartSchedule(schedule);
      }
    }
  }
}