using Mentor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Mentor.Pages
{
    public partial class Settings
    {
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;

        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();
        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private IEnumerable<UserModel> UserEnum { get; set; }
        private IEnumerable<LevelModel> LevelEnum { get; set; }
        private IEnumerable<SubjectModel> SubjectEnum { get; set; }
        private IEnumerable<CityModel> CityEnum { get; set; }

        private RadzenDataGrid<UserModel> UserDataGrid { get; set; } = new RadzenDataGrid<UserModel>();
        private RadzenDataGrid<SubjectModel> SubjectDataGrid { get; set; } = new RadzenDataGrid<SubjectModel>();
        private RadzenDataGrid<LevelModel> LevelDataGrid { get; set; } = new RadzenDataGrid<LevelModel>();
        private RadzenDataGrid<CityModel> CityDataGrid { get; set; } = new RadzenDataGrid<CityModel>();

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
                    UserEnum = await UserService.SelectAllAsync();
                    SubjectEnum = await SubjectService.SelectAllAsync();
                    LevelEnum = await LevelService.SelectAllAsync();
                    CityEnum = await CityService.SelectAllAsync();
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async void UserEdit(int userID)
        {
            try {
                AppState.SetParamAsInteger("USER_ID", userID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                NavigationManager.NavigateTo("/user_editor");
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void SubjectAdd()
        {
            NavigationManager.NavigateTo("/subject");
        }

        private async void SubjectEdit(int subjectID)
        {
            try
            {
                AppState.SetParamAsInteger("SUBJECT_ID", subjectID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                NavigationManager.NavigateTo("/subject");
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void LevelAdd()
        {
            NavigationManager.NavigateTo("/level");
        }

        private async void LevelEdit(int levelID)
        {
            try
            {
                AppState.SetParamAsInteger("LEVEL_ID", levelID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                NavigationManager.NavigateTo("/level");
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CityAdd()
        {
            NavigationManager.NavigateTo("/city");
        }

        private async void CityEdit(int cityID)
        {
            try
            {
                AppState.SetParamAsInteger("CITY_ID", cityID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                NavigationManager.NavigateTo("/city");
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
