using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;

namespace XebecPortal.UI.Pages.HR
{
    public partial class JobPortal
    {
        private bool changeForm;
        private string searchJob;
        private bool nextButton, preButton = true;
        private List<int> pageNum = new List<int>();
        private IList<Job> jobList = new List<Job>();
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();
        private IList<JobPlatform> jobPlatforms = new List<JobPlatform>();
        private IList<JobPlatformHelper> jobPlatformHelpers = new List<JobPlatformHelper>();
        private List<JobPlatform> platformsUsed = new List<JobPlatform>();
        private List<JobType> JobTypes;
        private List<Status> status;
        private List<string> Departments = new List<string>() { "Accounting & Finance", "HR", "Sales & Marketing", "Legal", "Research & Development", "IT", "Admin", "Customer Support" };
        private List<string> Locations = new List<string>() { "Eastern Cape", "Free State", " Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northen Cape", "North West", "Western Cape" };
        private bool JobPortalIsHidden = false;
        private bool ApplicantPortalIsHidden = true;

        private IEnumerable<string> mudSelectLocation;
        private IEnumerable<string> mudSelectCompany;
        private IEnumerable<string> mudSelectDepartment;
        private IEnumerable<string> mudSelectStatus;

        protected override async Task OnInitializedAsync()
        {
            await ShowJobPortal();
        }

        private async Task ShowJobPortal()
        {
            JobTypes = await httpClient.GetFromJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            jobPlatforms = await httpClient.GetFromJsonAsync<List<JobPlatform>>("https://xebecapi.azurewebsites.net/api/jobplatform");
            jobPlatformHelpers = await httpClient.GetFromJsonAsync<List<JobPlatformHelper>>("https://xebecapi.azurewebsites.net/api/jobplatformhelper");
            status = await httpClient.GetFromJsonAsync<List<Status>>("/mockData/Status.json");

            jobListFilter = jobList;
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            displayJobDetail = jobListFilter.FirstOrDefault();
            DisplayJobDetail(displayJobDetail.Id);

            JobPortalIsHidden = false;
            ApplicantPortalIsHidden = true;
        }

        private void ShowApplicantPortal(int jobId)
        {
            hrJobState.JobId = jobId;

            JobPortalIsHidden = true;
            ApplicantPortalIsHidden = false;
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("HrJobPortalJS");
            return base.OnAfterRenderAsync(firstRender);
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
                await httpClient.DeleteAsync($"https://xebecapi.azurewebsites.net/api/Job/{id}");
                jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
                jobListFilter = jobList;
                pageNum.Clear();
                jobPagedList = jobListFilter.ToPagedList(1, 17);
                displayJobDetail = jobListFilter.FirstOrDefault();
                pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            }
        }

        private async Task SaveData(Job value)
        {
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Override This Item?"))
            {
                var itemSearch = jobList.First(x => x.Id == value.Id);
                var index = jobList.IndexOf(itemSearch);
                jobList[index] = value;
            }
        }

        private void PageListNav(int value)
        {
            jobPagedList = jobListFilter.ToPagedList(value, 17);
            nextButton = value == jobPagedList.PageCount || jobPagedList.PageCount == 1;
            preButton = value == 1;
        }

        private void SearchListJob(ChangeEventArgs e)
        {
            searchJob = e.Value.ToString();
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }

        private void SearchListLocation(IEnumerable<string> value)
        {
            mudSelectLocation = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }

        private void SearchListCompany(IEnumerable<string> value)
        {
            mudSelectCompany = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }

        private void SearchListDepartment(IEnumerable<string> value)
        {
            mudSelectDepartment = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
        }

        private void SearchListStatus(IEnumerable<string> value)
        {
            mudSelectStatus = value;
            jobListFilter = jobList;
            FilterDataHelper();
            FilterDataDisplayHelper();
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

        private static string GetMultiSelectionTextDepartment(List<string> selectedValues)
        {
            return $"Selected Department{(selectedValues.Count > 1 ? "s" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private static string GetMultiSelectionTextStatus(List<string> selectedValues)
        {
            return $"Selected Status{(selectedValues.Count > 1 ? "es" : " ")}: {string.Join(", ", selectedValues.Select(x => x))}";
        }

        private void FilterDataHelper()
        {
            if (!string.IsNullOrEmpty(searchJob) && searchJob != " ")
            {
                jobListFilter = jobListFilter.Where(x => x.Title.Contains(searchJob, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (mudSelectLocation?.Any() == true)
            {
                var listLocations = jobListFilter.Select(x => x.Location).Except(mudSelectLocation).ToList();
                jobListFilter = jobListFilter.Where(x => !listLocations.Contains(x.Location)).ToList();
            }

            if (mudSelectCompany?.Any() == true)
            {
                var listCompany = jobListFilter.Select(x => x.Company).Except(mudSelectCompany).ToList();
                jobListFilter = jobListFilter.Where(x => !listCompany.Contains(x.Company)).ToList();
            }

            if (mudSelectDepartment?.Any() == true)
            {
                var listDepartments = jobListFilter.Select(x => x.Department).Except(mudSelectDepartment).ToList();
                jobListFilter = jobListFilter.Where(x => !listDepartments.Contains(x.Department)).ToList();
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
            displayJobDetail = jobListFilter.FirstOrDefault();
        }

        private object GetStyling(Job item)
        {
            if (displayJobDetail.Id == item.Id)
                return "box-shadow: rgba(0,51,64,0.86) 0px 0px 0px 2px, rgba(6, 24, 44, 0.65) 0px 4px 6px -1px, rgba(255, 255, 255, 0.08) 0px 1px 0px inset;";
            return "";
        }

        private string GetJobType(JobTypeHelper helper)
        {
            var type = new JobType { Id = 0, Type = "Contract" };

            if (JobTypes != null && helper != null)
            {
                type = JobTypes.FirstOrDefault(e => e.Id == helper.Id);
            }
            return ""; //type.Type;
        }
    }
}