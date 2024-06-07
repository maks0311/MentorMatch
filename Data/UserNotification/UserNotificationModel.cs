using System;
using System.Reflection;

namespace Mentor.Data
{
    public class UserNotificationModel
    {
        public int ID { get; set; } = 0;
        public int NOTIFICATION_ID { get; set; } = 0;
        public string TEXT { get; set; } = String.Empty;
        public int TUTOR_ID { get; set; } = 0;
        public string TUTOR_NAME { get; set; } = String.Empty;
        public int STUDENT_ID { get; set; } = 0;
        public string STUDENT_NAME { get; set; } = String.Empty;
        public int LESSON_ID { get; set; } = 0;
        public string LESSON_SUBJECT { get; set; } = String.Empty;
        public DateTime SENT_DATE {  get; set; } = DateTime.MinValue;
        public bool TUTOR_READ { get; set; } = false;
        public bool STUDENT_READ { get; set; } = false;
        public DateTime DATE_START { get; set; } = DateTime.MinValue;
        public DateTime DATE_STOP { get; set; } = DateTime.MinValue;
        public int RATING { get; set; } = 0;

    }
}
