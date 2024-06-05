using Mentor.Data;
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

        private readonly int ColumnLabelSize = 5;
        private readonly int ColumnControlSize = 7;

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
                    if (levelID.IsPositive())
                    {
                        LevelObject = await LevelService.SelectAsync(levelID);
                    }
                    else
                    {
                        DisableDelete = true;
                    }
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
                    var retval = 0;

                    if (LevelObject.LEVEL_ID.IsPositive())
                    {
                        retval = await LevelService.UpdateAsync(LevelObject);
                    }
                    else
                    {
                        retval = await LevelService.CreateAsync(LevelObject);
                    }

                    if (retval.IsPositive())
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Level saved" });
                        DisableSave = true;
                        AppState.SetParamAsInteger("LEVEL_ID", 0);
                        await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                        DialogService.Close();
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Level not saved", Detail = "Something went wrong. Try again." });
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
                var retval = await LevelService.DeleteAsync(LevelObject.LEVEL_ID);
                if(retval == 1)
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Level deleted" });
                    DisableSave = true;
                    AppState.SetParamAsInteger("LEVEL_ID", 0);
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                    DialogService.Close();
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Level not deleted", Detail = "Something went wrong. Try again." });

                }
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
                message.Style = NotificationPosition;
                message.Duration = NotificationDuration;
                NotificationService.Notify(message);
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        public async void Dispose()
        {

        }
    }
}
