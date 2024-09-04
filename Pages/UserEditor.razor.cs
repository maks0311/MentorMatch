using Mentor.Data;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace Mentor.Pages
{
    public partial class UserEditor
    {
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;
        private bool DisableSave { get; set; } = true;
        private bool DisableDelete { get; set; } = false;

        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private readonly int ColumnLabelSize = 5;
        private readonly int ColumnControlSize = 7;

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
                if (AppState.IsNotNull())
                {
                    int userID = AppState.GetParamAsInteger("USER_ID", 0);
                    UserObject = await UserService.SelectAsync(userID);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
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
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "User saved" });
                        DisableSave = true;
                        DialogService.Close();
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "User not saved", Detail = "Something went wrong. Try again." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private async Task UserDelete()
        {
            try
            {
                var retval = await UserService.DeleteAsync(UserObject.USER_ID);
                if (retval == 1)
                {
                    DisableSave = true;
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "User deleted" });
                    DialogService.Close();
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "User not deleted", Detail = "Something went wrong. Try again." });
                    DialogService.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
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

        public void Dispose()
        {
        }
    }
}
