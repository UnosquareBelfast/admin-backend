using AdminCore.DAL.Database;
using AdminCore.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AdminCore.DAL.Entity_Framework
{
  public class EntityFrameworkContext : IDatabaseContext
  {
    private readonly AdminCoreContext _adminCoreContext;

    private IRepository<Client> _clientRepository;

    private IRepository<Contract> _contractRepository;

    private IRepository<Country> _countryRepository;

    private IRepository<Employee> _employeeRepository;

    private IRepository<SystemUserRole> _employeeRoleRepository;

    private IRepository<EmployeeStatus> _employeeStatusRepository;

    private IRepository<EventDate> _eventDatesRepository;

    private IRepository<EventMessage> _eventMessageRepository;

    private IRepository<EventMessageType> _eventMessageTypeRepository;

    private IRepository<Event> _eventRepository;

    private IRepository<EventStatus> _eventStatusRepository;

    private IRepository<EventType> _eventTypeRepository;

    private IRepository<EventTypeDaysNotice> _eventTypeDaysNoticeRepository;

    private IRepository<EntitledHoliday> _entitledHolidayRepository;

    private IRepository<MandatoryEvent> _mandatoryEventRepository;

    private IRepository<Schedule> _scheduleRepository;

    private IRepository<Team> _teamRepository;

    private IRepository<EventWorkflow> _eventWorkflowRepository;

    private IRepository<EventTypeRequiredResponders> _eventTypeRequiredRespondersRepository;

    private IRepository<SystemUserApprovalResponse> _employeeApprovalResponsesRepository;

    private IRepository<Project> _projectRepository;
    private IRepository<SystemUser> _systemUserRepository;

    public EntityFrameworkContext()
    {
    }

    public EntityFrameworkContext(AdminCoreContext adminCoreContext)
    {
      _adminCoreContext = adminCoreContext;
    }

    public IRepository<T> RetrieveRepository<T>() where T : class
    {
      return new EntityFrameworkRepository<T>(this);
    }

    public virtual IRepository<EventDate> EventDatesRepository =>
      _eventDatesRepository ?? (_eventDatesRepository = RetrieveRepository<EventDate>());

    public virtual IRepository<Client> ClientRepository =>
      _clientRepository ?? (_clientRepository = RetrieveRepository<Client>());

    public virtual IRepository<Contract> ContractRepository =>
      _contractRepository ?? (_contractRepository = RetrieveRepository<Contract>());

    public virtual IRepository<Country> CountryRepository =>
      _countryRepository ?? (_countryRepository = RetrieveRepository<Country>());

    public virtual IRepository<Employee> EmployeeRepository =>
      _employeeRepository ?? (_employeeRepository = RetrieveRepository<Employee>());

    public virtual IRepository<SystemUserRole> EmployeeRoleRepository =>
      _employeeRoleRepository ?? (_employeeRoleRepository = RetrieveRepository<SystemUserRole>());

    public virtual IRepository<EmployeeStatus> EmployeeStatusRepository =>
      _employeeStatusRepository ?? (_employeeStatusRepository = RetrieveRepository<EmployeeStatus>());

    public virtual IRepository<Event> EventRepository =>
      _eventRepository ?? (_eventRepository = RetrieveRepository<Event>());

    public virtual IRepository<EventMessage> EventMessageRepository =>
      _eventMessageRepository ?? (_eventMessageRepository = RetrieveRepository<EventMessage>());

    public virtual IRepository<EventMessageType> EventMessageTypeRepository =>
      _eventMessageTypeRepository ??
      (_eventMessageTypeRepository = RetrieveRepository<EventMessageType>());

    public virtual IRepository<EventStatus> EventStatusRepository =>
      _eventStatusRepository ?? (_eventStatusRepository = RetrieveRepository<EventStatus>());

    public virtual IRepository<EventType> EventTypeRepository =>
      _eventTypeRepository ?? (_eventTypeRepository = RetrieveRepository<EventType>());

    public virtual IRepository<EventTypeDaysNotice> EventTypeDaysNoticeRepository =>
      _eventTypeDaysNoticeRepository ?? (_eventTypeDaysNoticeRepository = RetrieveRepository<EventTypeDaysNotice>());

    public virtual IRepository<MandatoryEvent> MandatoryEventRepository =>
      _mandatoryEventRepository ?? (_mandatoryEventRepository = RetrieveRepository<MandatoryEvent>());

    public virtual IRepository<EntitledHoliday> EntitledHolidayRepository =>
      _entitledHolidayRepository ?? (_entitledHolidayRepository = RetrieveRepository<EntitledHoliday>());

    public IRepository<Schedule> SchedulesRepository =>
      _scheduleRepository ?? (_scheduleRepository = RetrieveRepository<Schedule>());

    public virtual IRepository<Team> TeamRepository =>
      _teamRepository ?? (_teamRepository = RetrieveRepository<Team>());

    public virtual IRepository<EventWorkflow> EventWorkflowRepository =>
      _eventWorkflowRepository ?? (_eventWorkflowRepository = RetrieveRepository<EventWorkflow>());

    public virtual IRepository<EventTypeRequiredResponders> EventTypeRequiredRespondersRepository =>
      _eventTypeRequiredRespondersRepository ?? (_eventTypeRequiredRespondersRepository = RetrieveRepository<EventTypeRequiredResponders>());

    public virtual IRepository<SystemUserApprovalResponse> EmployeeApprovalResponsesRepository =>
      _employeeApprovalResponsesRepository ?? (_employeeApprovalResponsesRepository = RetrieveRepository<SystemUserApprovalResponse>());

    public virtual IRepository<Project> ProjectRepository =>
      _projectRepository ?? (_projectRepository = RetrieveRepository<Project>());

    public virtual IRepository<SystemUser> SystemUserRepository =>
      _systemUserRepository ?? (_systemUserRepository = RetrieveRepository<SystemUser>());

    public virtual void SaveChanges()
    {
      _adminCoreContext.SaveChanges();
    }

    public virtual EntityEntry Entry<T>(T entity) where T : class
    {
      return _adminCoreContext.Entry(entity);
    }

    public virtual DbSet<T> Set<T>() where T : class
    {
      return _adminCoreContext.Set<T>();
    }
  }
}
