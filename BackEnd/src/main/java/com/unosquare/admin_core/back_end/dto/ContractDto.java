package com.unosquare.admin_core.back_end.dto;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.unosquare.admin_core.back_end.entity.ContractPK;
import com.unosquare.admin_core.back_end.enums.ContractStatus;
import com.unosquare.admin_core.back_end.service.ClientService;
import com.unosquare.admin_core.back_end.service.EmployeeService;
import lombok.Data;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;

@Data
public class ContractDto {

    @JsonIgnore
    @Autowired
    ClientService clientService;

    @JsonIgnore
    @Autowired
    EmployeeService employeeService;

    private EmployeeDto employee;
    private ClientDto client;
    private int contractStatusId;
    private String contractStatusDescription;

    public ContractDto() {

    }

    public ContractDto(int clientId, int employeeId, int contractStatusId) {
        this.client = new ModelMapper().map(clientService.findById(clientId), ClientDto.class);
        this.employee = new ModelMapper().map(employeeService.findById(employeeId), EmployeeDto.class);
        this.contractStatusId = contractStatusId;
        this.contractStatusDescription = getContractStatus().getDescription();
    }

    @JsonIgnore
    public ContractPK getContractId() {
        return new ContractPK(employee.getEmployeeId(), client.getClientId());
    }

    @JsonIgnore
    public ContractStatus getContractStatus() {
        return ContractStatus.fromId(contractStatusId);
    }
}


