using System;

namespace AdminCore.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static int weekLengthDays = 7;
        private static int intDaySunday = 7;
        private static int intDaySaturday = 6;

        // https://stackoverflow.com/questions/1617049/calculate-the-number-of-business-days-between-two-dates
        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDaysAcc = span.Days + 1;
            int fullWeekCount = businessDaysAcc / weekLengthDays;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDaysAcc > fullWeekCount * weekLengthDays)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int) firstDay.DayOfWeek;
                int lastDayOfWeek = (int) lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += weekLengthDays;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= weekLengthDays)// Both Saturday and Sunday are in the remaining time interval
                        businessDaysAcc -= 2;
                    else if (lastDayOfWeek >= intDaySaturday)// Only Saturday is in the remaining time interval
                        businessDaysAcc -= 1;
                }
                else if (firstDayOfWeek <= intDaySunday && lastDayOfWeek >= intDaySunday)// Only Sunday is in the remaining time interval
                    businessDaysAcc -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDaysAcc -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bankHolidayDate = bankHoliday.Date;
                if (firstDay <= bankHolidayDate && bankHolidayDate <= lastDay)
                    --businessDaysAcc;
            }

            return businessDaysAcc;
        }
    }
}
