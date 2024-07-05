using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mentor
{
    public enum AvailabilityMode { SET_AVAILABLE, SET_NOT_AVAILABLE }

    public static class Extensions
    {

        public static bool IsNull<T>(this T obj) where T : class
        {
            try
            {
                return obj == null;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        public static bool IsNull<T>(this T? obj) where T : struct
        {
            try
            {
                return !obj.HasValue;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        public static bool IsNotNull<T>(this T obj) where T : class
        {
            try
            {
                return obj != null;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        public static bool IsNotNull<T>(this T? obj) where T : struct
        {
            try
            {
                return obj.HasValue;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        public static int ToInt(this DayOfWeek val)
        {
            return (int)val;
        }

        public static bool IsTrue(this bool val)
        {
            return (val == true);
        }

        public static bool IsFalse(this bool val)
        {
            return (val == false);
        }

        public static bool IsPositive(this int val)
        {
            return (val > 0);
        }

        public static bool IsPositiveOrZero(this int val)
        {
            return (val >= 0);
        }

        public static bool IsNegative(this int val)
        {
            return (val < 0);
        }
        public static bool IsNegativeOrZero(this int val)
        {
            return (val <= 0);
        }

        public static bool IsZero(this int val)
        {
            return (val == 0);
        }

        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }
    }
}
