using System;

namespace Mentor.Data
{
    [Serializable()]
    public class SubjectModel
    {
        public int SUBJECT_ID { get; set; } = 0;
        public string SUBJECT_NAME { get; set; } = String.Empty;
        public bool IS_ACTIVE { get; set; } = true;
        public SubjectModel() { }
    }
}