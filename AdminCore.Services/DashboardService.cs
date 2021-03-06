﻿using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Dashboard;
using AdminCore.DTOs.Event;
using AdminCore.Services.Base;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AdminCore.Constants;
using Microsoft.EntityFrameworkCore;

namespace AdminCore.Services
{
  public class DashboardService : BaseService, IDashboardService
  {
    private readonly IMapper _mapper;

    public DashboardService(IDatabaseContext databaseContext, IMapper mapper) : base(databaseContext)
    {
      _mapper = mapper;
    }

    public IList<EventDto> GetEmployeeDashboardEvents(int employeeId, DateTime date)
    {
      var eventList = DatabaseContext.EventRepository.Get
      (
        evnt => EmployeeDashboardEventsQuery(employeeId, date, evnt),
        null,
        evnt => evnt.EventDates,
        evnt => evnt.Employee,
        evnt => evnt.EventType,
        evnt => evnt.EventStatus
      );
      return _mapper.Map<IList<EventDto>>(eventList);
    }

    public IList<EventDto> GetEmployeeEventsForMonth(int employeeId, DateTime date)
    {
      var events = DatabaseContext.EventRepository.Get(evnt => evnt.EmployeeId == employeeId && DateService.EventContainsEventDatesThatHappenDuringMonth(evnt.EventDates, date),
        null,
        x => x.EventDates,
        x => x.Employee,
        x => x.EventType,
        x => x.EventStatus);
      return _mapper.Map<IList<EventDto>>(events);
    }

    public IDictionary<string, List<EmployeeSnapshotDto>> GetEmployeeSnapshotsByEmployeeId(int employeeId)
    {
      var contractList = DatabaseContext.ContractRepository.Get
      (
        contract => GetEmployeeSnapshotByEmployeeIdQuery(employeeId, contract, DateTime.Today),
        null,
        contract => contract.Team
      );
      return ConvertContractListToEmployeeSnapshotDtoMap(contractList);
    }

    public IDictionary<string, List<EmployeeSnapshotDto>> GetTeamSnapshotDashboardEvents()
    {
      var contractList = DatabaseContext.ContractRepository.Get
      (
        contract => GetTeamSnapshotDashboardEventsQuery(contract, DateTime.Today),
        null,
        contract => contract.Team
      );
      return ConvertContractListToEmployeeSnapshotDtoMap(contractList);
    }

    public IList<ClientSnapshotDto> GetTeamDashboardEvents(int employeeId, DateTime date)
    {
      var teamsForEmployee = GetTeamIdsForEmployee(employeeId, date);
      var clientList = DatabaseContext.ClientRepository
        .GetAsQueryable(QueryClientsWithContractsForEmployeeId(teamsForEmployee, date))
        .Include(client => client.Projects)
        .ThenInclude(project => project.Teams)
        .ThenInclude(team => team.Contracts)
        .ThenInclude(contract => contract.Employee)
        .ThenInclude(employee => employee.Events)
        .ThenInclude(evnt => evnt.EventDates);

      return clientList.Select(client => BuildClientSnapshot(client, date)).ToList();
    }

    public static bool EmployeeDashboardEventsQuery(int employeeId, DateTime date, Event evnt)
    {
      const int cancelled = (int)EventStatuses.Cancelled;
      var startOfMonth = DateService.GetMonthStartDate(date);
      var endOfMonth = DateService.GetMonthEndDate(date);

      return EmployeeDashboardEvents(evnt, employeeId, cancelled, startOfMonth, endOfMonth);
    }

    public static bool GetEmployeeSnapshotByEmployeeIdQuery(int employeeId, Contract contract, DateTime date)
    {
      return contract.EmployeeId == employeeId &&
             DateService.ContractIsActiveDuringDate(contract, date) &&
             contract.Employee.Events.Any(evnt => DateService.EventContainsEventDatesThatHappenDuringMonth(evnt.EventDates, date));
    }

    public IList<EventDto> GetEmployeeTeamEvents(int employeeId, DateTime date)
    {
      var cancelled = (int)EventStatuses.Cancelled;
      var employeeTeams = GetTeamIdsForEmployee(employeeId, date);
      var eventsForTeams = DatabaseContext.EventRepository.Get(@event =>
        @event.Employee.Contracts.Any(contract => employeeTeams.Contains(contract.TeamId)) &&
        @event.EventStatusId != cancelled &&
        DateService.EventContainsEventDatesThatHappenDuringMonth(@event.EventDates, date),
        null,
        @event => @event.Employee,
        @event => @event.EventDates,
        @event => @event.EventStatus,
        @event => @event.EventType,
        @event => @event.EventMessages
        );
      return _mapper.Map<IList<EventDto>>(eventsForTeams);
    }

    private List<int> GetTeamIdsForEmployee(int employeeId, DateTime date)
    {
      return DatabaseContext.TeamRepository.Get(team => team.Contracts.Any(contract => contract.EmployeeId == employeeId && DateService.ContractIsActiveDuringDate(contract, date)))
        .Select(team => team.TeamId).ToList();
    }

    private static bool EmployeeDashboardEvents(Event evnt, int employeeId, int cancelled, DateTime startOfMonth, DateTime endOfMonth)
    {
      return (evnt.EmployeeId == employeeId) &&
             (evnt.EventStatus.EventStatusId != cancelled) &&
             DateService.EventContainsEventDatesThatHappenDuringMonth(evnt.EventDates, startOfMonth) &&
             EmployeeHasActiveContractsDuringMonth(evnt.Employee.Contracts, startOfMonth, endOfMonth);
    }

