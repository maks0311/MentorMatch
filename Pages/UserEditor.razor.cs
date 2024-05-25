using Mentor.Data;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace Mentor.Pages
{
    public partial class UserEditor
    {
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;
        private static readonly NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();
        private bool DisableSave { get; set; } = true;
        private bool DisableDelete { get; set; } = false;

        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private readonly int ColumnLabelSize = 2;
        private readonly int ColumnControlSize = 10;

        private UserModel UserObject { get; set; } = new UserModel();

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
                if (AppState.IsNotNull())
                {
                    int userID = AppState.GetParamAsInteger("USER_ID", 0);
                    UserObject = await UserService.SelectAsync(userID);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task UserSave()
        {
            try
            {
                if (UserObject.IsNotNull())
                {
                    var retval = await UserService.UpdateAsync(UserObject);

                    if (retval.IsPositive())
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "User", Detail = "Saved", Duration = NotificationDuration, Style = NotificationPosition });
                        DisableSave = true;
                        NavigationManager.NavigateTo("/settings");
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "User", Detail = "Not Saved", Duration = NotificationDuration, Style = NotificationPosition });
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task UserDelete()
        {
            try
            {
                await UserService.DeleteAsync(UserObject.USER_ID);
                DisableSave = true;
                NavigationManager.NavigateTo("/settings");
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnCheckBoxChange()
        {
            DisableSave = false;
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

        public void Dispose()
        {
        }
    }
}
