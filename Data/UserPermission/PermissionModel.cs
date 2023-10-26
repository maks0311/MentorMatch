using System;

namespace Mentor.Data
{
    [Serializable()]
    public class PermissionModel
    {
        public int ID { get; set; } = 0;
        public int USER_GROUP_ID { get; set; } = 0;
        public string USER_GROUP_NAME { get; set; } = String.Empty;
        public int ELEMENT_ID { get; set; } = 0;
        public string ELEMENT_NAME { get; set; } = String.Empty;
        public bool IS_SELECTED { get; set; } = false;

        public PermissionModel()
        {

        }
    }
}
