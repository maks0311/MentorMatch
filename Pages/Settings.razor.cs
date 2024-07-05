using Mentor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Mentor.Pages
{
    public partial class Settings
    {
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;

        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        RadzenDataGrid<UserModel> UserGrid = new RadzenDataGrid<UserModel>();


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
                    await InitializeData();
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
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
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async Task UserEdit(int userID)
        {
            try
            {
                AppState.SetParamAsInteger("USER_ID", userID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                await DialogService.OpenAsync<UserEditor>("User Edit", new Dictionary<string, object> { });
                UserEnum = await UserService.SelectAllAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async void SubjectAdd()
        {
            try
            {
                await DialogService.OpenAsync<Subject>("Subject Add", new Dictionary<string, object> { });
                SubjectEnum = await SubjectService.SelectAllAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async void SubjectEdit(int subjectID)
        {
            try
            {
                AppState.SetParamAsInteger("SUBJECT_ID", subjectID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                await DialogService.OpenAsync<Subject>("Subject Edit", new Dictionary<string, object> { });
                SubjectEnum = await SubjectService.SelectAllAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async void LevelAdd()
        {
            try
            {
                await DialogService.OpenAsync<Level>("Level Add", new Dictionary<string, object> { });
                LevelEnum = await LevelService.SelectAllAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async void LevelEdit(int levelID)
        {
            try
            {
                AppState.SetParamAsInteger("LEVEL_ID", levelID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                await DialogService.OpenAsync<Level>("Level Edit", new Dictionary<string, object> { });
                LevelEnum = await LevelService.SelectAllAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async void CityAdd()
        {
            try
            {
                await DialogService.OpenAsync<City>("City Add", new Dictionary<string, object> { });
                CityEnum = await CityService.SelectAllAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async void CityEdit(int cityID)
        {
            try
            {
                AppState.SetParamAsInteger("CITY_ID", cityID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                await DialogService.OpenAsync<City>("City Edit", new Dictionary<string, object> { });
                CityEnum = await CityService.SelectAllAsync();
                StateHasChanged();
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
