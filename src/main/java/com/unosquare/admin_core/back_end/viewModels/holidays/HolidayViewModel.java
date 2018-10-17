package com.unosquare.admin_core.back_end.viewModels.holidays;

import com.unosquare.admin_core.back_end.viewModels.employee.EmployeeViewModel;
import com.unosquare.admin_core.back_end.entity.EventStatus;
import com.unosquare.admin_core.back_end.entity.EventType;
import lombok.Data;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Data
public class HolidayViewModel {

        private int eventId;

        private LocalDate startDate;

        private LocalDate endDate;

        private EmployeeViewModel employee;

        private EventStatus eventStatus;

        private EventType eventType;

        private boolean isHalfDay;

        private LocalDateTime lastModified;

        private LocalDateTime dateCreated;
}
