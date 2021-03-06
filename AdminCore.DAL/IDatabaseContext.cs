﻿using AdminCore.DAL.Models;

namespace AdminCore.DAL
{
  public interface IDatabaseContext
  {
    IRepository<Client> ClientRepository { get; }

    IRepository<Contract> ContractRepository { get; }

    IRepository<Country> CountryRepository { get; }

    IRepository<Employee> EmployeeRepository { get; }

    IRepository<EmployeeRole> EmployeeRoleRepository { get; }

    IRepository<EmployeeStatus> EmployeeStatusRepository { get; }

    IRepository<Event> EventRepository { get; }

    IRepository<EventDate> EventDatesRepository { get; }

    IRepository<EventMessage> EventMessageRepository { get; }

    IRepository<EventMessageType> EventMessageTypeRepository { get; }

    IRepository<EventStatus> EventStatusRepository { get; }

    IRepository<EventType> EventTypeRepository { get; }

    IRepository<EntitledHoliday> EntitledHolidayRepository { get; }

    IRepository<MandatoryEvent> MandatoryEventRepository { get; }

    IRepository<Schedule> SchedulesRepository { get; }

    IRepository<Team> TeamRepository { get; }

    IRepository<Project> ProjectRepository { get; }

    void SaveChanges();
  }
}
