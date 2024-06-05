using System;

namespace Mentor.Data
{
    [Serializable()]
    public class UserModel
    {
        public int USER_ID { get; set; } = 0;
        public string USER_FULLNAME { get; set; } = String.Empty;
        public string USER_NICKNAME { get; set; } = String.Empty;
        public string USER_EMAIL { get; set; } = String.Empty;
        public string USER_PHONE { get; set; } = String.Empty;
        public string USER_PASS { get; set; } = String.Empty;
        public string USER_DESCRIPTION { get; set; } = String.Empty;
        public bool IS_ACTIVE { get; set; } = true;
        public int GROUP_ID { get; set; } = 0;
        public string GROUP_NAME { get; set; } = String.Empty;
        public int WORK_TYPE_ID { get; set; } = 3;
        public string WORK_TYPE_NAME { get; set; } = String.Empty;

        public bool IsAuthenticated
        {
            get { return (USER_ID > 0); }
        }
        public bool IsStudent
        {
            get { return (GROUP_ID == 3); }
        }
        public bool IsTutor
        {
            get { return (GROUP_ID == 2); }
        }
        public bool IsAdmin
        {
            get { return (GROUP_ID == 1); }
        }
        public UserModel()
        {
        }
    }
}
