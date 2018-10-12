package com.unosquare.admin_core.back_end.helper;

import com.unosquare.admin_core.back_end.dto.EmployeeDTO;
import com.unosquare.admin_core.back_end.dto.EventDTO;
import com.unosquare.admin_core.back_end.dto.EventMessageDTO;
import com.unosquare.admin_core.back_end.entity.*;

import java.time.LocalDate;
import java.time.LocalDateTime;

public class UnitTestHelper {

    private static final LocalDate date = LocalDate.now();
    private static final LocalDateTime dateTime = LocalDateTime.now();

    public Event createEvent() {
        Event event = new Event();
        EventStatus eventStatus = new EventStatus();
        EventType eventType = new EventType();

        Employee employee = createEmployee();

        event.setEventId(1);
        event.setStartDate(date);
        event.setEndDate(date);
        event.setEmployee(employee);
        event.setEventStatus(eventStatus);
        event.setEventType(eventType);
        event.setHalfDay(false);
        event.setLastModified(dateTime);
        event.setDateCreated(dateTime);

        return event;
    }

    public EventDTO createEventDTO() {
        EventDTO eventDTO = new EventDTO();

        EmployeeDTO employee = createEmployeeDTO();
        EventMessageDTO eventMessageDTO = createEventMessageDTO();

        eventDTO.setEventId(1);
        eventDTO.setStartDate(date);
        eventDTO.setEndDate(date);
        eventDTO.setEventTypeId(1);
        eventDTO.setEventTypeDescription("test");
        eventDTO.setEventStatusId(1);
        eventDTO.setEventStatusDescription("test description");
        eventDTO.setEmployee(employee);
        eventDTO.setHalfDay(false);
        eventDTO.setLastModified(dateTime);
        eventDTO.setDateCreated(dateTime);
        eventDTO.setLatestMessage(eventMessageDTO);

        return eventDTO;
    }

    public EventType createEventType() {
        EventType eventType = new EventType();

        eventType.setEventTypeId(1);
        eventType.setDescription("event description");

        return eventType;
    }

    public EventMessage createEventMessage() {
        EventMessage eventMessage = new EventMessage();

        Event event = createEvent();

        Employee employee = createEmployee();

        eventMessage.setEmployee(employee);
        eventMessage.setEvent(event);
        eventMessage.setEventMessageId(1);
        eventMessage.setLastModified(dateTime);
        eventMessage.setMessage("test message");

        return eventMessage;
    }

    public EventMessageDTO createEventMessageDTO() {
        EventMessageDTO eventMessageDTO = new EventMessageDTO();

        eventMessageDTO.setAuthor("test");
        eventMessageDTO.setEventMessageId(1);
        eventMessageDTO.setLastModified(date);
        eventMessageDTO.setMessage("test message");
        eventMessageDTO.setMessageTypeDescription("message desc.");
        eventMessageDTO.setMessageTypeId(1);

        return eventMessageDTO;
    }

    public Employee createEmployee() {
        Employee employee = new Employee();

        EmployeeStatus employeeStatus = new EmployeeStatus();
        employeeStatus.setEmployeeStatusId(1);
        employeeStatus.setDescription("test");

        EmployeeRole employeeRole = new EmployeeRole();
        employeeRole.setEmployeeRoleId(1);
        employeeRole.setDescription("test");

        Country country = new Country();
        country.setCountryId(1);
        country.setDescription("test");

        employee.setEmployeeRole(employeeRole);
        employee.setEmployeeId(1);
        employee.setForename("name");
        employee.setSurname("surname");
        employee.setPassword("password");
        employee.setEmail("email@email.com");
        employee.setEmployeeStatus(employeeStatus);
        employee.setCountry(country);
        employee.setStartDate(date);

        return employee;
    }

    public EmployeeDTO createEmployeeDTO() {
        EmployeeDTO employeeDTO = new EmployeeDTO();

        employeeDTO.setEmployeeRoleId(1);
        employeeDTO.setEmployeeId(1);
        employeeDTO.setForename("test");
        employeeDTO.setSurname("testy");
        employeeDTO.setPassword("password");
        employeeDTO.setEmail("test@email.com");
        employeeDTO.setEmployeeStatusId(1);
        employeeDTO.setStatusDescription("test");
        employeeDTO.setCountryDescription("test");
        employeeDTO.setCountryId(1);
        employeeDTO.setStartDate(date);

        return employeeDTO;
    }
}