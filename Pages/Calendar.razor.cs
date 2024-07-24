using Mentor.Data;
using Mentor.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Pages
{
    public partial class Calendar
    {
        AppState AppState { get; set; } = new AppState();
        string Msg { get; set; }
        private bool IsRendered { get; set; } = false;

        RadzenScheduler<LessonModel> Scheduler;
        TopMenu Menu;
        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private UserModel UserObject { get; set; } = new UserModel();
        private LessonModel LessonObject { get; set; } = new LessonModel();
        private DailySlotList DailySlotList = new();
        private readonly TermItem Term = new();
        private int AvMode { get; set; } = 1;

        private DateTime TimeScopeStart { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        private DateTime TimeScopeStop { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0).AddMonths(2);

        private bool AvailablityCreateDisabled
        {
            get
            {
                if (AvMode == 1)
                {
                    // SINGLE TERM MODE
                    if (Term.DurationAsTimeSpan.TotalMinutes > 0)
                        return false;
                    else
                        return true;
                }
                else
                {
                    // RECURING TERM MODE
                    if (DailySlotList.DurationWeeklyTimeSpan.TotalMinutes > 0)
                        return false;
                    else
                        return true;
                }
            }
        }

        private static bool AvailablityRemoveDisabled
        {
            get
            {
                return false;
            }
        }

        private readonly IEnumerable<TimeOnly> QuarterOnlyList = TimeHelper.GetDailyQuartersAsTimeOnly();
        private IEnumerable<LessonModel> LessonEnum;
        private IEnumerable<SubjectModel> SubjectEnum;
        private IEnumerable<CompetenceModel> CompetenceEnum;
        private IEnumerable<AvailabilityModel> AvailabilityEnum;
        private IEnumerable<AvailabilityModel> AvailabilityEnumFiltered;

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
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
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
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async Task InitializeData()
        {
            try
            {
                if (AppState.UserInfo.USER_ID.IsPositive())
                {
                    UserObject = await UserService.SelectAsync(AppState.UserInfo.USER_ID);
                    SubjectEnum = await SubjectService.SelectAllAsync();
                    CompetenceEnum = await CompetenceService.SelectAllByTutorAsync(AppState.UserInfo.USER_ID);
                    SubjectEnum = FilterSubjectList(AppState.UserInfo.USER_ID);

                    AppState.SetParam("CALENDAR_OWNER_ID", UserObject.USER_ID);
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);

                    if (UserObject.IsTutor || UserObject.IsAdmin)
                    {
                        LessonEnum = await LessonService.SelectAllByTutorAsync(AppState.UserInfo.USER_ID);
                        AvailabilityEnum = await AvailabilityService.SelectAllAsync(AppState.UserInfo.USER_ID, TimeScopeStart, TimeScopeStop);
                        DailySlotList = new DailySlotList(AppState.UserInfo.USER_ID);
                    }
                    else if (UserObject.IsStudent)
                    {
                        LessonEnum = await LessonService.SelectAllByStudentAsync(AppState.UserInfo.USER_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private IEnumerable<SubjectModel> FilterSubjectList(int tutor_id)
        {
            try
            {
                List<int> tutorSubjects = CompetenceEnum.Where(x => x.TUTOR_ID == tutor_id).Select(y => y.SUBJECT_ID).ToList();
                return SubjectEnum.Where(x => tutorSubjects.Contains(x.SUBJECT_ID));
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
            return null;

        }

        private static void OnLessonRender(SchedulerAppointmentRenderEventArgs<LessonModel> args)
        {
            try
            {
                switch (args.Data.LESSON_STATUS_ID)
                {
                    case 1:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatusPending;
                        break;
                    case 2:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatusAccepted;
                        break;
                    case 4:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatusPending;
                        break;
                    case 5:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatusPending;
                        break;
                    case 6:
                        args.Attributes["style"] = Globals.CalendarStyleLessonStatusEnded;
                        break;
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }

        }

        private void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            try
            {
                if (UserObject.IsTutor)
                {
                    AvailabilityEnumFiltered = AvailabilityEnum.Where(x => x.DATE_START >= args.View.StartDate && x.DATE_STOP < args.View.EndDate);

                    if (args.View.Text == "Month")
                    {
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

                if (args.View.Text == "Month")
                {
                    // Highlight today in month view
                    if (args.Start.Date == DateTime.Today)
                    {
                        args.Attributes["style"] = Globals.CalendarStyleToday;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async Task OnLessonSelect(SchedulerAppointmentSelectEventArgs<LessonModel> args)
        {
            try
            {
                LessonObject = await LessonService.SelectAsync(args.Data.LESSON_ID);

                await DialogService.OpenAsync<LessonEditor>(String.Format("Lesson - {0}", LessonObject.LESSON_STATUS_NAME), new Dictionary<string, object> { { "LessonObject", LessonObject } });
                if (UserObject.IsTutor)
                {
                    LessonEnum = await LessonService.SelectAllByTutorAsync(AppState.UserInfo.USER_ID);
                }
                else if (UserObject.IsStudent)
                {
                    LessonEnum = await LessonService.SelectAllByStudentAsync(AppState.UserInfo.USER_ID);
                }
                await Menu.Reload();
                await Scheduler.Reload();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private void OnDayDropDownChange(string key)
        {
            try
            {
                if ((key == "TIME_START") && (Term.TimeStart > Term.TimeStop))
                {
                    Term.TimeStop = Term.TimeStart;
                }
                else if ((key == "TIME_STOP") && (Term.TimeStop < Term.TimeStart))
                {
                    Term.TimeStart = Term.TimeStop;
                }

                if ((key == "DATE_RECURING_START") && (DailySlotList.DateStart > DailySlotList.DateStop))
                {
                    DailySlotList.DateStop = DailySlotList.DateStart;
                }
                else if (key == "DATE_RECURING_STOP" && (DailySlotList.DateStop < DailySlotList.DateStart))
                {
                    DailySlotList.DateStart = DailySlotList.DateStop;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private void OnWeekDropDownChange(string key, int weekday)
        {
            try
            {
                if (DailySlotList.Items.Count > 0)
                {
                    var start = DailySlotList.Items[weekday - 1].TimeStart;
                    var stop = DailySlotList.Items[weekday - 1].TimeStop;

                    if ((key == "TIME_RECURING_START") && (start > stop))
                    {
                        DailySlotList.Items[weekday - 1].TimeStop = DailySlotList.Items[weekday - 1].TimeStart;
                    }
                    else if ((key == "TIME_RECURING_STOP") && (stop < start))
                    {
                        DailySlotList.Items[weekday - 1].TimeStart = DailySlotList.Items[weekday - 1].TimeStop;
                    }
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }


        public async Task<int> CreateDailyAvailability(AvailabilityMode mode)
        {
            try
            {
                int retCreate = 0;
                int retDel = 0;
                var av_new = new List<AvailabilityModel>();

                DateTime DayStart = DateTime.Now;
                DateTime DayStop = DateTime.Now;

                if (@AvMode == 1)
                {
                    DayStart = Term.SelectedDay;
                    DayStop = Term.SelectedDay;

                    AvailabilityModel item = new()
                    {
                        AVAILABILITY_ID = 0,
                        TUTOR_ID = AppState.UserInfo.USER_ID,
                        DATE_START = new DateTime(Term.SelectedDay.Year, Term.SelectedDay.Month, Term.SelectedDay.Day, Term.TimeStart.Hour, Term.TimeStart.Minute, 0),
                        DATE_STOP = new DateTime(Term.SelectedDay.Year, Term.SelectedDay.Month, Term.SelectedDay.Day, Term.TimeStop.Hour, Term.TimeStop.Minute, 0),
                        IS_SELECTED = true
                    };
                    av_new.Add(item);
                }
                else
                {
                    DayStart = DailySlotList.DateStart;
                    DayStop = DailySlotList.DateStop;

                    var days = TimeHelper.EachDay(DayStart, DayStop);
                    foreach (DateTime day_in_period in days)
                    {
                        var x = day_in_period.DayOfWeek.ToInt();
                        foreach (var daily_slot in DailySlotList.Items)
                        {
                            if (daily_slot.Weekday == day_in_period.DayOfWeek.ToInt())
                            {
                                AvailabilityModel av_item = new()
                                {
                                    AVAILABILITY_ID = 0,
                                    TUTOR_ID = AppState.UserInfo.USER_ID,
                                    DATE_START = new DateTime(day_in_period.Year, day_in_period.Month, day_in_period.Day, daily_slot.TimeStart.Hour, daily_slot.TimeStart.Minute, 0),
                                    DATE_STOP = new DateTime(day_in_period.Year, day_in_period.Month, day_in_period.Day, daily_slot.TimeStop.Hour, daily_slot.TimeStop.Minute, 0),
                                    IS_SELECTED = true
                                };
                                av_new.Add(av_item);
                            }
                        }
                    }
                }

                if (!av_new.Any()) { return 0; }

                // do for each day in selected period
                foreach (DateTime day_in_period in TimeHelper.EachDay(DayStart, DayStop))
                {
                    List<AvailabilityModel> av_matrix = TimeHelper.GetDailyQuartersAsAvailabilityList(AppState.UserInfo.USER_ID, day_in_period);

                    var start = new DateTime(day_in_period.Year, day_in_period.Month, day_in_period.Day, 0, 0, 0);
                    var stop = start.AddDays(1);
                    var av_prev = await AvailabilityService.SelectAllAsync(AppState.UserInfo.USER_ID, start, stop);

                    foreach (var item in av_matrix)
                    {
                        var exists_prev = av_prev.FirstOrDefault(x => x.DATE_START <= item.DATE_START && x.DATE_STOP >= item.DATE_STOP);
                        var exists_new = av_new.FirstOrDefault(x => x.DATE_START <= item.DATE_START && x.DATE_STOP >= item.DATE_STOP);

                        if (mode == Mentor.AvailabilityMode.SET_AVAILABLE)
                        {
                            if (exists_prev.IsNotNull() || exists_new.IsNotNull())
                            {
                                item.IS_SELECTED = true;
                            }
                        }
                        else
                        {
                            if ((exists_prev.IsNotNull() && exists_new.IsNull()))
                            {
                                item.IS_SELECTED = true;
                            }
                        }
                    }

                    List<AvailabilityModel> lst = new();
                    bool start_found = false;
                    bool stop_found = false;

                    DateTime item_start = new();
                    DateTime item_stop = new();

                    foreach (var item in av_matrix)
                    {
                        if (item.IS_SELECTED.IsTrue() && start_found.IsFalse())
                        {
                            item_start = item.DATE_START;
                            start_found = true;
                        }

                        if (item.IS_SELECTED.IsFalse() && start_found.IsTrue())
                        {
                            item_stop = item.DATE_STOP.AddMinutes(-15);
                            stop_found = true;
                        }

                        if (start_found.IsTrue() && stop_found.IsTrue())
                        {
                            AvailabilityModel av = new()
                            {
                                AVAILABILITY_ID = 0,
                                TUTOR_ID = AppState.UserInfo.USER_ID,
                                DATE_START = item_start,
                                DATE_STOP = item_stop
                            };
                            lst.Add(av);

                            start_found = false;
                            stop_found = false;
                        }
                    }

                    retDel = await AvailabilityService.DeleteAsync(AppState.UserInfo.USER_ID, start, stop);

                    if (retDel.IsPositiveOrZero())
                    {
                        foreach (var item in lst)
                        {
                            retCreate += await AvailabilityService.CreateAsync(AppState.UserInfo.USER_ID, item.DATE_START, item.DATE_STOP);
                        }
                    }

                }

                if (retDel.IsPositive())
                {
                    AvailabilityEnum = await AvailabilityService.SelectAllAsync(AppState.UserInfo.USER_ID, TimeScopeStart, TimeScopeStop);
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Availablility Deleted", Detail = Msg });
                }

                if (retCreate.IsPositive())
                {
                    AvailabilityEnum = await AvailabilityService.SelectAllAsync(AppState.UserInfo.USER_ID, TimeScopeStart, TimeScopeStop);
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Availablility Saved", Detail = Msg });
                }

                return retCreate + retDel;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
            return 0;
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
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private void ShowTooltip(ElementReference elementReference, string msg)
        {
            try
            {
                TooltipOptions options = new() { Duration = NotificationDuration, Style = Globals.ColorTooltip };
                TooltipService.Open(elementReference, msg, options);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        public void Dispose()
        {
        }
    }
}
