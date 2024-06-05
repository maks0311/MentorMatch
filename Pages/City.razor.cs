using Mentor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace Mentor.Pages
{
    public partial class City
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

        private CityModel CityObject { get; set; } = new CityModel();

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
                    int cityID = AppState.GetParamAsInteger("CITY_ID", 0);
                    if (cityID.IsPositive())
                    {
                        CityObject = await CityService.SelectAsync(cityID);
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

        private async Task CitySave()
        {
            try
            {
                if (CityObject.IsNotNull())
                {
                    var retval = 0;

                    if (CityObject.CITY_ID.IsPositive())
                    {
                        retval = await CityService.UpdateAsync(CityObject);
                    }
                    else
                    {
                        retval = await CityService.CreateAsync(CityObject);
                    }

                    if (retval.IsPositive())
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "City saved"});
                        DisableSave = true;
                        AppState.SetParamAsInteger("CITY_ID", 0);
                        await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                        DialogService.Close();
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "City not saved", Detail = "Something went wrong. Try again."});
                    }
                }
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task CityDelete()
        {
            try
            {
                var retval = await CityService.DeleteAsync(CityObject.CITY_ID);
                if(retval == 1)
                {
                    DisableSave = true;
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "City deleted" });
                    AppState.SetParamAsInteger("CITY_ID", 0);
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                    DialogService.Close();
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "City not deleted", Detail = "Something went wrong. Try again." });

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

        public void Dispose()
        {
        }
    }
}
