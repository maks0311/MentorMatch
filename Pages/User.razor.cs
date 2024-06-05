using Mentor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mentor.Pages
{
    public partial class User
    {
        AppState AppState { get; set; } = new AppState();
        string Msg { get; set; }
        private bool IsRendered { get; set; } = false;

        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();
        private string Pass1 { get; set; } = string.Empty;
        private string Pass2 { get; set; } = string.Empty;
        private bool DisableSave { get; set; } = true;
        private bool DisablePasswordSave { get; set; } = true;
        private bool DisableAdd
        {
            get
            {
                return !(LevelEnum.IsNotNull() && LevelEnum.Any() && SubjectEnum.IsNotNull() && SubjectEnum.Any());
            }
        }
        private readonly int ColumnLabelSize = 2;
        private readonly int ColumnControlSize = 10;

        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private UserModel UserObject { get; set; } = new UserModel();
        private CompetenceModel CompetenceObject { get; set; } = new CompetenceModel();
        private LessonModel LessonObject { get; set; } = new LessonModel();
        private UserToCityModel UserToCityObject { get; set; } = new UserToCityModel();
        private CityModel CityObject { get; set; } = new CityModel();

        private IEnumerable<GroupModel> GroupEnum { get; set; }
        private IEnumerable<WorkTypeModel> WorkTypeEnum { get; set; }
        private IEnumerable<LevelModel> LevelEnum { get; set; }
        private IEnumerable<SubjectModel> SubjectEnum { get; set; }
        private IEnumerable<CompetenceModel> CompetenceEnum { get; set; }
        private IEnumerable<LessonModel> LessonEnum { get; set; }
        private IEnumerable<UserToCityModel> UserToCityEnum { get; set; }
        private IEnumerable<CityModel> CityEnum { get; set; }
        private IEnumerable<UserNotificationModel> UserNotificationEnum { get; set; }

        private readonly string NamePattern = "^[A-Za-z '-]+$";
        private readonly string phonePattern = @"^\+?\d{1,4}[-. ]?\(?\d{1,4}\)?[-. ]?\d{4,10}$";

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

                    if (UserObject.IsAdmin)
                    {
                        GroupEnum = await GroupService.SelectAllAsync();
                        GroupEnum = GroupEnum.Where(x => x.GROUP_ID == 1);
                    }
                    else
                    {
                        GroupEnum = await GroupService.SelectAllAsync();
                        GroupEnum = GroupEnum.Where(x => x.GROUP_ID > 1);
                    }

                    WorkTypeEnum = await WorkTypeService.SelectAllAsync();
                    LevelEnum = await LevelService.SelectAllAsync();
                    SubjectEnum = await SubjectService.SelectAllAsync();
                    CompetenceEnum = await CompetenceService.SelectAllByTutorAsync(UserObject.USER_ID);
                    UserNotificationEnum = await UserNotificationService.SelectAllByUserAsync(UserObject.USER_ID);
                    SwapVarsInNotifications();
                    AppState.TabIndex = 0;
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);

                    if (LevelEnum.IsNotNull() && LevelEnum.Any() && SubjectEnum.IsNotNull() && SubjectEnum.Any())
                    {
                        CompetenceObject.TUTOR_ID = UserObject.USER_ID;
                        CompetenceObject.SUBJECT_ID = SubjectEnum.FirstOrDefault().SUBJECT_ID;
                        CompetenceObject.LEVEL_ID = LevelEnum.FirstOrDefault().LEVEL_ID;
                    }

                    if (UserObject.IsTutor)
                    {
                        LessonEnum = await LessonService.SelectArchiveByTutorAsync(UserObject.USER_ID);
                        CityEnum = await CityService.SelectAllAsync();
                        UserToCityEnum = await UserToCityService.SelectAllByTutorAsync(UserObject.USER_ID);
                    }
                    else if (UserObject.IsStudent)
                    {
                        LessonEnum = await LessonService.SelectArchiveByStudentAsync(UserObject.USER_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, NamePattern);
        }

        private bool IsPhoneValid(string phone)
        {
            return Regex.IsMatch(phone, phonePattern);
        }

        private static bool IsDescValid(string description)
        {
            return description.Length < 250;
        }

        private async Task UserSave()
        {
            try
            {
                if (UserObject.IsNotNull() && UserObject.IsAuthenticated)
                {
                    if (string.IsNullOrEmpty(UserObject.USER_FULLNAME))
                    {
                        Msg = "Your full name cannot be empty. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "User info not saved", Detail = Msg });
                        return;
                    }

                    if (!IsValidName(UserObject.USER_FULLNAME))
                    {
                        Msg = "Invalid full name. Please use only letters, spaces, hyphens, and apostrophes.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "User info not saved", Detail = Msg });
                        return;
                    }

                    if (!IsPhoneValid(UserObject.USER_PHONE))
                    {
                        Msg = "Invalid phone number. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "User info not saved", Detail = Msg });
                        return;
                    }

                    if (!IsDescValid(UserObject.USER_DESCRIPTION))
                    {
                        Msg = "Description is too long.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "User info not saved", Detail = Msg });
                        return;
                    }

                    var retval = await UserService.UpdateAsync(UserObject);

                    if (retval == 1)
                    {
                        AppState.UserInfo = UserObject;
                        await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);

                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "User info saved", Detail = "Personal information updated." });
                        DisableSave = true;
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "User info not saved", Detail = "Something went wrong. Try again later." });
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task RevertChanges()
        {
            try
            {
                UserObject = await UserService.SelectAsync(UserObject.USER_ID);
                DisableSave = true;
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnChange()
        {
            DisableSave = false;
        }

        private void OnDropDownChange(object value, string key)
        {
            DisableSave = false;
        }

        private static bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            return true;

        }

        private void OnPasswordChange()
        {
            if (!String.IsNullOrEmpty(Pass1) && !String.IsNullOrEmpty(Pass2))
            {
                DisablePasswordSave = false;
            }
            else
            {
                DisablePasswordSave = true;
            }
        }

        private async Task PasswordSave()
        {
            try
            {
                if (!IsPasswordValid(Pass1))
                {
                    Msg = "Invalid password. Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter and one digit.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Password not saved", Detail = Msg });
                    return;
                }

                if (!Pass1.Equals(Pass2, StringComparison.InvariantCulture))
                {
                    Msg = "Password and its repetition are not identical, enter them again please.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Password not saved", Detail = Msg });
                    return;
                }

                int retval = await UserService.PasswordUpdateAsync(UserObject.USER_ID, EncryptionHelper.EncryptString(Globals.key, Pass1));

                if (retval.IsPositive())
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Password saved", Detail = "Password updated." });
                    DisablePasswordSave = true;
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Password not saved", Detail = "Something went wrong. Try again." });
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task AddCompetence()
        {
            try
            {
                if (UserObject.IsNotNull() && UserObject.IsAuthenticated && CompetenceObject.IsNotNull())
                {
                    var retval = await CompetenceService.CreateAsync(CompetenceObject);
                    if (retval.IsPositive())
                    {
                        CompetenceEnum = await CompetenceService.SelectAllByTutorAsync(UserObject.USER_ID);
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Competence saved", Detail = "New competence added." });
                    }
                    else if (retval.IsZero())
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Competence not saved", Detail = "This competence already exists." });
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Competence not saved", Detail = "Something went wrong. Try again." });
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task DeleteCompetence(int id)
        {
            try
            {
                if (id > 0)
                {
                    var retVal = CompetenceService.Delete(id);
                    if (retVal.IsPositive())
                    {
                        CompetenceEnum = await CompetenceService.SelectAllByTutorAsync(UserObject.USER_ID);
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Competence deleted" });
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Competence not deleted", Detail = "Something went wrong. Try again." });
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task AddCity()
        {
            try
            {
                if (UserObject.IsNotNull() && UserObject.IsAuthenticated && CityObject.IsNotNull())
                {
                    UserToCityObject.CITY_ID = CityObject.CITY_ID;
                    UserToCityObject.TUTOR_ID = UserObject.USER_ID;
                    var retval = await UserToCityService.CreateAsync(UserToCityObject);
                    if (retval.IsPositive())
                    {
                        UserToCityEnum = await UserToCityService.SelectAllByTutorAsync(UserObject.USER_ID);
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "City saved", Detail = "New city added." });
                    }
                    else if (retval.IsZero())
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "City not saved", Detail = "This city is already registered." });
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "City not saved", Detail = "Something went wrong. Try again." });
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task DeleteCity(int id)
        {
            try
            {
                if (id > 0)
                {
                    var retVal = await UserToCityService.DeleteAsync(id);
                    if (retVal.IsPositive())
                    {
                        UserToCityEnum = await UserToCityService.SelectAllByTutorAsync(UserObject.USER_ID);
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "City deleted" });
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "City not deleted", Detail = "Something went wrong. Try again." });
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void SwapVarsInNotifications()
        {
            try
            {
                foreach (UserNotificationModel notification in UserNotificationEnum)
                {
                    string tutor = notification.TUTOR_NAME;
                    string student = notification.STUDENT_NAME;
                    string subject = notification.LESSON_SUBJECT;
                    string rating = notification.RATING.ToString();

                    if (UserObject.IsTutor)
                        tutor = "You";
                    if (UserObject.IsStudent)
                        student = "You";

                    string text = notification.TEXT;

                    text = text.Replace("{student}", student);
                    text = text.Replace("{tutor}", tutor);
                    text = text.Replace("{subject}", subject);
                    text = text.Replace("{rating}", rating);

                    notification.TEXT = text;
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task NotificationOnRead(int notificationID)
        {
            try
            {
                int retval = 0;
                if (UserObject.IsTutor)
                {
                    retval = await UserNotificationService.UpdateToReadTutorAsync(notificationID);
                }
                else if (UserObject.IsStudent)
                {
                    retval = await UserNotificationService.UpdateToReadStudentAsync(notificationID);
                }

                if (retval.IsPositive())
                {
                    UserNotificationEnum = UserNotificationService.SelectAllByUser(UserObject.USER_ID);
                    SwapVarsInNotifications();
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Notification marked as read" });
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Notification not marked as read", Detail = "Something went wrong. Try again." });
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnRenderNotification(DataGridCellRenderEventArgs<UserNotificationModel> args)
        {
            try
            {
                if (UserObject.IsTutor)
                {
                    if (args.Data.TUTOR_READ)
                    {
                        args.Attributes.Add("style", $"background-color: #f6f7fa");
                    }
                    else
                    {
                        args.Attributes.Add("style", $"background-color: white;");

                    }
                }
                else
                {
                    if (args.Data.STUDENT_READ)
                    {
                        args.Attributes.Add("style", $"background-color: #f6f7fa;");
                    }
                    else
                    {
                        args.Attributes.Add("style", $"background-color: white;");
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }


        }

        private async Task OpenLessonDetails(int lessonID, int notificationID)
        {
            try
            {
                LessonObject = await LessonService.SelectAsync(lessonID);

                await DialogService.OpenAsync<LessonEditor>(String.Format("Lesson - {0}", LessonObject.LESSON_STATUS_NAME), new Dictionary<string, object> { { "LessonObject", LessonObject } });
                if (UserObject.IsTutor)
                {
                    LessonEnum = await LessonService.SelectArchiveByTutorAsync(AppState.UserInfo.USER_ID);
                }
                else if (UserObject.IsStudent)
                {
                    LessonEnum = await LessonService.SelectArchiveByStudentAsync(AppState.UserInfo.USER_ID);
                }
                if (notificationID.IsPositive())
                {
                    await NotificationOnRead(notificationID);
                }
                UserNotificationEnum = await UserNotificationService.SelectAllByUserAsync(UserObject.USER_ID);
                SwapVarsInNotifications();
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
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
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void ShowTooltip(ElementReference elementReference, string msg)
        {
            TooltipOptions options = new TooltipOptions() { Duration = NotificationDuration };
            TooltipService.Open(elementReference, msg, options);
        }
        public void Dispose()
        {

        }
    }
}

