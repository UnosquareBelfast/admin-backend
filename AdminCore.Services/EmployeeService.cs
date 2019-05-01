using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.Services.Base;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using AdminCore.FsmWorkflow;

namespace AdminCore.Services
{
  public class EmployeeService : BaseService, IEmployeeService
  {
    private readonly IMapper _mapper;

    public EmployeeService(IDatabaseContext databaseContext, IMapper mapper) :
      base(databaseContext)
    {
      _mapper = mapper;
    }

    public string Create(EmployeeDto newEmployeeDto)
    {
      var employee = _mapper.Map<Employee>(newEmployeeDto);
      employee.TotalHolidays = CalculateTotalHolidaysFromStartDate(employee, newEmployeeDto.StartDate);

      DatabaseContext.EmployeeRepository.Insert(employee);
      AddPublicHolidays(employee);
      DatabaseContext.SaveChanges();

      return employee.Email;
    }

    private void AddPublicHolidays(Employee employee)
    {
      var publicHolidays = DatabaseContext.MandatoryEventRepository.Get(x => x.CountryId == employee.CountryId);
      CreatePublicHolidays(employee, publicHolidays);
    }

    public bool VerifyEmailExists(string email)
    {
      return DatabaseContext.EmployeeRepository
        .Get(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)).Any();
    }

    public IList<EmployeeDto> GetAll()
    {
      var employees = DatabaseContext.EmployeeRepository.Get();
      return _mapper.Map<IList<EmployeeDto>>(employees);
    }

    public void Delete(int employeeId)
    {
      var employeeDbEntry = GetEmployeeById(employeeId);
      if (employeeDbEntry == null)
      {
        throw new Exception($"Employee could not be delete. Employee ID {employeeId} not found.");
      }
      DatabaseContext.EmployeeRepository.Delete(employeeDbEntry);
      DatabaseContext.SaveChanges();
    }

    public EmployeeDto Get(int employeeId)
    {
      var employeeDbEntry = GetEmployeeById(employeeId);
      return _mapper.Map<EmployeeDto>(employeeDbEntry);
    }

    public IList<EmployeeDto> GetByForenameAndSurname(string forename, string surname)
    {
      var employee = DatabaseContext.EmployeeRepository.Get(x =>
        x.Forename.Equals(forename, StringComparison.CurrentCultureIgnoreCase) &&
        x.Surname.Equals(surname, StringComparison.CurrentCultureIgnoreCase));
      return _mapper.Map<IList<EmployeeDto>>(employee);
    }

    public IList<EmployeeDto> GetByCountryId(int countryId)
    {
      var employee = DatabaseContext.EmployeeRepository.Get(x => x.CountryId == countryId);
      return _mapper.Map<IList<EmployeeDto>>(employee);
    }

    public void Save(EmployeeDto employeeDto)
    {
      if (employeeDto.EmployeeId == 0)
      {
        var newEmployeeEntry = _mapper.Map<Employee>(employeeDto);
        DatabaseContext.EmployeeRepository.Insert(newEmployeeEntry);
      }
      else
      {
        var employee = GetEmployeeById(employeeDto.EmployeeId);
        _mapper.Map(employeeDto, employee);
      }

      DatabaseContext.SaveChanges();
    }

    public EmployeeDto GetEmployeeByEmail(string email)
    {
      var result = DatabaseContext.EmployeeRepository.GetSingle(employee => employee.Email == email);
      return _mapper.Map<EmployeeDto>(result);
    }

    private Employee GetEmployeeById(int id)
    {
      var employee = DatabaseContext.EmployeeRepository.Get(x => x.EmployeeId == id);
      return employee.Any() ? employee.First() : null;
    }

    private void CreatePublicHolidays(Employee employee, IList<MandatoryEvent> publicHolidays)
    {
      var eventService = new EventService(DatabaseContext, _mapper, new DateService(), new EventWorkflowService(DatabaseContext, _mapper, new EmployeeService(DatabaseContext, _mapper), new FsmWorkflowHandler(DatabaseContext)));
      foreach (var holiday in publicHolidays)
      {
        eventService.CreateEvent(ConvertHolidayToEventDate(holiday), EventTypes.PublicHoliday,
          employee);
      }
    }

    private static EventDateDto ConvertHolidayToEventDate(MandatoryEvent holiday)
    {
      var eventDateDto = new EventDateDto
      {
        StartDate = holiday.MandatoryEventDate,
        EndDate = holiday.MandatoryEventDate,
        IsHalfDay = false
      };
      return eventDateDto;
    }

    private short CalculateTotalHolidaysFromStartDate(Employee employee, DateTime startDate)
    {
      var holidays = IsNorthernIrishEmployee(employee) ? GetNorthernIrishHolidays(startDate) : GetMexicanHolidays(startDate);

      return holidays;
    }

    private short GetMexicanHolidays(DateTime startDate)
    {
      if (IsInFirstThreeMonths(startDate, DateTime.Now))
      {
        return 0;
      }
      return (short)GetHolidaysByYearsWithCompany(startDate); //TODO Add Mexican Public Holidays From DB
    }

    private static int GetYearsWithCompany(DateTime startDate)
    {
      return DateTime.Today.Year - startDate.Year;
    }

    private static bool IsInFirstThreeMonths(DateTime startDate, DateTime currentDate)
    {
      var isWithinFirstThreeMonths = currentDate.AddMonths(-3) < startDate;
      return startDate.Year == currentDate.Year && isWithinFirstThreeMonths;
    }

    private int GetHolidaysByYearsWithCompany(DateTime startDate)
    {
      return DatabaseContext.EntitledHolidayRepository.GetSingle(x => x.YearsWithCompany == GetYearsWithCompany(startDate)).EntitledHolidays;
    }

    private short GetNorthernIrishHolidays(DateTime startDate)
    {
      short holidays = 0;
      if (startDate.Year == DateTime.Now.Year)
      {
        var northernIrishHolidays = DatabaseContext.EntitledHolidayRepository
          .GetSingle(x => x.Month == startDate.Month, null).EntitledHolidays;
        return (short)(northernIrishHolidays + 3); //TODO Add public holidays from DB
      }
      holidays += 3; //TODO Add public holidays from DB
      return holidays;
    }

    private static bool IsNorthernIrishEmployee(Employee employee)
    {
      return employee.CountryId == 1;
    }
  }
}