    private static bool GetTeamSnapshotDashboardEventsQuery(Contract contract, DateTime date)
    {
      return DateService.ContractIsActiveDuringDate(contract, date) &&
             contract.Employee.Events.Any(evnt => DateService.EventContainsEventDatesThatHappenDuringMonth(evnt.EventDates, date));
    }

    private static bool EmployeeHasActiveContractsDuringMonth(IEnumerable<Contract> employeeContracts, DateTime startOfMonth, DateTime endOfMonth)
    {
      return employeeContracts.Any(contract => DateService.ContractIsActiveDuringRangeOfDates(contract, startOfMonth, endOfMonth));
    }

    private IEnumerable<Event> GetEventsForListOfEmployeesInMonth(IEnumerable<Employee> employeeList, DateTime date)
    {
      return DatabaseContext.EventRepository.Get(evnt => employeeList.Contains(evnt.Employee) && DateService.EventContainsEventDatesThatHappenDuringMonth(evnt.EventDates, date),
        null,
        evnt => evnt.EventDates,
        evnt => evnt.Employee,
        evnt => evnt.EventType,
        evnt => evnt.EventStatus);
    }

    private static IDictionary<string, List<EmployeeSnapshotDto>> ConvertContractListToEmployeeSnapshotDtoMap(IList<Contract> contractList)
    {
      IDictionary<string, List<EmployeeSnapshotDto>> snapshotMap = new Dictionary<string, List<EmployeeSnapshotDto>>();
      foreach (var contract in contractList)
      {
        var employeeSnapshotDto = CreateEmployeeSnapshotFromContract(contract);
        AddEmployeeSnapshotToSnapshotMap(snapshotMap, contract.Team.TeamName, employeeSnapshotDto);
      }
      return snapshotMap;
    }

    private static void AddEmployeeSnapshotToSnapshotMap(IDictionary<string, List<EmployeeSnapshotDto>> snapshotMap, string teamName, EmployeeSnapshotDto employeeSnapshotDto)
    {
      if (snapshotMap.ContainsKey(teamName))
      {
        snapshotMap[teamName].Add(employeeSnapshotDto);
      }
      else
      {
        snapshotMap.Add(teamName, new List<EmployeeSnapshotDto>() { employeeSnapshotDto });
      }
    }

    private static EmployeeSnapshotDto CreateEmployeeSnapshotFromContract(Contract contract)
    {
      return new EmployeeSnapshotDto
      {
        Email = contract.Employee.Email,
        EmployeeId = contract.EmployeeId,
        Forename = contract.Employee.Forename,
        Surname = contract.Employee.Surname
      };
    }

    private static Expression<Func<Client, bool>> QueryClientsWithContractsForEmployeeId(IList<int> teamIds, DateTime date)
    {
      return client =>
        client.Projects.SelectMany(project => project.Teams).Any(team => teamIds.Contains(team.TeamId) &&
          team.Contracts.Any(contract => DateService.ContractIsActiveDuringDate(contract, date)));
    }

    private ClientSnapshotDto BuildClientSnapshot(Client client, DateTime date)
    {
      var clientSnapShot = _mapper.Map<ClientSnapshotDto>(client);
      clientSnapShot.Projects = client.Projects.Select(clientProject => BuildProjectSnapshot(clientProject, date)).ToList();
      return clientSnapShot;
    }

    private ProjectSnapshotDto BuildProjectSnapshot(Project project, DateTime date)
    {
      var projectSnapshot = _mapper.Map<ProjectSnapshotDto>(project);
      projectSnapshot.Teams = project.Teams.Select(clientTeam => BuildTeamSnapshot(clientTeam, date)).ToList();
      return projectSnapshot;
    }

    private TeamSnapshotDto BuildTeamSnapshot(Team team, DateTime date)
    {
      var teamSnapshot = _mapper.Map<TeamSnapshotDto>(team);
      teamSnapshot.Employees = team.Contracts.Select(contract => BuildEmployeeSnapshot(contract.Employee, date)).ToList();
      return teamSnapshot;
    }

    private EmployeeSnapshotDto BuildEmployeeSnapshot(Employee employee, DateTime date)
    {
      var employeeSnapshot = _mapper.Map<EmployeeSnapshotDto>(employee);
      employeeSnapshot.Location = GetLocationFromEmployee(employee, date);
      return employeeSnapshot;
    }

    private string GetLocationFromEmployee(Employee employee, DateTime date)
    {
      var employeeEvents = employee.Events.Where(evnt =>
        DateService.EventContainsEventDatesThatHappenDuringAGivenDay(evnt.EventDates, date)
        && evnt.EventStatusId == (int)EventStatuses.Approved).ToList();
      return employeeEvents.Any() ? GetLocationFromEvent(employeeEvents.First()) : EmployeeLocationConstants.InOffice;
    }

    private static string GetLocationFromEvent(Event @event)
    {
      switch (@event.EventTypeId)
      {
        case 1:
          return EmployeeLocationConstants.OnHoliday;

        case 2:
          return EmployeeLocationConstants.WorkingFromHome;

        case 3:
          return EmployeeLocationConstants.SickLeave;

        case 4:
          return EmployeeLocationConstants.WorkRelatedTravel;

        default:
          throw new Exception($"There is no employee location mapping for event type id {@event.EventTypeId}");
      }
    }
  }
}
