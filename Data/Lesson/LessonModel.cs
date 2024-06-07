using System;

namespace Mentor.Data
{
    [Serializable()]
    public class LessonModel
    {
        public int LESSON_ID { get; set; } = 0;
        public int SUBJECT_ID { get; set; } = 0;
        public string SUBJECT_NAME { get; set; } = string.Empty;
        public int LEVEL_ID { get; set; } = 0;
        public string LEVEL_NAME { get; set; } = string.Empty;
        public int LESSON_STATUS_ID { get; set; } = 0;
        public string LESSON_STATUS_NAME { get; set; } = string.Empty;
        public string LESSON_STATUS_ICON { get; set; } = string.Empty;
        public int TUTOR_ID { get; set; } = 0;
        public string TUTOR_NAME { get; set; } = string.Empty;
        public int STUDENT_ID { get; set; } = 0;
        public string STUDENT_NAME { get; set; } = string.Empty;
        public DateTime DATE_START { get; set; } = DateTime.MinValue;
        public DateTime DATE_STOP { get; set; } = DateTime.MinValue;
        public int RATING_ID { get; set; } = 6;
        public int RATING_VALUE { get; set; } = 0;
        public string RATING_NAME { get; set; } = "Not Rated Yet";
        public string TOPIC { get; set; } = string.Empty;

        public string CAPTION_FOR_STUDENT
        {
            get
            {
                return String.Format("{0} - {1}", TUTOR_NAME, SUBJECT_NAME);
            }
        }
        public string CAPTION_FOR_TUTOR
        {
            get
            {
                return String.Format("{0} - {1}", STUDENT_NAME, SUBJECT_NAME);
            }
        }
        public bool IsCreated
        {
            get
            {
                return LESSON_ID.IsPositive();
            }
        }
        public bool IsRated
        {
            get
            {
                return RATING_ID != 6;
            }
        }

        public bool HasEnded
        {
            get
            {
                return LESSON_STATUS_ID == 6;
            }
        }

        public LessonModel()
        {

        }

    }
}