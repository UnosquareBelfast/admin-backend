package com.unosquare.admin_core.back_end.repository;

import com.unosquare.admin_core.back_end.entity.Employee;
import com.unosquare.admin_core.back_end.entity.Event;
import com.unosquare.admin_core.back_end.entity.EventStatus;
import com.unosquare.admin_core.back_end.entity.EventType;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.time.LocalDate;
import java.util.List;

public interface EventRepository extends JpaRepository<Event, Integer> {

    List<Event> findByEmployee(Employee employee);

    @Query(value = "SELECT e FROM Event e " +
            "WHERE " +
            "(e.eventStatus = '1') AND (e.eventType = '1') " +
            "AND " +
            "e.employee.employeeId = :employeeId"
    )
    List<Event> findALByEmployee(@Param("employeeId") int employeeId);

    @Query(value = "SELECT e FROM Event e " +
            "WHERE " +
            "(e.eventStatus = '1') AND (e.eventType = '2') " +
            "AND " +
            "e.employee.employeeId = :employeeId"
    )
    List<Event> findWFHByEmployee(@Param("employeeId") int employeeId);

    Event findByEmployeeAndStartDateAndEndDateAndEventType(Employee employee, LocalDate startDate, LocalDate endDate, EventType eventType);

    List<Event> findByStartDateBetweenAndEventType(LocalDate rangeStart, LocalDate rangeEnd,EventType eventType);

    List<Event> findByEventStatusAndEventType(EventStatus eventStatus, EventType eventType);

    List<Event> findByEventType(EventType eventType);
}