using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Mentor.Data
{
    [Serializable()]
    public class LessonModel
    {
        public int LESSON_ID { get; set; } = 0;
        public int SUBJECT_ID { get; set; } = 0;
        public string SUBJECT_NAME { get; set; } = string.Empty;
        public int LESSON_STATUS_ID { get; set; } = 0;
        public string LESSON_STATUS_NAME { get; set;} = string.Empty;
        public int TUTOR_ID { get; set; } = 0;
        public string TUTOR_NAME { get; set; } = string.Empty;
        public int STUDENT_ID { get; set; } = 0;
        public string STUDENT_NAME { get; set; } = string.Empty;
        public DateTime DATE_START { get; set; } = DateTime.MinValue;
        public DateTime DATE_STOP { get; set; } = DateTime.MinValue;
        public int RATING_ID { get; set; } = 0;
        public int RATING_VALUE { get; set; } = 0;
        public string RATING_NAME { get; set; } = string.Empty;
    }
}
