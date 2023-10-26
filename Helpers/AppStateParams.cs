using System;

namespace Mentor
{
    [Serializable()]
    public class AppStateParams
    {
        public string Key { get; set; } = String.Empty;
        public string Val { get; set; } = String.Empty;

        public AppStateParams()
        {

        }
    }
}
