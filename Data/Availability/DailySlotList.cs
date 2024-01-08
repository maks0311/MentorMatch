using System;
using System.Collections.Generic;

namespace Mentor.Data
{
    [Serializable]
    public class DailySlotList
    {
        public DateTime DateStart { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public DateTime DateStop { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day);
        public string DurationDays
        {
            get
            {
                TimeSpan ts = (DateStop - DateStart);
                return String.Format("{0}", ts.Days);
            }
        }

        public TimeSpan DurationWeeklyTimeSpan
        {
            get
            {
                TimeSpan ts = new TimeSpan();
                foreach (var item in Items)
                {
                    ts = ts.Add(item.DurationAsTimeSpan);
                }
                return ts;
            }
        }

        public List<DailySlot> Items { get; set; } = new List<DailySlot>();
        public DailySlotList() { }
        public DailySlotList(int TutorID)
        {
            for (int i = 1; i <= 7; i++)
            {
                DailySlot item = new DailySlot
                {
                    TutorID = TutorID,
                    Weekday = i,
                    TimeStart = new TimeOnly(0, 0),
                    TimeStop = new TimeOnly(0, 0)
                };

                this.Items.Add(item);
            }
        }
    }
}
