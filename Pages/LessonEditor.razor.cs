using Mentor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Diagnostics;

namespace Mentor.Pages
{
    public partial class LessonEditor
    {
        [Parameter]
        public LessonModel LessonObject { get; set; }
        private bool IsObjectLessonChanged { get; set; } = false;
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;

        private string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        private int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private UserModel UserObject { get; set; } = new UserModel();
        private UserNotificationModel UserNotificationObject { get; set; } = new UserNotificationModel();

        private IEnumerable<SubjectModel> SubjectEnum;
        private IEnumerable<SubjectModel> SubjectEnumFiltered;
        private IEnumerable<LevelModel> LevelEnum;
        private IEnumerable<LevelModel> LevelEnumFiltered;
        private IEnumerable<CompetenceModel> CompetenceEnum;
        private IEnumerable<RatingModel> RatingEnum;
        private IEnumerable<UserNotificationModel> UserNotificationEnum;

        private readonly IEnumerable<TimeOnly> QuarterOnlyList = TimeHelper.GetDailyQuartersAsTimeOnly();
        private TermItem Term { get; set; } = new TermItem();
        private int PreviousRating { get; set; } = 0;

        private bool RatingIsReadOnly
        {
            get
            {
                if (LessonObject.IsNotNull() && UserObject.IsNotNull())
                    return (UserObject.IsTutor || !LessonObject.IsCreated || PreviousRating.IsPositive());
                else return true;
            }
        }

        private bool TopicIsReadOnly
        {
            get
            {
                if (LessonObject.IsNotNull() && UserObject.IsNotNull())
                    if (LessonObject.LESSON_STATUS_ID == 2 || LessonObject.LESSON_STATUS_ID == 1)
                    {
                        return false;
                    }
                    else if (LessonObject.LESSON_STATUS_ID == 1 && UserObject.IsTutor)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                else return true;
            }
        }

        private bool SaveEnabled
        {
            get
            {
                bool retval = false;

                if (LessonObject.IsNotNull())
                {
                    // ACCEPTED
                    if ((IsObjectLessonChanged) && (LessonObject.LESSON_STATUS_ID == 2))
                        retval = true;
                    // NEW
                    else if (LessonObject.IsCreated.IsFalse())
                        retval = true;
                    //NOT RATED
                    else if (LessonObject.HasEnded && LessonObject.IsRated && IsObjectLessonChanged)
                        retval = true;

                }

                return retval;
            }
        }

        private bool DateTimePickerEnabled
        {
            get
            {
                if (LessonObject.IsNotNull())
                {
                    return ((LessonObject.LESSON_STATUS_ID == 2) || ((LessonObject.LESSON_STATUS_ID == 1) && (!LessonObject.IsCreated)));
                }
                else return true;
            }
        }

        private bool AcceptEnabled
        {
            get
            {
                bool retval = false;

                if (LessonObject.IsNotNull())
                {
                    // PENDING
                    if ((LessonObject.LESSON_STATUS_ID == 1) && UserObject.IsTutor)
                        retval = true;
                    // POSTPONED BY STUDENT
                    else if ((LessonObject.LESSON_STATUS_ID == 4) && UserObject.IsTutor)
                        retval = true;
                    // POSTPONED BY TUTOR
                    else if ((LessonObject.LESSON_STATUS_ID == 5) && UserObject.IsStudent)
                        retval = true;
                }

                return retval;
            }
        }

