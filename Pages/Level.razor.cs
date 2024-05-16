﻿using Mentor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace Mentor.Pages
{
    public partial class Level
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

        private LevelModel LevelObject { get; set; } = new LevelModel();

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
                    int levelID = AppState.GetParamAsInteger("LEVEL_ID", 0);
                    LevelObject = await LevelService.SelectAsync(levelID);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task LevelSave()
        {
            try
            {
                if (LevelObject.IsNotNull())
                {
                    var retval = await LevelService.UpdateAsync(LevelObject);

                    if (retval == 1)
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "User", Detail = "Saved", Duration = NotificationDuration, Style = NotificationPosition });
                        DisableSave = true;
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

        private async Task LevelDelete()
        {
            try
            {
                await LevelService.DeleteAsync(LevelObject.LEVEL_ID);
                DisableSave = true;
                NavigationManager.NavigateTo("/settings");
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void OnChange()
        {
            DisableSave = false;
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
