using System;

namespace Mentor.Data
{
    [Serializable]
    public class RatingModel
    {
        public int RATING_ID { get; set; } = 0;
        public int RATING_VALUE { get; set; } = -1;
        public string RATING_NAME { get; set; } = string.Empty;
        public bool IS_ACTIVE { get; set; } = true;

        public bool IsCreated
        {
            get
            {
                return RATING_ID.IsPositive();
            }
        }
        public RatingModel()
        {

        }
    }
}
