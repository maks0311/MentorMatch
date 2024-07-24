using Mentor.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Mentor.Pages
{
    public partial class Search
    {
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;
        string NotificationPosition { get { return AppConfig.GetSection("PopUpNotifications").GetValue<string>("Position"); } }
        int NotificationDuration { get { return AppConfig.GetSection("PopUpNotifications").GetValue<int>("Duration"); } }

        private WorkTypeModel WorkTypeObject { get; set; } = new WorkTypeModel();
        private RatingModel RatingObject { get; set; } = new RatingModel();

        private string SearchTutorName { get; set; } = string.Empty;
        private SubjectModel SearchSubjectObject { get; set; } = new SubjectModel();
        private LevelModel SearchLevelObject { get; set; } = new LevelModel();
        private WorkTypeModel SearchWorkTypeObject { get; set; } = new WorkTypeModel();
        private CityModel SearchCityObject { get; set; } = new CityModel();

        private IEnumerable<TutorModel> TutorEnum { get; set; }
        private IEnumerable<LevelModel> LevelEnum { get; set; }
        private IEnumerable<SubjectModel> SubjectEnum { get; set; }
        private IEnumerable<WorkTypeModel> WorkTypeEnum { get; set; }
        private IEnumerable<WorkTypeModel> WorkTypeDropdownEnum { get; set; }
        private IEnumerable<RatingModel> RatingEnum { get; set; }
        private IEnumerable<CityModel> CityEnum { get; set; }

        private bool CityDisabled
        {
            get
            {
                if (SearchWorkTypeObject.WORK_TYPE_ID == 2)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

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
                    await InitializeData();

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

        private async Task InitializeData()
        {
            try
            {
                TutorEnum = await TutorService.SearchAllAsync(string.Empty, 0, 0, 0, 0);
                LevelEnum = await LevelService.SelectAllAsync();
                SubjectEnum = await SubjectService.SelectAllAsync();
                WorkTypeEnum = await WorkTypeService.SelectAllAsync();
                WorkTypeDropdownEnum = WorkTypeEnum.Where(args => args.WORK_TYPE_ID != 3);
                RatingEnum = await RatingService.SelectAllAsync();
                CityEnum = await CityService.SelectAllAsync();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        private async Task SearchTutors()
        {
            try
            {
                TutorEnum = await TutorService.SearchAllAsync(SearchTutorName, SearchSubjectObject.SUBJECT_ID, SearchLevelObject.LEVEL_ID, SearchWorkTypeObject.WORK_TYPE_ID, SearchCityObject.CITY_ID);
                if (TutorEnum.Any())
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Tutors found", Detail = "We found " + TutorEnum.Count() + " tutor(s) for you" });
                }
                else
                {
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "No tutors found", Detail = "Change criteria to find your tutor." });
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }

        async Task TutorView(int tutorID)
        {
            try
            {
                AppState.SetParamAsInteger("TUTOR_ID", tutorID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                NavigationManager.NavigateTo("/tutor");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
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
                EventLog.WriteEntry("Mentor", ex.Message, EventLogEntryType.Error);
            }
        }
    }
}
