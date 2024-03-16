namespace Mentor
{
    public static class Globals
    {
        public static string[] AdminHostNames
        {
            get { return new string[] { "DESKTOP-I5PE8FV", "VPS1052" }; }
        }

        public static string UserNotLoggedInMessage = "User is not logged in";

        public static string ColorStatus1 = "darkorange";
        public static string ColorStatus2 = "lightgreen";
        public static string ColorStatus3 = "red";
        public static string ColorStatus4 = "darkorange";
        public static string ColorStatus5 = "darkorange";
        public static string ColorStatus6 = "silver";
        public static string ColorStatus7 = "yellowgreen";
        public static string ColorInactive = "gray";
        public static string ColorToday = "lightyellow";
        public static string ColorAvailability = "peachpuff";

        private static string CalendarStylePrefix = "background: ";
        public static string CalendarStyleLessonStatus1 = CalendarStylePrefix + ColorStatus1;
        public static string CalendarStyleLessonStatus2 = CalendarStylePrefix + ColorStatus2;
        public static string CalendarStyleLessonStatus3 = CalendarStylePrefix + ColorStatus3;
        public static string CalendarStyleLessonStatus4 = CalendarStylePrefix + ColorStatus4;
        public static string CalendarStyleLessonStatus5 = CalendarStylePrefix + ColorStatus5;
        public static string CalendarStyleLessonStatus6 = CalendarStylePrefix + ColorStatus6;
        public static string CalendarStyleLessonStatus7 = CalendarStylePrefix + ColorStatus7;

        public static string CalendarStyleLessonInactive = CalendarStylePrefix + ColorInactive;
        public static string CalendarStyleToday = CalendarStylePrefix + ColorToday;
        public static string CalendarStyleAvailability = CalendarStylePrefix + ColorAvailability;

        private static string LegendPrefix = "overflow:hidden; white-space:nowrap; text-overflow:ellipsis; font-size:smaller; font-weight:bold; padding-top:5px; text-align:center; height:25px; width:max-content; background-color: ";
        public static string LegendStyleStatus1 = LegendPrefix + ColorStatus1;
        public static string LegendStyleStatus2 = LegendPrefix + ColorStatus2;
        public static string LegendStyleStatus3 = LegendPrefix + ColorStatus3;
        public static string LegendStyleStatus4 = LegendPrefix + ColorStatus4;
        public static string LegendStyleStatus5 = LegendPrefix + ColorStatus5;
        public static string LegendStyleStatus6 = LegendPrefix + ColorStatus6;
        public static string LegendStyleStatus7 = LegendPrefix + ColorStatus7;

    }
}
