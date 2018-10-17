package com.unosquare.admin_core.back_end.viewModels.workfromhome;

import com.unosquare.admin_core.back_end.dto.EmployeeDTO;
import lombok.Data;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Data
public class ViewWorkingFromHomeViewModel {

    private int eventId;
    private LocalDate startDate;
    private LocalDate endDate;
    private int eventStatusId;
    private String eventStatusDescription;
    private EmployeeDTO employee;
    private LocalDateTime lastModified;
    private LocalDateTime dateCreated;
}