        private bool CancelEnabled
        {
            get
            {
                bool retval = false;

                if (LessonObject.IsCreated && LessonObject.LESSON_STATUS_ID != 3 && LessonObject.LESSON_STATUS_ID != 6)
                {
                    retval = true;
                }

                return retval;
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
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private async Task InitializeData()
        {
            try
            {
                if (AppState.UserInfo.IsNotNull() && LessonObject.IsNotNull())
                {
                    UserObject = UserService.Select(AppState.UserInfo.USER_ID);
                    SubjectEnum = await SubjectService.SelectAllAsync();
                    CompetenceEnum = await CompetenceService.SelectAllByTutorAsync(LessonObject.TUTOR_ID);
                    UserNotificationEnum = await UserNotificationService.SelectAllNewByUserAsync(UserObject.USER_ID);
                    UserNotificationEnum = UserNotificationEnum.Where(x => x.LESSON_ID == LessonObject.LESSON_ID);

                    LevelEnum = await LevelService.SelectAllAsync();
                    RatingEnum = await RatingService.SelectAllAsync();

                    SubjectEnumFiltered = FilterSubjectList(LessonObject.TUTOR_ID);
                    LevelEnumFiltered = FilterLevelList(LessonObject.SUBJECT_ID, LessonObject.TUTOR_ID);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private IEnumerable<LevelModel> FilterLevelList(int subject_id, int tutor_id)
        {
            try
            {
                List<int> levelsOfTheSubject = CompetenceEnum.Where(x => x.TUTOR_ID == tutor_id && x.SUBJECT_ID == subject_id).Select(y => y.LEVEL_ID).ToList();
                return LevelEnum.Where(x => levelsOfTheSubject.Contains(x.LEVEL_ID));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
            return null;
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
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        protected override void OnParametersSet()
        {
            if (LessonObject.IsNotNull())
            {
                PreviousRating = LessonObject.RATING_VALUE;
                Term.SelectedDay = LessonObject.DATE_START;
                Term.TimeStart = TimeOnly.FromDateTime(LessonObject.DATE_START);
                Term.TimeStop = TimeOnly.FromDateTime(LessonObject.DATE_STOP);
            }
        }

        private void OnDateChange()
        {
            IsObjectLessonChanged = true;
            LessonObject.DATE_STOP = LessonObject.DATE_START;
        }

        private void OnDropDownChange(string key)
        {
            try
            {
                if (key == "SUBJECT")
                {
                    LevelEnumFiltered = FilterLevelList(LessonObject.SUBJECT_ID, LessonObject.TUTOR_ID);
                    LessonObject.LEVEL_ID = LevelEnumFiltered.First().LEVEL_ID;
                }

                IsObjectLessonChanged = true;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private void OnDayDropDownChange(string key)
        {
            try
            {
                if ((key == "TIME_START") && (Term.TimeStart >= Term.TimeStop))
                {
                    Term.TimeStop = Term.TimeStart.AddMinutes(15);
                }
                else if ((key == "TIME_STOP") && (Term.TimeStop <= Term.TimeStart))
                {
                    Term.TimeStart = Term.TimeStop.AddMinutes(-15);
                }

                LessonObject.DATE_START = new DateTime(LessonObject.DATE_START.Year, LessonObject.DATE_START.Month, LessonObject.DATE_START.Day, Term.TimeStart.Hour, Term.TimeStart.Minute, 0);
                LessonObject.DATE_STOP = new DateTime(LessonObject.DATE_STOP.Year, LessonObject.DATE_STOP.Month, LessonObject.DATE_STOP.Day, Term.TimeStop.Hour, Term.TimeStop.Minute, 0);

                IsObjectLessonChanged = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }


        private async Task LessonSave()
        {
            try
            {
                // NEW => PENDING ACCEPT
                if (LessonObject.IsCreated.IsFalse())
                {
                    LessonObject.LESSON_STATUS_ID = 1;
                    UserNotificationObject.NOTIFICATION_ID = 1;
                }

                // ACCEPTED => POSTPONED BY STUDENT
                if (LessonObject.LESSON_STATUS_ID == 2 && UserObject.IsStudent)
                {
                    LessonObject.LESSON_STATUS_ID = 4;
                    UserNotificationObject.NOTIFICATION_ID = 6;
                }

                // ACCEPTED => POSTPONED BY TUTOR
                if (LessonObject.LESSON_STATUS_ID == 2 && UserObject.IsTutor)
                {
                    LessonObject.LESSON_STATUS_ID = 5;
                    UserNotificationObject.NOTIFICATION_ID = 7;
                }

                var retval = await LessonService.UpsertAsync(LessonObject);

                if (retval.IsPositive())
                {
                    UserNotificationObject.LESSON_ID = retval;
                    UserNotificationObject.STUDENT_ID = LessonObject.STUDENT_ID;
                    UserNotificationObject.TUTOR_ID = LessonObject.TUTOR_ID;
                    UserNotificationObject.SENT_DATE = DateTime.Now;
                    await UserNotificationService.CreateAsync(UserNotificationObject);

                    foreach (UserNotificationModel notification in UserNotificationEnum)
                    {
                        if (UserObject.IsStudent)
                            await UserNotificationService.UpdateToReadStudentAsync(notification.ID);
                        if (UserObject.IsTutor)
                            await UserNotificationService.UpdateToReadTutorAsync(notification.ID);
                    }

                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Lesson saved" });
                    IsObjectLessonChanged = false;
                    StateHasChanged();
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                    DialogService.Close();
                }
                else
                {
                    switch (retval)
                    {
                        case -3:
                            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Lesson not saved", Detail = "Tutor is not available at selected time" });
                            break;
                        case -4:
                            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Lesson not saved", Detail = "Another student registered lesson at selected time" });
                            break;
                        case -5:
                            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Lesson not saved", Detail = "You registered another lesson at selected time" });
                            break;
                        default:
                            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Lesson not saved", Detail = "Something went wrong. Try again." });
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private async Task LessonAccept()
        {
            try
            {
                // PENDING => ACCEPTED
                if (LessonObject.LESSON_STATUS_ID == 1 && UserObject.IsTutor)
                {
                    LessonObject.LESSON_STATUS_ID = 2;
                    UserNotificationObject.NOTIFICATION_ID = 3;
                }

                // POSTPONED BY STUDENT => ACCEPTED
                if (LessonObject.LESSON_STATUS_ID == 4 && UserObject.IsTutor)
                {
                    LessonObject.LESSON_STATUS_ID = 2;
                    UserNotificationObject.NOTIFICATION_ID = 3;
                }

                // POSTPONED BY TUTOR => ACCEPTED
                if (LessonObject.LESSON_STATUS_ID == 5 && UserObject.IsStudent)
                {
                    LessonObject.LESSON_STATUS_ID = 2;
                    UserNotificationObject.NOTIFICATION_ID = 2; //change ID
                }

                var retval = await LessonService.UpsertAsync(LessonObject);

                if (retval.IsPositive())
                {
                    UserNotificationObject.LESSON_ID = retval;
                    UserNotificationObject.STUDENT_ID = LessonObject.STUDENT_ID;
                    UserNotificationObject.TUTOR_ID = LessonObject.TUTOR_ID;
                    UserNotificationObject.SENT_DATE = DateTime.Now;

                    await UserNotificationService.CreateAsync(UserNotificationObject);

                    foreach (UserNotificationModel notification in UserNotificationEnum)
                    {
                        if (UserObject.IsStudent)
                            await UserNotificationService.UpdateToReadStudentAsync(notification.ID);
                        if (UserObject.IsTutor)
                            await UserNotificationService.UpdateToReadTutorAsync(notification.ID);
                    }

                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Lesson accepted" });
                    IsObjectLessonChanged = false;
                    StateHasChanged();
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                    DialogService.Close();
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Lesson not accepted", Detail = "Something went wrong" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }


        }

        private async Task LessonCancel()
        {
            try
            {
                int retval;

                if (LessonObject.LESSON_STATUS_ID != 6 && LessonObject.LESSON_STATUS_ID != 3)
                {
                    LessonObject.LESSON_STATUS_ID = 3;
                    retval = await LessonService.UpsertAsync(LessonObject);
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Lesson not cancelled", Detail = "Inactive lessons cannot be cancelled." });
                    return;
                }

                if (retval.IsPositive())
                {
                    if (UserObject.IsTutor)
                    {
                        UserNotificationObject.NOTIFICATION_ID = 5;
                    }
                    else if (UserObject.IsStudent)
                    {
                        UserNotificationObject.NOTIFICATION_ID = 4;
                    }
                    UserNotificationObject.LESSON_ID = retval;
                    UserNotificationObject.STUDENT_ID = LessonObject.STUDENT_ID;
                    UserNotificationObject.TUTOR_ID = LessonObject.TUTOR_ID;
                    UserNotificationObject.SENT_DATE = DateTime.Now;

                    await UserNotificationService.CreateAsync(UserNotificationObject);

                    foreach (UserNotificationModel notification in UserNotificationEnum)
                    {
                        if (UserObject.IsStudent)
                            await UserNotificationService.UpdateToReadStudentAsync(notification.ID);
                        if (UserObject.IsTutor)
                            await UserNotificationService.UpdateToReadTutorAsync(notification.ID);
                    }

                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Lesson cancelled" });
                    IsObjectLessonChanged = false;
                    StateHasChanged();
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                    DialogService.Close();
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Lesson not cancelled", Detail = "Something went wrong. Try again." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private void OnTopicUpdate()
        {
            IsObjectLessonChanged = true;
            StateHasChanged();
        }

        private void OnRatingUpdate(int val)
        {
            try
            {
                RatingModel rating = RatingEnum.FirstOrDefault(x => x.RATING_VALUE == val);

                if (rating.IsNotNull())
                {
                    IsObjectLessonChanged = true;
                    LessonObject.RATING_ID = rating.RATING_ID;
                    LessonObject.RATING_VALUE = rating.RATING_VALUE;
                    LessonObject.RATING_NAME = rating.RATING_NAME;

                    UserNotificationObject.NOTIFICATION_ID = 9;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
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
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
        }
    }
}
