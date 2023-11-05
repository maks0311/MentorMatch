using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Mentor.Data
{
    [Serializable()]
    public class LessonModel
    {
        public int LESSON_ID { get; set; } = 0;
        public int SUBJECT_ID { get; set; } = 0;
        public int LESSON_STATUS_ID { get; set; } = 0;
        public int TUTOR_ID { get; set; } = 0;
        public int STUDENT_ID { get; set; } = 0;
        public DateTime DATE_START { get; set; } = DateTime.MinValue;
        public DateTime DATE_STOP { get; set; } = DateTime.MinValue;
        public int RATING_ID { get; set; } = 0;
    }
}
