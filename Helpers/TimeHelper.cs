using System.Collections.Generic;
using System;
using Mentor.Data;

namespace Mentor
{
    public static class TimeHelper
    {
        public static IEnumerable<TimeOnly> GetDailyQuartersAsTimeOnly()
        {
            TimeOnly time = new TimeOnly(8, 0, 0);
            List<TimeOnly> tEnum = new List<TimeOnly>();
            for (int i = 0; i < 64; i++)
            {
                tEnum.Add(time);
                time = time.AddMinutes(15);
            }

            return tEnum;
        }
        
        public static List<DateTime> GetDailyQuartersAsDateTime(DateTime day)
        {
            TimeOnly time = new TimeOnly(8, 0, 0);
            List<DateTime> dtEnum = new List<DateTime>();
            
            for (int i = 0; i < 64; i++)
            {
                DateTime dt = new DateTime(day.Year, day.Month, day.Day, time.Hour, time.Minute, 0);
                dtEnum.Add(dt);
                time = time.AddMinutes(15);
            }

            return dtEnum;
        }

        public static List<AvailabilityModel> GetDailyQuartersAsAvailabilityList(int tutoID, DateTime day)
        {
            TimeOnly time = new TimeOnly(8, 0, 0);
            List<AvailabilityModel> avEnum = new List<AvailabilityModel>();

            for (int i = 0; i < 64; i++)
            {
                DateTime dt = new DateTime(day.Year, day.Month, day.Day, time.Hour, time.Minute, 0);

                avEnum.Add(new AvailabilityModel
                {
                    AVAILABILITY_ID = 0,
                    TUTOR_ID = tutoID,
                    DATE_START = dt,
                    DATE_STOP = dt.AddMinutes(15)
                });
                
                time = time.AddMinutes(15);
            }

            return avEnum;
        }

        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var month = from.Date; month.Date <= thru.Date || month.Month == thru.Month; month = month.AddMonths(1))
                yield return month;
        }

        public static IEnumerable<DateTime> EachDayTo(this DateTime dateFrom, DateTime dateTo)
        {
            return EachDay(dateFrom, dateTo);
        }

        public static IEnumerable<DateTime> EachMonthTo(this DateTime dateFrom, DateTime dateTo)
        {
            return EachMonth(dateFrom, dateTo);
        }
    }
}
