using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Mentor.Data;
using System.Diagnostics;
using Mentor.Shared;

namespace Mentor.Pages
{
    public partial class Login
    {
        private AppState AppState { get; set; } = new AppState();
        private string Msg { get; set; }
        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }
        private bool IsRendered { get; set; } = false;

        private string UserName = string.Empty;
        private string UserPass = string.Empty;

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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private void OnChange(string value, string key)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private async Task OnClick(string key)
        {
            try
            {
                if (key == "LOGIN")
                {
                    if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserPass))
                    {
                        Msg = "Username and Password cannot be empty. Please try again.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = Msg });
                        return;
                    }

                    int UserID = UserService.Authenticate(UserName, UserPass);

                    if (UserID > 0)
                    {
                        UserModel UserObject = await UserService.SelectAsync(UserID);
                        AppState.UserInfo = UserObject;
                        if (UserObject.GROUP_ID == 1)
                        {
                            AppState.IsAdmin = true;
                        }
                        await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);

                        Msg = UserName + " logged in.";
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Authentication", Detail = Msg });

                        NavigationManager.NavigateTo("./");
                    }
                    else
                    {
                        AppState.UserInfo = new();

                        switch (UserID)
                        {
                            case 0:
                                Msg = "Incorrect password.";
                                break;
                            case -1:
                                Msg = "Login failed. User [" + UserName + "] does not exist.";
                                break;
                            case -2:
                                Msg = "Cannot login. User [" + UserName + "] is disabled.";
                                break;
                            default:
                                Msg = "Login failed.";
                                break;
                        }

                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Authentication", Detail = Msg });
                    }
                }

                if (key == "LOGOUT")
                {
                    AppState.UserInfo = new();
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
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

        public void Dispose()
        {
        }
    }
}
