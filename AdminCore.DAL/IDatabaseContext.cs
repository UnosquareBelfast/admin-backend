using AdminCore.DAL.Models;

namespace AdminCore.DAL
{
  public interface IDatabaseContext
  {
    IRepository<Client> ClientRepository { get; }

    IRepository<Contract> ContractRepository { get; }

    IRepository<Country> CountryRepository { get; }

    IRepository<Employee> EmployeeRepository { get; }

    IRepository<SystemUserRole> SystemUserRoleRepository { get; }

    IRepository<EmployeeStatus> EmployeeStatusRepository { get; }

    IRepository<Event> EventRepository { get; }

    IRepository<EventDate> EventDatesRepository { get; }

    IRepository<EventMessage> EventMessageRepository { get; }

    IRepository<EventMessageType> EventMessageTypeRepository { get; }

    IRepository<EventStatus> EventStatusRepository { get; }

    IRepository<EventType> EventTypeRepository { get; }

    IRepository<EventTypeDaysNotice> EventTypeDaysNoticeRepository { get; }

    IRepository<EntitledHoliday> EntitledHolidayRepository { get; }

    IRepository<MandatoryEvent> MandatoryEventRepository { get; }

    IRepository<Schedule> SchedulesRepository { get; }

    IRepository<Team> TeamRepository { get; }

    IRepository<EventWorkflow> EventWorkflowRepository { get; }
    IRepository<EventTypeRequiredResponders> EventTypeRequiredRespondersRepository { get; }
    IRepository<SystemUserApprovalResponse> SystemUserApprovalResponsesRepository { get; }

    IRepository<Project> ProjectRepository { get; }
    IRepository<SystemUser> SystemUserRepository { get; }

    void SaveChanges();
  }
}
