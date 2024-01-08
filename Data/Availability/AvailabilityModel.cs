using System;

namespace Mentor.Data
{
    [Serializable]
    public class AvailabilityModel
    {
        public int AVAILABILITY_ID { get; set; } = 0;
        public int TUTOR_ID { get; set; } = 0;
        public DateTime DATE_START { get; set; } = DateTime.MinValue;
        public DateTime DATE_STOP { get; set;} = DateTime.MinValue;
        public bool IS_SELECTED { get; set; } = false;
        public AvailabilityModel()
        {
            
        }
    }
}
