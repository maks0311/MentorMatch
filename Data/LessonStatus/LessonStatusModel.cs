using System;

namespace Mentor.Data
{
    [Serializable()]
    public class LessonStatusModel
    {
        public int LESSON_STATUS_ID { get; set; } = 0;
        public string LESSON_STATUS_NAME { get; set; } = string.Empty;
        public string LESSON_STATUS_ICON { get; set; } = string.Empty;
        public bool IS_ACTIVE { get; set; } = true;
    }
}