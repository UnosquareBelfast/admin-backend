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

    private IRepository<EmployeeRole> _employeeRoleRepository;

    private IRepository<EmployeeStatus> _employeeStatusRepository;

    private IRepository<EventDate> _eventDatesRepository;

    private IRepository<EventMessage> _eventMessageRepository;

    private IRepository<EventMessageType> _eventMessageTypeRepository;

    private IRepository<Event> _eventRepository;

    private IRepository<EventStatus> _eventStatusRepository;

    private IRepository<EventType> _eventTypeRepository;

    private IRepository<EntitledHoliday> _entitledHolidayRepository;

    private IRepository<MandatoryEvent> _mandatoryEventRepository;

    private IRepository<Team> _teamRepository;

    /*    public EntityFrameworkContext()
        {
        }*/

    public EntityFrameworkContext(AdminCoreContext adminCoreContext)
    {
      _adminCoreContext = adminCoreContext;
    }

    public virtual IRepository<EventDate> EventDatesRepository =>
      _eventDatesRepository ?? (_eventDatesRepository = new EntityFrameworkRepository<EventDate>(this));

    public virtual IRepository<Client> ClientRepository =>
      _clientRepository ?? (_clientRepository = new EntityFrameworkRepository<Client>(this));

    public virtual IRepository<Contract> ContractRepository =>
      _contractRepository ?? (_contractRepository = new EntityFrameworkRepository<Contract>(this));

    public virtual IRepository<Country> CountryRepository =>
      _countryRepository ?? (_countryRepository = new EntityFrameworkRepository<Country>(this));

    public virtual IRepository<Employee> EmployeeRepository =>
      _employeeRepository ?? (_employeeRepository = new EntityFrameworkRepository<Employee>(this));

    public virtual IRepository<EmployeeRole> EmployeeRoleRepository =>
      _employeeRoleRepository ?? (_employeeRoleRepository = new EntityFrameworkRepository<EmployeeRole>(this));

    public virtual IRepository<EmployeeStatus> EmployeeStatusRepository =>
      _employeeStatusRepository ?? (_employeeStatusRepository = new EntityFrameworkRepository<EmployeeStatus>(this));

    public virtual IRepository<Event> EventRepository =>
      _eventRepository ?? (_eventRepository = new EntityFrameworkRepository<Event>(this));

    public virtual IRepository<EventMessage> EventMessageRepository =>
      _eventMessageRepository ?? (_eventMessageRepository = new EntityFrameworkRepository<EventMessage>(this));

    public virtual IRepository<EventMessageType> EventMessageTypeRepository =>
      _eventMessageTypeRepository ??
      (_eventMessageTypeRepository = new EntityFrameworkRepository<EventMessageType>(this));

    public virtual IRepository<EventStatus> EventStatusRepository =>
      _eventStatusRepository ?? (_eventStatusRepository = new EntityFrameworkRepository<EventStatus>(this));

    public virtual IRepository<EventType> EventTypeRepository =>
      _eventTypeRepository ?? (_eventTypeRepository = new EntityFrameworkRepository<EventType>(this));

    public virtual IRepository<MandatoryEvent> MandatoryEventRepository =>
      _mandatoryEventRepository ?? (_mandatoryEventRepository = new EntityFrameworkRepository<MandatoryEvent>(this));

    public virtual IRepository<EntitledHoliday> EntitledHolidayRepository =>
      _entitledHolidayRepository ?? (_entitledHolidayRepository = new EntityFrameworkRepository<EntitledHoliday>(this));

    public virtual IRepository<Team> TeamRepository =>
      _teamRepository ?? (_teamRepository = new EntityFrameworkRepository<Team>(this));

    public void SaveChanges()
    {
      _adminCoreContext.SaveChanges();
    }

    public EntityEntry Entry<T>(T entity) where T : class
    {
      return _adminCoreContext.Entry(entity);
    }

    public virtual DbSet<T> Set<T>() where T : class
    {
      return _adminCoreContext.Set<T>();
    }
  }
}