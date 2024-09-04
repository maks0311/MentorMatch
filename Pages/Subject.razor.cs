using Mentor.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace Mentor.Pages
{
    public partial class Subject
    {
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;
        private bool DisableSave { get; set; } = true;
        private bool DeleteVisible { get; set; } = true;

        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private readonly int ColumnLabelSize = 5;
        private readonly int ColumnControlSize = 7;

        private SubjectModel SubjectObject { get; set; } = new SubjectModel();

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
                    int subjectID = AppState.GetParamAsInteger("SUBJECT_ID", 0);
                    if (subjectID.IsPositive())
                    {
                        SubjectObject = await SubjectService.SelectAsync(subjectID);
                    }
                    else
                    {
                        DeleteVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private async Task SubjectSave()
        {
            try
            {
                if (SubjectObject.IsNotNull())
                {
                    var retval = 0;

                    if (SubjectObject.SUBJECT_ID.IsPositive())
                    {
                        retval = await SubjectService.UpdateAsync(SubjectObject);
                    }
                    else
                    {
                        retval = await SubjectService.CreateAsync(SubjectObject);
                    }

                    if (retval.IsPositive())
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Subject saved" });
                        DisableSave = true;
                        AppState.SetParamAsInteger("SUBJECT_ID", 0);
                        await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                        DialogService.Close();
                    }
                    else
                    {
                        ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Subject not saved", Detail = "Something went wrong. Try again." });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }

        private async Task SubjectDelete()
        {
            try
            {
                var retval = await SubjectService.DeleteAsync(SubjectObject.SUBJECT_ID);
                if (retval >= 1)
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Subject deleted" });
                    DisableSave = true;
                    AppState.SetParamAsInteger("SUBJECT_ID", 0);
                    await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                    DialogService.Close();
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Subject not deleted", Detail = "Something went wrong. Try again." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
        }
        public void Dispose()
        {

        }
    }
}
