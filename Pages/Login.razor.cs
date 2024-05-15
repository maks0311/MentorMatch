using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace Mentor.Pages
{
    public partial class Login
    {
        private AppState AppState { get; set; } = new AppState();
        private string msg { get; set; }
        private bool IsRendered { get; set; } = false;

        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        private string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        private int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private string UserName = string.Empty;
        private string UserPass = string.Empty;

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

        private void OnChange(string value, string key)
        {
            if (key == "USER_NAME")
            {
                UserName = value;
            }

            if (key == "USER_PASS")
            {
                UserPass = value;
            }
        }

        private async Task OnClick(string key)
        {

            if (key == "LOGIN")
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserPass))
                {
                    msg = "Username and Password cannot be empty. Please try again.";
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                    return;
                }

                int UserID = UserService.Authenticate(UserName, UserPass);

                if (UserID > 0)
                {
                    AppState.UserInfo = await UserService.SelectAsync(UserID);
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);

                    NavigationManager.NavigateTo("./");
                }
                else
                {
                    AppState.UserInfo = new();

                    switch (UserID)
                    {
                        case 0:
                            msg = "Incorrect password.";
                            break;
                        case -1:
                            msg = "Login failed. User [" + UserName + "] does not exist";
                            break;
                        case -2:
                            msg = "Cannot login. User [" + UserName + "] is disabled";
                            break;
                        default:
                            msg = "Login failed";
                            break;
                    }

                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = msg, Duration = NotificationDuration, Style = NotificationPosition });
                }
            }

            if (key == "LOGOUT")
            {
                AppState.UserInfo = new();
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
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
