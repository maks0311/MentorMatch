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
        private string msg { get; set; }
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


        protected override void OnInitialized()
        {
            base.OnInitialized();
            IsRendered = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeSession();
                await InitializeData();

                IsRendered = true;

                StateHasChanged();
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
            if (key == "SIGNUP")
            {
                if (string.IsNullOrEmpty(UserObject.USER_FULLNAME))
                {
                    msg = "Your full name cannot be empty. Please try again.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsNameValid(UserObject.USER_FULLNAME))
                {
                    msg = "Invalid full name. Please use only letters, spaces, hyphens, and apostrophes.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (string.IsNullOrEmpty(UserObject.USER_NICKNAME))
                {
                    msg = "Your  nickname cannot be empty. Please try again.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsNicknameValid(UserObject.USER_NICKNAME))
                {
                    msg = "Invalid nickname format. Nickname must be 3 to 20 characters and contain only letters, numbers, underscores, and hyphens.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsNicknameUnique(UserObject.USER_NICKNAME))
                {
                    msg = "Nickname is already in use. Please choose a different nickname.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (string.IsNullOrEmpty(UserObject.USER_EMAIL))
                {
                    msg = "Your email cannot be empty. Please try again.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsEmailValid(UserObject.USER_EMAIL))
                {
                    msg = "Invalid email or already in use. Please try again.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsPhoneValid(UserObject.USER_PHONE))
                {
                    msg = "Invalid phone number. Please try again.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsPhoneValid(UserObject.USER_PHONE))
                {
                    msg = "Invalid phone number. Please enter a valid phone number.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (string.IsNullOrEmpty(UserObject.USER_PASS))
                {
                    msg = "Your password cannot be empty. Please try again.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsPasswordValid(UserObject.USER_PASS))
                {
                    msg = "Invalid password. Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter and one digit.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (string.IsNullOrEmpty(UserPassRepeat) || UserPassRepeat != UserObject.USER_PASS)
                {
                    msg = "Your password has to match. Please try again.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsGroupValid(UserObject.GROUP_ID))
                {
                    msg = "Invalid user type selected.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Validation Error", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                if (!IsWorkTypeValid(UserObject.WORK_TYPE_ID))
                {
                    msg = "Invalid work type selected.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Validation Error", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                UserObject.USER_ID = await UserService.CreateAsync(UserObject);
                await UserService.PasswordUpdateAsync(UserObject.USER_ID, Encryption.EnryptString(UserObject.USER_PASS));
                msg = "User " + UserObject.USER_NICKNAME + " created.";
                ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Sign up succesful", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                NavigationManager.NavigateTo("./");
            }
        }

        private void ShowNotification(NotificationMessage message)
        {
            try
            {
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
