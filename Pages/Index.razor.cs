using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace Mentor.Pages
{
    public partial class Index
    {
        AppState AppState { get; set; } = new AppState();

        static string Today { get { return DateTime.Now.ToString("D", new CultureInfo("en-EN")); } }
        private bool IsRendered { get; set; } = false;

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
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
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
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        public void Dispose()
        {
        }
    }
}
