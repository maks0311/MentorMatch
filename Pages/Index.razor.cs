using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Radzen;
using Microsoft.Extensions.Configuration;

namespace Mentor.Pages
{
    public partial class Index
    {
        AppState AppState { get; set; } = new AppState();

        static string Today { get { return DateTime.Now.ToString("D", new CultureInfo("en-EN")); } }
        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }
        private bool IsRendered { get; set; } = false;
        private static readonly NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();
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
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task AdminAutoLogon()
        {
            try
            {
                if (Globals.AdminHostNames.Contains(System.Environment.MachineName))
                {
                    int UserID = UserInfoService.Authenticate("admin", "123qwe!@#QWE");
                    AppState.UserInfo = await UserInfoService.SelectAsync(UserID);
                    AppState.SetParamAsInteger("USER_ID", UserID);
                    AppState.CRUD = "UPDATE";
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
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
            TooltipOptions options = new TooltipOptions() { Duration = NotificationDuration };
            TooltipService.Open(elementReference, msg, options);
        }
        public void Dispose()
        {
        }
    }
}
