using System;

namespace Mentor.Data
{
    [Serializable()]
    public class LevelModel
    {
        public int LEVEL_ID { get; set; } = 0;
        public string LEVEL_NAME { get; set; } = string.Empty;
        public bool IS_ACTIVE { get; set; } = true;
        
        public LevelModel() { }
    }
}
