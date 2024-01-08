using System;
using System.Diagnostics.Metrics;

namespace Mentor.Data
{
    [Serializable]
    public class DailySlot
    {
        public int TutorID { get; set; }
        public int Weekday { get; set; } = 1;
        public TimeOnly TimeStart { get; set; } = new TimeOnly(8, 0);
        public TimeOnly TimeStop { get; set; } = new TimeOnly(9, 0);

        public string Duration
        {
            get
            {
                TimeSpan ts = (TimeStop - TimeStart);
                return String.Format("{0}:{1}", ts.Hours.ToString("00"), ts.Minutes.ToString("00"));
            }
        }
        public TimeSpan DurationAsTimeSpan
        {
            get
            {
                TimeSpan ts = (TimeStop - TimeStart);
                return ts;
            }
        }

        public string WeekdayName
        {
            get
            {
                string weekday_name = "Monday";

                switch (Weekday)
                {
                    case 1:
                        weekday_name = "Monday";
                        break;
                    case 2:
                        weekday_name = "Tuesday";
                        break;
                    case 3:
                        weekday_name = "Wednesday";
                        break;
                    case 4:
                        weekday_name = "Thursday";
                        break;
                    case 5:
                        weekday_name = "Friday";
                        break;
                    case 6:
                        weekday_name = "Saturday";
                        break;
                    case 7:
                        weekday_name = "Sunday";
                        break;
                }

                return weekday_name;
            }
        }
        public DailySlot()
        {

        }
    }
}
