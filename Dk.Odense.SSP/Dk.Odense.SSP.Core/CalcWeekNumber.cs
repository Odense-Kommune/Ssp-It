using System;
using System.Globalization;

namespace Dk.Odense.SSP.Core
{
    public class CalcWeekNumber
    {
        public int GetWeekNumber(DateTime date)
        {
            return CultureInfo.CreateSpecificCulture("da-DK").Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
