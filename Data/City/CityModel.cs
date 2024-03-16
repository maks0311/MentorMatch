using System;
using System.Collections.Generic;

namespace Mentor.Data
{
    [Serializable()]
    public class CityModel
    {
        public int CITY_ID { get; set; } = 0;
        public string CITY_NAME { get; set; } = String.Empty;

        public CityModel()
        {

        }
    }
}
