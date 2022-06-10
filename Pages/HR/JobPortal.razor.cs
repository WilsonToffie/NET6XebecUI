using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;
using XebecPortal.UI.Utils.Handlers;
namespace XebecPortal.UI.Pages.HR
{
    public partial class JobPortal
    {
        private bool changeForm;
        private bool isDialogVisible;
        private string searchJob;
        private string token;
        private bool nextButton, preButton = true;
        private bool isFilterContainAnyVal;
        private List<int> pageNum = new List<int>();
        private IList<Job> jobList = new List<Job>();
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();
        private IList<JobPlatform> jobPlatforms = new List<JobPlatform>();
        private IList<JobPlatformHelper> jobPlatformHelpers = new List<JobPlatformHelper>();
        private List<JobPlatform> platformsUsed = new List<JobPlatform>();
        private List<JobType> JobTypes;
        private List<JobTypeHelper> jobTypeHelper;
        private List<Status> status;
        private List<AppUser> appUser;
        private List<AppUser> appUserFilter;
        private List<CollaboratorsAssigned> collaboratorsAssigned;
        private List<CollaboratorsAssigned> collaboratorsAssigned2;
        private List<PersonalInformation> personalInformation;
        private List<Department> departments;
        private List<string> locations = new() { "Eastern Cape", "Free State", " Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northen Cape", "North West", "Western Cape" };
        private MudBlazor.DialogOptions options = new() { CloseButton = true, FullWidth = true};

        private IEnumerable<string> mudSelectLocation;
        private IEnumerable<string> mudSelectCompany;
        private IEnumerable<int> mudSelectDepartment;
        private IEnumerable<string> mudSelectStatus;

        private bool ShowingJobPortal = true;
        private bool ShowingApplicantPortal;
        private bool ShowingPhaseManager;

        private IJSObjectReference _jsModule;
        private string defaultCollaboratorImage = "https://xebecstorage.blob.core.windows.net/profile-images/0";
        protected override async Task OnInitializedAsync()
        {
            token = await localStorage.GetItemAsync<string>("jwt_token");

            ShowJobPortal();
            
            JobTypes = await httpClient.GetListJsonAsync<List<JobType>>($"https://xebecapi.azurewebsites.net/api/JobType", new AuthenticationHeaderValue("Bearer", token));
            jobList = await httpClient.GetListJsonAsync<List<Job>>($"https://xebecapi.azurewebsites.net/api/Job", new AuthenticationHeaderValue("Bearer", token));
            jobPlatforms = await httpClient.GetListJsonAsync<List<JobPlatform>>($"https://xebecapi.azurewebsites.net/api/jobplatform", new AuthenticationHeaderValue("Bearer", token));
            jobPlatformHelpers = await httpClient.GetListJsonAsync<List<JobPlatformHelper>>($"https://xebecapi.azurewebsites.net/api/jobplatformhelper", new AuthenticationHeaderValue("Bearer", token));
            jobTypeHelper = await httpClient.GetListJsonAsync<List<JobTypeHelper>>($"https://xebecapi.azurewebsites.net/api/JobTypeHelper", new AuthenticationHeaderValue("Bearer", token));
            appUser = await httpClient.GetListJsonAsync<List<AppUser>>($"https://xebecapi.azurewebsites.net/api/User", new AuthenticationHeaderValue("Bearer", token));
            collaboratorsAssigned = await httpClient.GetListJsonAsync<List<CollaboratorsAssigned>>($"https://xebecapi.azurewebsites.net/api/CollaboratorsAssigned", new AuthenticationHeaderValue("Bearer", token));
            personalInformation = await httpClient.GetListJsonAsync<List<PersonalInformation>>("https://xebecapi.azurewebsites.net/api/PersonalInformation", new AuthenticationHeaderValue("Bearer", token));


            status = await httpClient.GetFromJsonAsync<List<Status>>("/mockData/Status.json");
            departments = await httpClient.GetFromJsonAsync<List<Department>>("/mockData/departmentMockDatav1.json");
            _jsModule = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "/jsPages/HR/JobPortalv3.js");

            jobListFilter = jobList;
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            displayJobDetail = jobListFilter.FirstOrDefault();
            DisplayJobDetail(displayJobDetail.Id);
            OpenJobCollabToolTip(displayJobDetail.Id);


        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("HrJobPortalJS");
            _jsModule.InvokeVoidAsync("ResizeTextArea");

            return base.OnAfterRenderAsync(firstRender);
        }

        private void ShowApplicantPortal(int id)
        {
            hrJobState.JobId = id;
            ShowingApplicantPortal = true;
            ShowingJobPortal = false;
            ShowingPhaseManager = false;
        }

        private void ShowPhaseManager()
        {
            ShowingApplicantPortal = false;
            ShowingJobPortal = false;
            ShowingPhaseManager = true;
        }

        private void ShowJobPortal()
        {
            ShowingApplicantPortal = false;
            ShowingJobPortal = true;
            ShowingPhaseManager = false;
        }

        private void pageRedirect()
        {
            nav.NavigateTo("/createjob");
        }

        private void FormType(bool value)
        {
            changeForm = value;
        }

