package com.unosquare.admin_core.back_end.dto;

import lombok.Data;

import javax.validation.constraints.NotBlank;
import java.util.List;

@Data
public class CreateHolidayDTO {

    @NotBlank
    private List<DateDTO> dates;

    @NotBlank
    private int employeeId;
}
