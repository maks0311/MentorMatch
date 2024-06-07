namespace Mentor
{
    public static class Globals
    {
        public static string[] AdminHostNames
        {
            get { return new string[] { "DESKTOP-I5PE8FV", "VPS1052" }; }
        }

        public static readonly string key = "b14ca5898a4e4133bbce2ea2315a2000";

        public static readonly string ColorStatusPending = "darkorange";
        public static readonly string ColorStatusAccepted = "#50C878";
        public static readonly string ColorStatusCancelled = "red";
        public static readonly string ColorStatusEnded = "#6082B6";
        public static readonly string ColorInactive = "silver";
        public static readonly string ColorToday = "lightyellow";
        public static readonly string ColorAvailability = "#F0F8FF";
        public static readonly string ColorTooltip = "background: #35A0D7";

        private static readonly string StylePrefix = "background: ";
        public static readonly string CalendarStyleLessonStatusPending = StylePrefix + ColorStatusPending;
        public static readonly string CalendarStyleLessonStatusAccepted = StylePrefix + ColorStatusAccepted;
        public static readonly string CalendarStyleLessonStatusCancelled = StylePrefix + ColorStatusCancelled;
        public static readonly string CalendarStyleLessonStatusEnded = StylePrefix + ColorStatusEnded;

        public static readonly string CalendarStyleLessonInactive = StylePrefix + ColorInactive;
        public static readonly string CalendarStyleToday = StylePrefix + ColorToday;
        public static readonly string CalendarStyleAvailability = StylePrefix + ColorAvailability;

    }
}
