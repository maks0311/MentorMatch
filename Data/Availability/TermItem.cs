using System;

namespace Mentor.Data
{
    [Serializable]
    public class TermItem
    {
        public DateTime SelectedDay { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
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
        public TermItem()
        {

        }
    }
}
