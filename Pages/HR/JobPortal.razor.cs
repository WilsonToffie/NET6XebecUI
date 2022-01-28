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
        private IList<MockDepartment> mockDepartments = new List<MockDepartment>();
        private IList<MockLocation> mockLocations = new List<MockLocation>();
        private IList<MockSocialMedia> mockSocialMedia = new List<MockSocialMedia>();


        protected override async Task OnInitializedAsync()
        {
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            mockDepartments = await httpClient.GetFromJsonAsync<List<MockDepartment>>("/mockData/departmentMockData.json");
            mockLocations = await httpClient.GetFromJsonAsync<List<MockLocation>>("/mockData/locationMockData.json");
            mockSocialMedia = await httpClient.GetFromJsonAsync<List<MockSocialMedia>>("/mockData/socialmediaMockData.json");
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
            pageNum.Clear();

            if (value != null && value != "" && value != " ")
            {
                jobListFilter = jobList.Where(x => $"{x.Title} {x.Company} {x.Location}".Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();
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
        }
    }
}