using Mentor.Data;
using Radzen.Blazor;
using Radzen;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace Mentor.Pages
{
    public partial class Tutor
    {
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;

        private static readonly NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        RadzenScheduler<LessonModel> Scheduler;
        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }
        private DateTime TimeScopeStart { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        private DateTime TimeScopeStop { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0).AddMonths(2);

        private int TutorID { get; set; } = 0;
        private UserModel UserObject { get; set; } = new UserModel();
        private TutorModel TutorObject { get; set; } = new TutorModel();
        private LessonModel LessonObject { get; set; } = new LessonModel();
        private IEnumerable<LevelModel> LevelEnum { get; set; }
        private IEnumerable<SubjectModel> SubjectEnum { get; set; }
        private IEnumerable<CompetenceModel> CompetenceEnum { get; set; }
        private IEnumerable<LessonModel> LessonEnum { get; set; }
        private IEnumerable<LessonStatusModel> LessonStatusEnum { get; set; }
        private IEnumerable<AvailabilityModel> AvailabilityEnum { get; set; }
        private IEnumerable<AvailabilityModel> AvailabilityEnumFiltered { get; set; }
        private IEnumerable<LevelModel> LevelEnumFiltered { get; set; }
        private IEnumerable<UserToCityModel> UserToCityEnum { get; set; }

        private bool IsCalendarVisible
        {
            get
            {
                if (UserObject.IsNotNull())
                {
                    return !UserObject.IsTutor;
                }
                else
                {
                    return false;
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            IsRendered = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await InitializeSession();

                    IsRendered = true;

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task InitializeSession()
        {
            try
            {
                if (await SessionStorage.ContainKeyAsync("APP_STATE"))
                {
                    AppState = await SessionStorage.GetItemAsync<AppState>("APP_STATE");
                    await InitializeData();
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task InitializeData()
        {
            try
            {
                if (AppState.UserInfo.IsNotNull())
                {
                    UserObject = await UserService.SelectAsync(AppState.UserInfo.USER_ID);
                    TutorID = AppState.GetParamAsInteger("TUTOR_ID", 0);
                    TutorObject = await TutorService.SelectAsync(TutorID);

                    if (UserObject.IsNotNull() && TutorObject.IsNotNull())
                    {
                        LevelEnum = await LevelService.SelectAllAsync();
                        SubjectEnum = await SubjectService.SelectAllAsync();
                        LessonStatusEnum = await LessonStatusService.SelectAllAsync();
                        CompetenceEnum = await CompetenceService.SelectAllByTutorAsync(TutorID);

                        LevelEnumFiltered = FilterLevelList(LessonObject.SUBJECT_ID);
                        SubjectEnum = FilterSubjectList();

                        LessonEnum = await LessonService.SelectAllByTutorAsync(TutorID);
                        AvailabilityEnum = await AvailabilityService.SelectAllAsync(TutorID, TimeScopeStart, TimeScopeStop);

                        UserToCityEnum = await UserToCityService.SelectAllByTutorAsync(TutorID);

                        AppState.SetParam("CALENDAR_OWNER_ID", TutorID);
                        await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            try
            {
                AvailabilityEnumFiltered = AvailabilityEnum.Where(x => x.DATE_START >= args.View.StartDate && x.DATE_STOP < args.View.EndDate);

                if (args.View.Text == "Month")
                {
                    // Highlight today in month view
                    if (args.Start.Date == DateTime.Today)
                    {
                        args.Attributes["style"] = Globals.CalendarStyleToday;
                    }

                    foreach (AvailabilityModel av in AvailabilityEnumFiltered)
                    {
                        if (args.Start.Date == av.DATE_START.Date)
                        {
                            args.Attributes["style"] = Globals.CalendarStyleAvailability;
                        }
                    }
                }

                if (args.View.Text == "Week")
                {
                    foreach (AvailabilityModel av in AvailabilityEnumFiltered)
                    {
                        if (args.Start.Date == av.DATE_START.Date && args.Start.TimeOfDay >= av.DATE_START.TimeOfDay && args.Start.TimeOfDay < av.DATE_STOP.TimeOfDay)
                        {
                            args.Attributes["style"] = Globals.CalendarStyleAvailability;
                        }
                    }
                }

                if (args.View.Text == "Day")
                {
                    foreach (AvailabilityModel av in AvailabilityEnumFiltered)
                    {
                        if (args.Start.Date == av.DATE_START.Date && args.Start.TimeOfDay >= av.DATE_START.TimeOfDay && args.Start.TimeOfDay < av.DATE_STOP.TimeOfDay)
                        {
                            args.Attributes["style"] = Globals.CalendarStyleAvailability;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            try
            {
                // when selected not-available slot
                if (AvailabilityEnum.Where(x => x.DATE_START <= args.Start && x.DATE_STOP >= args.End).Count().IsZero()) 
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Time not available", Detail = "Select available time." });
                    return; 
                }

                // when selected past slot
                if (args.Start < DateTime.Now)
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Time not available", Detail = "Only future lesson can be registered." });
                    return;
                }

                var tutor = UserService.Select(TutorID);
                var student = UserService.Select(UserObject.USER_ID);
                var status = LessonStatusEnum.OrderBy(x => x.LESSON_STATUS_ID).FirstOrDefault();
                var subject = SubjectEnum.OrderBy(x => x.SUBJECT_ID).FirstOrDefault();

                LevelEnumFiltered = FilterLevelList(subject.SUBJECT_ID);

                var level = LevelEnumFiltered.OrderBy(x => x.LEVEL_ID).FirstOrDefault();

                if (tutor.IsNull() || student.IsNull() || status.IsNull() || subject.IsNull() || level.IsNull()) { return; }

                LessonModel lesson = new()
                {
                    TUTOR_ID = tutor.USER_ID,
                    TUTOR_NAME = tutor.USER_NICKNAME,
                    STUDENT_ID = student.USER_ID,
                    STUDENT_NAME = student.USER_NICKNAME,
                    DATE_START = args.Start,
                    DATE_STOP = args.End,
                    LESSON_STATUS_ID = status.LESSON_STATUS_ID,
                    LESSON_STATUS_NAME = status.LESSON_STATUS_NAME,
                    LESSON_STATUS_ICON = status.LESSON_STATUS_ICON,
                    LESSON_STATUS_ICON_COLOR = status.LESSON_STATUS_ICON_COLOR,
                    SUBJECT_ID = subject.SUBJECT_ID,
                    SUBJECT_NAME = subject.SUBJECT_NAME,
                    LEVEL_ID = level.LEVEL_ID,
                    LEVEL_NAME = level.LEVEL_NAME
                };

                await DialogService.OpenAsync<LessonEditor>("Add New Lesson", new Dictionary<string, object> { { "LessonObject", lesson } });
                LessonEnum = await LessonService.SelectAllByTutorAsync(TutorID);
                await Scheduler.Reload();
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task OnLessonSelect(SchedulerAppointmentSelectEventArgs<LessonModel> args)
        {
            try
            {
                LessonObject = await LessonService.SelectAsync(args.Data.LESSON_ID);

                if (LessonObject.IsNotNull() && (LessonObject.STUDENT_ID == UserObject.USER_ID))
                {
                    await DialogService.OpenAsync<LessonEditor>(String.Format("Lesson - {0}", LessonObject.LESSON_STATUS_NAME), new Dictionary<string, object> { { "LessonObject", LessonObject } });
                    LessonEnum = await LessonService.SelectAllByTutorAsync(TutorID);
                    await Scheduler.Reload();
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Cannot edit lesson", Detail = "This lesson belongs to other student." });
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnLessonRender(SchedulerAppointmentRenderEventArgs<LessonModel> args)
        {
            if (args.Data.STUDENT_ID == AppState.UserInfo.USER_ID)
            {
                switch (args.Data.LESSON_STATUS_ID)
                {
                    case 1:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatus1;
                        break;
                    case 2:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatus2;
                        break;
                    case 3:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatus3;
                        break;
                    case 4:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatus4;
                        break;
                    case 5:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatus5;
                        break;
                    case 6:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatus6;
                        break;
                }
            }
            else
            {
                args.Attributes["style"] = Globals.CalendarStyleLessonInactive;
            }
        }

        private void OnLessonHover(SchedulerAppointmentMouseEventArgs<LessonModel> args)
        {
            if (args.Data.STUDENT_ID == AppState.UserInfo.USER_ID)
            {
                ShowTooltip(args.Element, args.Data.LESSON_STATUS_NAME);
            }
        }

        private IEnumerable<LevelModel> FilterLevelList(int subjectID)
        {
            try
            {
                List<int> levelsOfTheSubject = CompetenceEnum.Where(x => x.TUTOR_ID == TutorID && x.SUBJECT_ID == subjectID).Select(y => y.LEVEL_ID).ToList();
                return LevelEnum.Where(x => levelsOfTheSubject.Contains(x.LEVEL_ID));
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return null;
        }

        private IEnumerable<SubjectModel> FilterSubjectList()
        {
            try
            {
                List<int> tutorSubjects = CompetenceEnum.Where(x => x.TUTOR_ID == TutorID).Select(y => y.SUBJECT_ID).ToList();
                return SubjectEnum.Where(x => tutorSubjects.Contains(x.SUBJECT_ID));
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return null;
        }
        private void ShowNotification(NotificationMessage message)
        {
            try
            {
                message.Style = NotificationPosition;
                message.Duration = NotificationDuration;
                NotificationService.Notify(message);
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void ShowTooltip(ElementReference elementReference, string msg)
        {
            TooltipOptions options = new TooltipOptions() { Duration = NotificationDuration };
            TooltipService.Open(elementReference, msg, options);
        }
    }
}
