namespace Mentor.Data
{
    public class CompetenceModel
    {
        public int ID { get; set; } = 0;
        public int TUTOR_ID { get; set; } = 0;
        public int SUBJECT_ID { get; set; } = 0;
        public string SUBJECT_NAME { get; set; } = string.Empty;
        public int LEVEL_ID { get; set; } = 0;
        public string LEVEL_NAME { get; set; } = string.Empty;
        public string DESCRIPTION { get; set; } = string.Empty;

       
        public CompetenceModel() { }
    }
}
