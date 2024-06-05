using Mentor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

namespace Mentor.Pages
{
    public partial class SignUp
    {
        private AppState AppState { get; set; } = new AppState();
        private string Msg { get; set; }
        private bool IsRendered { get; set; } = false;

        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        private string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        private int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }


        private UserModel UserObject { get; set; } = new UserModel();
        private string UserPassRepeat = string.Empty;

        private readonly string NamePattern = "^[A-Za-z '-]+$";
        private readonly string NicknamePattern = "^[a-zA-Z0-9_-]+$";
        private readonly string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private readonly string phonePattern = @"^\+?\d{1,4}[-. ]?\(?\d{1,4}\)?[-. ]?\d{4,10}$";
        private const int MinNicknameLength = 3;
        private const int MaxNicknameLength = 20;

        private IEnumerable<GroupModel> GroupEnum { get; set; }
        private IEnumerable<WorkTypeModel> WorkTypeEnum { get; set; }

        private bool IsWorkTypeVisible
        {
            get
            {
                if(UserObject.GROUP_ID == 2)
                {
                    return true;
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
                    await InitializeData();

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
                GroupEnum = await GroupService.SelectAllAsync();
                GroupEnum = GroupEnum.Where(x => x.GROUP_ID != 1);

                WorkTypeEnum = await WorkTypeService.SelectAllAsync();
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private bool IsNameValid(string name)
        {
            return Regex.IsMatch(name, NamePattern);
        }

        private bool IsNicknameValid(string nickname)
        {
            if (nickname.Length < MinNicknameLength || nickname.Length > MaxNicknameLength)
            {
                return false;
            }
            return Regex.IsMatch(nickname, NicknamePattern);
        }

        private bool IsNicknameUnique(string nickname)
        {
            UserModel user = UserService.SelectByNick(nickname);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        private bool IsEmailValid(string email)
        {
            UserModel user = UserService.SelectByEmail(email);
            if (user != null)
            {
                return false;
            }
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsPhoneValid(string phone)
        {
            return Regex.IsMatch(phone, phonePattern);
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

        private static bool IsWorkTypeValid(int workTypeID)
        {
            return workTypeID == 1 || workTypeID == 2 || workTypeID == 3;
        }

        private static bool IsGroupValid(int groupID)
        {
            return groupID == 1 || groupID == 2 || groupID == 3;
        }

        private async Task OnClick(string key)
        {
            try
            {
                if (key == "SIGNUP")
                {
                    if (string.IsNullOrEmpty(UserObject.USER_FULLNAME))
                    {
                        Msg = "Your full name cannot be empty. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (!IsNameValid(UserObject.USER_FULLNAME))
                    {
                        Msg = "Invalid full name. Please use only letters, spaces, hyphens, and apostrophes.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (string.IsNullOrEmpty(UserObject.USER_NICKNAME))
                    {
                        Msg = "Your nickname cannot be empty. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (!IsNicknameValid(UserObject.USER_NICKNAME))
                    {
                        Msg = "Invalid nickname format. Nickname must be 3 to 20 characters and contain only letters, numbers, underscores, and hyphens.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (!IsNicknameUnique(UserObject.USER_NICKNAME))
                    {
                        Msg = "Nickname is already in use. Please choose a different nickname.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (string.IsNullOrEmpty(UserObject.USER_EMAIL))
                    {
                        Msg = "Your email cannot be empty. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (!IsEmailValid(UserObject.USER_EMAIL))
                    {
                        Msg = "Invalid email or already in use. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration" });
                        return;
                    }

                    if (!IsPhoneValid(UserObject.USER_PHONE))
                    {
                        Msg = "Invalid phone number. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (!IsPhoneValid(UserObject.USER_PHONE))
                    {
                        Msg = "Invalid phone number. Please enter a valid phone number.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (string.IsNullOrEmpty(UserObject.USER_PASS))
                    {
                        Msg = "Your password cannot be empty. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (!IsPasswordValid(UserObject.USER_PASS))
                    {
                        Msg = "Invalid password. Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter and one digit.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (string.IsNullOrEmpty(UserPassRepeat) || UserPassRepeat != UserObject.USER_PASS)
                    {
                        Msg = "Your password has to match. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (!IsGroupValid(UserObject.GROUP_ID))
                    {
                        Msg = "Invalid user type selected.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    if (!IsWorkTypeValid(UserObject.WORK_TYPE_ID))
                    {
                        Msg = "Invalid work type selected.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Registration", Detail = Msg });
                        return;
                    }

                    UserObject.USER_ID = await UserService.CreateAsync(UserObject);

                    if (UserObject.USER_ID == -1)
                    {
                        Msg = "Something went wrong. User " + UserObject.USER_NICKNAME + " not created. Try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Registration usuccesful", Detail = Msg });
                        NavigationManager.NavigateTo("./");
                        return;
                    }

                    await UserService.PasswordUpdateAsync(UserObject.USER_ID, EncryptionHelper.EncryptString(Globals.key, UserObject.USER_PASS));
                    Msg = "User " + UserObject.USER_NICKNAME + " created.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Registration succesful", Detail = Msg });
                    NavigationManager.NavigateTo("./");
                }
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
            try
            {
                TooltipOptions options = new TooltipOptions() { Duration = NotificationDuration };
                TooltipService.Open(elementReference, msg, options);
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        public void Dispose()
        {
        }
    }
}
