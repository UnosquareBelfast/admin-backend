package com.unosquare.admin_core.back_end.viewModels.events;

import com.unosquare.admin_core.back_end.viewModels.DateViewModel;
import lombok.Data;

import javax.validation.constraints.NotBlank;
import java.util.List;

@Data
public class CreateEventViewModel {
    @NotBlank
    private List<DateViewModel> dates;

    @NotBlank
    private int employeeId;
}
