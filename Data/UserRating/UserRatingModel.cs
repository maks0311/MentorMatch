using System;

namespace Mentor.Data
{
    public class UserRatingModel
    {
        public int ID { get; set; } = 0;
        public int TUTOR_ID { get; set; } = 0;
        public int STUDENT_ID { get; set; } = 0;
        public int RATING_ID { get; set; } = 0;
        public DateTime DATE_TIME { get; set; } = DateTime.MinValue;

        public UserRatingModel()
        {
            
        }
    }
}