        private async Task DeleteData(int id)
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Delete This Item?"))
            {
                await httpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/Job/{id}", new AuthenticationHeaderValue("Bearer", token));
                jobList = await httpClient.GetListJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job", new AuthenticationHeaderValue("Bearer", token));
                jobListFilter = jobList;
                pageNum.Clear();
                jobPagedList = jobListFilter.ToPagedList(1, 17);
                displayJobDetail = jobListFilter.FirstOrDefault();
                pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            }
        }

        private async Task SaveData(Job jobValue, string jobTypeHelperValue, bool boolValue)
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Override This Item?"))
            {
                await _jsModule.InvokeVoidAsync("CursorWait");
                var newJobTypeHelper = jobTypeHelper.Find(x => x.JobId == jobValue.Id);
                newJobTypeHelper.JobTypeId = JobTypes.Find(x => x.Type == jobTypeHelperValue).Id;
                await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Job/{jobValue.Id}", jobValue, new AuthenticationHeaderValue("Bearer", token));
                await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/JobTypeHelper/{newJobTypeHelper.Id}", newJobTypeHelper, new AuthenticationHeaderValue("Bearer", token));
                changeForm = boolValue;
                await _jsModule.InvokeVoidAsync("CursorDefault");
            }
        }

        private async Task PageListNav(int value)
        {
            jobPagedList = jobListFilter.ToPagedList(value, 17);
            nextButton = value == jobPagedList.PageCount || jobPagedList.PageCount == 1;
            preButton = value == 1;
            await _jsModule.InvokeVoidAsync("Scroll");
            DisplayJobDetail(jobPagedList.FirstOrDefault().Id);
        }

        private async Task SearchListJob(ChangeEventArgs e)
        {
            searchJob = e.Value.ToString();
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private async Task SearchListLocation(IEnumerable<string> value)
        {
            mudSelectLocation = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private async Task SearchListCompany(IEnumerable<string> value)
        {
            mudSelectCompany = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private async Task SearchListDepartment(IEnumerable<int> value)
        {
            mudSelectDepartment = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private async Task SearchListStatus(IEnumerable<string> value)
        {
            mudSelectStatus = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
            await _jsModule.InvokeVoidAsync("Scroll");
        }

        private void DisplayJobDetail(int id)
        {
            hrJobState.JobId = id;
            displayJobDetail = jobListFilter.FirstOrDefault(x => x.Id == id);
            platformsUsed.Clear();
        }

        private static string GetMultiSelectionTextLocation(List<string> selectedValues)
        {
            return $"Selected Location{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextCompany(List<string> selectedValues)
        {
            return $"Selected Compan{(selectedValues.Count > 1 ? "ies" : "y")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private string GetMultiSelectionTextDepartment(List<string> selectedValues)
        {
            return $"Selected Department{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => departments.Find(y => y.Id == Convert.ToInt32(x)).Name))}";
        }

        private static string GetMultiSelectionTextStatus(List<string> selectedValues)
        {
            return $"Selected Status{(selectedValues.Count > 1 ? "es" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private void FilterDataHelper()
        {
            if (!string.IsNullOrEmpty(searchJob))
            {
                jobListFilter = jobListFilter.Where(x => x.Title.Contains(searchJob, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (mudSelectLocation?.Any() == true)
            {
                var listLocations = jobListFilter.Select(x => x.Location.Name).Except(mudSelectLocation).ToList();
                jobListFilter = jobListFilter.Where(x => !listLocations.Contains(x.Location.Name)).ToList();
            }

            if (mudSelectCompany?.Any() == true)
            {
                var listCompany = jobListFilter.Select(x => x.Company.Name).Except(mudSelectCompany).ToList();
                jobListFilter = jobListFilter.Where(x => !listCompany.Contains(x.Company.Name)).ToList();
            }

            if (mudSelectDepartment?.Any() == true)
            {
                var listDepartments = jobListFilter.Select(x => x.DepartmentId).Except(mudSelectDepartment).ToList();
                jobListFilter = jobListFilter.Where(x => !listDepartments.Contains(x.DepartmentId)).ToList();
            }

            if (mudSelectStatus?.Any() == true)
            {
                var listStatus = jobListFilter.Select(x => x.Status).Except(mudSelectStatus).ToList();
                jobListFilter = jobListFilter.Where(x => !listStatus.Contains(x.Status)).ToList();
            }
        }

        private void FilterDataDisplayHelper()
        {
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            nextButton = jobPagedList.PageNumber == jobPagedList.PageCount;
            preButton = jobPagedList.PageNumber == 1;
            jsRuntime.InvokeVoidAsync("");
            displayJobDetail = jobListFilter.FirstOrDefault();
        }

        private object GetStyling(Job item)
        {
            if (displayJobDetail.Id == item.Id)
                return "box-shadow: rgba(0,51,64,0.86) 0px 0px 0px 2px, rgba(6, 24, 44, 0.65) 0px 4px 6px -1px, rgba(255, 255, 255, 0.08) 0px 1px 0px inset;";
            return "";
        }

        private string GetJobType(int id)
        {
            int jobTypeId = jobTypeHelper.Find(x => x.JobId == id).JobTypeId;
            return JobTypes.Find(x => x.Id == jobTypeId).Type;
        }

        private void OpenJobCollabDialog(int id)
        {
            isDialogVisible = true;

            var collabId = collaboratorsAssigned.Where(x => x.JobId == id).Select(x => x.AppUserId);
            appUserFilter = appUser.Where(x => collabId.Contains(x.id) && x.role != "Candidate").ToList();
        }

        private void OpenJobCollabToolTip(int id)
        {
            var collabId = collaboratorsAssigned.Where(x => x.JobId == id).Select(x => x.AppUserId);
            appUserFilter = appUser.Where(x => collabId.Contains(x.id) && x.role != "Candidate").ToList();
        }
    }
}