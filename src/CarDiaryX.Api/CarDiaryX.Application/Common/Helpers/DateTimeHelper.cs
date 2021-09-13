using System;
using System.Globalization;

namespace CarDiaryX.Application.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static bool AreDatesInTheSameMonth(DateTimeOffset date, DateTimeOffset date2)
            => date.Year == date2.Year && date.Month == date2.Month;

        public static bool Have30DaysPassed(DateTimeOffset date)
            => (DateTimeOffset.UtcNow - date).Days >= 30;

        public static bool AreDatesInTheSameWeek(DateTimeOffset date, DateTimeOffset date2)
            => GetWeekNumberOfCurrentYear(date) == GetWeekNumberOfCurrentYear(date2);

        // https://stackoverflow.com/a/37670914
        private static int GetWeekNumberOfCurrentYear(DateTimeOffset dt)
        {
            var startDays = 0;

            // first day of the year
            var firstJanuary = new DateTime(dt.Year, 1, 1);

            if (firstJanuary.DayOfWeek == DayOfWeek.Tuesday)
            {
                startDays = 1;
            }
            else if (firstJanuary.DayOfWeek == DayOfWeek.Wednesday)
            {
                startDays = 2;
            }
            else if (firstJanuary.DayOfWeek == DayOfWeek.Thursday)
            {
                startDays = 3;
            }
            else if (firstJanuary.DayOfWeek == DayOfWeek.Friday)
            {
                startDays = 4;
            }
            else if (firstJanuary.DayOfWeek == DayOfWeek.Saturday)
            {
                startDays = 5;
            }
            else if (firstJanuary.DayOfWeek == DayOfWeek.Sunday)
            {
                startDays = 6;
            }

            if (DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek == DayOfWeek.Sunday)
            {
                startDays++;
                startDays %= 7;
            }

            return ((dt.DayOfYear + startDays - 1) / 7) + 1;
        }
    }
}
