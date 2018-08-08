package com.unosquare.admin_core.back_end.controller;

import com.unosquare.admin_core.back_end.ViewModels.*;
import com.unosquare.admin_core.back_end.dto.EventDTO;
import com.unosquare.admin_core.back_end.dto.UpdateEventDTO;
import com.unosquare.admin_core.back_end.enums.EventStatuses;
import com.unosquare.admin_core.back_end.enums.EventTypes;
import com.unosquare.admin_core.back_end.service.EventService;
import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@RestController
@CrossOrigin(origins = "*", allowCredentials = "true", allowedHeaders = "*")
@RequestMapping("/holidays")
public class HolidayController {

    @Autowired
    EventService eventService;

    @Autowired
    ModelMapper modelMapper;

    @GetMapping(value = "/", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public List<HolidayViewModel> findAll() {
        List holidays = eventService.findByType(EventTypes.ANNUAL_LEAVE);
        return mapEventDtosToHolidays(holidays);
    }

    @GetMapping(value = "/{holidayId}", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public HolidayViewModel findholidayById(@PathVariable("holidayId") int eventId) {
        EventDTO holiday = eventService.findById(eventId);
        return modelMapper.map(holiday, HolidayViewModel.class);
    }

    @GetMapping(value = "findByEmployeeId/{employeeId}", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public List<HolidayViewModel> findHolidaysByEmployeeId(@PathVariable("employeeId") int employeeId) {
        List holidays = eventService.findByEmployee(employeeId);
        return mapEventDtosToHolidays(holidays);
    }

    @PostMapping(value = "/", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseStatus(HttpStatus.CREATED)
    public ResponseEntity createHoliday(@RequestBody CreateHolidayViewModel createHolidayViewModel) {

        List<String> responses = new ArrayList<>();

        for (DateViewModel date : createHolidayViewModel.getDates()) {
            EventDTO existentEvent = eventService.findByEmployeeIdStartDataEndDate(
                    createHolidayViewModel.getEmployeeId(), date.getStartDate(), date.getEndDate(), EventTypes.ANNUAL_LEAVE);

            if (existentEvent != null) {
                responses.add("Holiday already exists");
                continue;
            }

            if (date.getStartDate().isAfter(date.getEndDate())) {
                responses.add("Starting date cannot be after end date");
                continue;
            }
        }

        if (responses.isEmpty()) {

            ArrayList<EventDTO> newHolidays = new ArrayList<>();

            for (DateViewModel date : createHolidayViewModel.getDates()) {

                EventDTO newHoliday = modelMapper.map(date , EventDTO.class);
                modelMapper.map(createHolidayViewModel, newHoliday);
                newHolidays.add(newHoliday);
            }

            eventService.saveEvents(newHolidays.stream().toArray(EventDTO[]::new));
        }

        return ResponseEntity.ok(responses);
    }

    @PutMapping(value = "/", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseStatus(HttpStatus.OK)
    public void updateHoliday(@RequestBody UpdateHolidayViewModel updateHolidayViewModel) {
        UpdateEventDTO event = modelMapper.map(updateHolidayViewModel, UpdateEventDTO.class);
        eventService.updateEvent(event); }

    @PutMapping(value = "/approveHoliday", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseStatus(HttpStatus.OK)
    public void approveHoliday(@RequestBody ApproveHolidayViewModel approveHolidayViewModel){
        EventDTO event = modelMapper.map(approveHolidayViewModel, EventDTO.class);
        eventService.approveEvent(event.getEventId());
    }

    @PutMapping(value = "/cancelHoliday", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseStatus(HttpStatus.OK)
    public void cancelHoliday(@RequestBody CancelHolidayViewModel cancelHolidayViewModel){
        EventDTO event = modelMapper.map(cancelHolidayViewModel, EventDTO.class);
        eventService.cancelEvent(event.getEventId());
    }

    @GetMapping(value = "/findByDateBetween/{rangeStart}/{rangeEnd}", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public List<HolidayViewModel> findByDateBetween(@PathVariable("rangeStart") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate rangeStart,
                                            @PathVariable("rangeEnd") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate rangeEnd) {
        return mapEventDtosToHolidays(eventService.findByDateBetween(rangeStart, rangeEnd,EventTypes.ANNUAL_LEAVE));
    }

    @GetMapping(value = "/findByHolidayStatus/{holidayStatusId}", produces = MediaType.APPLICATION_JSON_VALUE)
    @ResponseBody
    public List<HolidayViewModel> findByHolidayStatus(@PathVariable("holidayStatusId") int holidayStatusId) {
        return mapEventDtosToHolidays(eventService.findByStatusAndType(EventStatuses.fromId(holidayStatusId), EventTypes.ANNUAL_LEAVE));
    }

    private List<HolidayViewModel> mapEventDtosToHolidays(List<EventDTO> events) {
            return events.stream().map(event -> modelMapper.map(event, HolidayViewModel.class)).collect(Collectors.toList());
    }
}