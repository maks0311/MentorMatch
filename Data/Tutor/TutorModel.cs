using System;
using System.Collections.Generic;

namespace Mentor.Data
{
    [Serializable()]
    public class TutorModel
    {
        public int TUTOR_ID { get; set; } = 0;
        public string TUTOR_FULLNAME { get; set; } = String.Empty;
        public string TUTOR_EMAIL { get; set; } = String.Empty;
        public string TUTOR_PHONE { get; set; } = String.Empty;
        public string TUTOR_DESCRIPTION { get; set; } = String.Empty;
        public int WORK_TYPE_ID { get; set; } = 0;
        public string WORK_TYPE_NAME { get; set; } = String.Empty;
        public int TUTOR_RATING { get; set; } = 0;
        public List<CompetenceModel> Competences { get; set; } = new List<CompetenceModel>();
        public List<CityModel> Cities { get; set; } = new List<CityModel>();
        public TutorModel()
        {

        }
    }
}