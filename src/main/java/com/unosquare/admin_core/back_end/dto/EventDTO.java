package com.unosquare.admin_core.back_end.dto;

import lombok.Data;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Data
public class EventDTO {

    private int eventId;
    private LocalDate startDate;
    private LocalDate endDate;
    private int eventTypeId;
    private String eventTypeDescription;
    private int eventStatusId;
    private String eventStatusDescription;
    private EmployeeDTO employee;
    private boolean isHalfDay;
    private LocalDateTime lastModified;
    private LocalDateTime dateCreated;
    private TeamDTO Team;
    private EventMessageDTO latestMessage;
}