using Mentor.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Mentor.Pages
{
    public partial class Search
    {
        AppState AppState { get; set; } = new AppState();
        private bool IsRendered { get; set; } = false;
        private static readonly NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

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
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private async Task SearchTutors()
        {
            TutorEnum = await TutorService.SearchAllAsync(SearchTutorName, SearchSubjectObject.SUBJECT_ID, SearchLevelObject.LEVEL_ID, SearchWorkTypeObject.WORK_TYPE_ID, SearchCityObject.CITY_ID);
        }

        private void OnFilterChange(string key)
        {
            if (key == "WORK_TYPE")
            {
                TutorEnum = TutorEnum.Where(tutor => tutor.WORK_TYPE_ID == WorkTypeObject.WORK_TYPE_ID);
            }

            if (key == "RATING")
            {
                TutorEnum = TutorEnum.Where(tutor => tutor.TUTOR_RATING == RatingObject.RATING_VALUE);
            }
        }

        async Task TutorView(int TutorID)
        {
            try
            {
                AppState.SetParamAsInteger("TUTOR_ID", TutorID);
                await SessionStorage.SetItemAsync<AppState>("APP_STATE", AppState);
                NavigationManager.NavigateTo("/tutor");
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
