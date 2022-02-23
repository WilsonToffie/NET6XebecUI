using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;
using System.Text.Json;

namespace XebecPortal.UI.Pages.HR
{
    public partial class JobPortal
    {
        private bool changeForm;
        private string searchJob;
     
        private List<int> pageNum = new List<int>();
        private IList<Job> jobList = new List<Job>();
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();
        private List<string> Departments = new List<string>() { "Accounting & Finance", "HR", "Sales & Marketing", "Legal", "Research & Development", "IT", "Admin", "Customer Support" };
        private List<string> Locations = new List<string>() { "Eastern Cape", "Free State", " Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northen Cape", "North West", "Western Cape" };
        private IList<JobPlatform> jobPlatforms = new List<JobPlatform>();
        private IList<JobPlatformHelper> jobPlatformHelpers = new List<JobPlatformHelper>();
        private List<JobPlatform> platformsUsed = new List<JobPlatform>();
        private List<JobType> JobTypes;

        protected override async Task OnInitializedAsync()
        {
            JobTypes = await httpClient.GetFromJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            jobPlatforms = await httpClient.GetFromJsonAsync<List<JobPlatform>>("https://xebecapi.azurewebsites.net/api/jobplatform");
            jobPlatformHelpers = await httpClient.GetFromJsonAsync<List<JobPlatformHelper>>("https://xebecapi.azurewebsites.net/api/jobplatformhelper");
            jobListFilter = jobList;
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            displayJobDetail = jobListFilter.FirstOrDefault();
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
            jobPagedList = jobList.ToPagedList(value, 17);
        }

        private void SeachListJob(string value)
        {
            if (value != null && value != "" && value != " ")
            {
                jobListFilter = jobList.Where(x => $"{x.Title} {x.Company} {x.Department}".Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
                jobPagedList = jobListFilter.ToPagedList(1, 17);
                pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            }
            else
            {
                jobListFilter = jobList;
                jobPagedList = jobListFilter.ToPagedList(1, 17);
                pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            }
            displayJobDetail = jobListFilter.FirstOrDefault();
        }

        private void DisplayJobDetail(int id)
        {
            displayJobDetail = jobListFilter.FirstOrDefault(x => x.Id == id);
            platformsUsed.Clear();
        }

        private async Task RemovePlatform(JobPlatform platform)
        {
            int tempId = platform.id;
            await httpClient.DeleteAsync($"https://xebecapi.azurewebsites.net/api/jobplatformhelper/{tempId}");
        }

        private async Task AddPlatform(JobPlatform platform)
        {
            await httpClient.PutAsJsonAsync($"https://xebecapi.azurewebsites.net/api/jobplatformhelper",platform);
        }
    }
}