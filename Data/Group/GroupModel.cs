using System;

namespace Mentor.Data
{
    [Serializable()]
    public class GroupModel
    {
        public int GROUP_ID { get; set; } = 0;
        public string GROUP_NAME { get; set; } = String.Empty;

        public GroupModel()
        {
            
        }
    }
}
