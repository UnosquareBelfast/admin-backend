import { getDurationBetweenDates } from './dates';
import mandatoryEvents from './mandatoryEvents';
import flow from 'lodash/fp/flow';
import moment from 'moment';

// The pipeline that our events go through to make them calendar ready.
export const transformEvents = allEvents => {
  return flow(
    _formatEventsForCalendar,
    _appendMandatoryEvents,
  )(allEvents);
};

// Private. Takes the events from the server and transforms them into a format
// that our calendar supports.
const _formatEventsForCalendar = events => {
  return events.map(event => {
    return {
      eventId: event.eventId,
      title: `${event.employee.forename} ${event.employee.surname}`,
      allDay: !event.halfDay,
      start: new moment([event.startDate], 'YYYY-MM-DD'),
      end: new moment([event.endDate], 'YYYY-MM-DD'),
      halfDay: event.halfDay,
      employee: event.employee,
      eventStatus: event.eventStatus,
      eventType: event.eventType,
    };
  });
};

// Private. Appends the mandatory holidays as specified in ./mandatoryEvents.js
const _appendMandatoryEvents = data => {
  return data.concat(mandatoryEvents);
};

// Takes an event and turns the duration of the event
export const getEventDuration = event => {
  const { start, end, isHalfday, eventType } = event;
  let duration = getDurationBetweenDates(start, end);
  const eventTypeId = eventType ? parseInt(eventType.eventTypeId) : 1;
  if (isHalfday) {
    if (duration != 0) {
      return 0.5;
    }
  } else if (eventTypeId !== 1) {
    return 0;
  } else {
    return duration;
  }
};

/* 
  Events Validation
*/

export const checkIfPastDatesSelected = start => {
  const today = new moment();
  return moment(start).isBefore(today);
};

export const checkIfDatesFallOnWeekend = (start, end) => {
  if (moment(start).isoWeekday() > 5 && moment(end).isoWeekday() > 5) {
    return true;
  } else {
    return false;
  }
};

export const checkIfSelectedDatesOverlapExisting = (
  events,
  employeeId,
  start,
  end,
) => {
  const overlappingEvents = events.filter(event => {
    const { employee } = event;
    if (employee && employee.employeeId === employeeId) {
      var selectedDateRange = moment.range(
        moment(start),
        moment(end).endOf('day'),
      );
      var existingEvent = moment.range(moment(event.start), moment(event.end));
      if (selectedDateRange.overlaps(existingEvent)) {
        return true;
      }
    }
  });
  return overlappingEvents.length > 0;
};

export const validateSelectedDates = (events, employeeId, start, end) => {
  const pastDatesSelected = checkIfPastDatesSelected(start);
  const datesFallOnWeekend = checkIfDatesFallOnWeekend(start, end);
  if (pastDatesSelected) {
    return 'Unable to select past dates';
  } else if (datesFallOnWeekend) {
    return 'Unable to select weekend dates';
  } else {
    const datesOverlapExisting = checkIfSelectedDatesOverlapExisting(
      events,
      employeeId,
      start,
      end,
    );
    if (datesOverlapExisting) {
      return 'You cannot request dates that have already been set';
    } else {
      return 'Dates approved';
    }
  }
};
