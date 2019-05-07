using System;
using System.Collections;
using System.Collections.Generic;
using AdminCore.DAL.Models;

namespace AdminCore.Services.Tests.ClassData
{
    public class EventServiceClassData
    {       
        public class CreateEvent_ValidNewEventOfOneDay_SuccessfullyInsertsNewEventIntoDb_ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // Last date to book by is 7 days in advance at midnight.
                yield return new object[] { 1, 1, new DateTime(2019, 06, 19), new DateTime(2019, 06, 19),
                    TestClassBuilder.AnnualLeaveEventType(),
                    new List<EventTypeDaysNotice>{
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 1,
                            DaysNotice = 7,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                    }},
                    new DateTime(2019, 06, 12)};
                // Last date to book by is 7 days in advance at midnight (TimeNotice in db is null).
                yield return new object[] { 1, 1, new DateTime(2019, 06, 19), new DateTime(2019, 06, 19),
                    TestClassBuilder.AnnualLeaveEventType(),
                    new List<EventTypeDaysNotice>{
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 1,
                            DaysNotice = 7,
                            TimeNotice = null,
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                    }},
                    new DateTime(2019, 06, 12)};
                // Last date to book by is 1 days in advance at 00:00.
                // Current time is 00:00
                yield return new object[] { 1, 1, new DateTime(2019, 06, 19), new DateTime(2019, 06, 19),
                    TestClassBuilder.AnnualLeaveEventType(),
                    new List<EventTypeDaysNotice>{
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 1,
                            DaysNotice = 1,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                    }},
                    new DateTime(2019, 06, 12)};
                // Last date to book by is 1 days in advance at 16:00.
                // Current time is 15:59:59
                yield return new object[] { 1, 1, new DateTime(2019, 06, 19), new DateTime(2019, 06, 19),
                    TestClassBuilder.AnnualLeaveEventType(),
                    new List<EventTypeDaysNotice>{
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 1,
                            DaysNotice = 1,
                            TimeNotice = new TimeSpan(16, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                    }},
                    new DateTime(2019, 06, 18, 15, 59, 59)};
                // No notice period definition in db.
                yield return new object[] { 1, 1, new DateTime(2019, 06, 19), new DateTime(2019, 06, 19),
                    TestClassBuilder.WorkingFromHomeEventType(),
                    new List<EventTypeDaysNotice>{
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 1,
                            DaysNotice = 7,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                    }},
                    new DateTime(2019, 06, 12)};
                // First EventTypeDaysNotice is used => 7 days notice.
                yield return new object[] { 1, 1, new DateTime(2019, 06, 17), new DateTime(2019, 06, 19),
                    TestClassBuilder.AnnualLeaveEventType(),
                    new List<EventTypeDaysNotice> {
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 1,
                            DaysNotice = 7,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                        },
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 4,
                            DaysNotice = 14,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                    }},
                    new DateTime(2019, 06, 10)};
                // Second EventTypeDaysNotice is used => 14 days notice.
                yield return new object[] { 1, 1, new DateTime(2019, 06, 17), new DateTime(2019, 06, 20),
                    TestClassBuilder.AnnualLeaveEventType(),
                    new List<EventTypeDaysNotice> {
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 1,
                            DaysNotice = 7,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                        },
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 4,
                            DaysNotice = 14,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                        }},
                    new DateTime(2019, 06, 3)};
                // Second EventTypeDaysNotice is used => 14 days notice.
                // Weekend spanning event, 4 days June 2019 20th => 25th (Thursday => Tuesday)
                yield return new object[] { 1, 1, new DateTime(2019, 06, 20), new DateTime(2019, 06, 25),
                    TestClassBuilder.AnnualLeaveEventType(),
                    new List<EventTypeDaysNotice> {
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 1,
                            DaysNotice = 7,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                        },
                        new EventTypeDaysNotice
                        {
                            LeaveLengthDays = 4,
                            DaysNotice = 14,
                            TimeNotice = new TimeSpan(0, 0, 0),
                            EventType = TestClassBuilder.AnnualLeaveEventType(),
                            EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId
                        }},
                    new DateTime(2019, 06, 6)};
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

    }
}