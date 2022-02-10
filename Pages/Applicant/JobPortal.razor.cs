using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;

namespace XebecPortal.UI.Pages.Applicant
{
    public partial class JobPortal
    {
        private string searchJob;
        private bool IsApplyHidden;
        private List<int> pageNum = new List<int>();
        private IList<Job> jobList = new List<Job>();
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();
        private IList<Application> applicationList = new List<Application>();
        private List<JobType> JobTypes;

        protected override async Task OnInitializedAsync()
        {
            JobTypes = await httpClient.GetFromJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/JobType");
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            applicationList = await httpClient.GetFromJsonAsync<List<Application>>("https://xebecapi.azurewebsites.net/api/Application");
            
            
            jobListFilter = jobList;
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            displayJobDetail = jobListFilter.FirstOrDefault();
            DisplayJobDetail(displayJobDetail.Id);
        }

        private async Task Apply(int id)
        {
            Application application = new Application();

            application.TimeApplied = DateTime.Today;
            application.BeginApplication = DateTime.Today;
            application.JobId = id;
            application.AppUserId = 1;

            _ = await httpClient.PostAsJsonAsync("https://xebecapi.azurewebsites.net/api/ApplicationModel", application);
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            applicationList = await httpClient.GetFromJsonAsync<List<Application>>("https://xebecapi.azurewebsites.net/api/ApplicationModel");

            IsApplyHidden = true;
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
            if (applicationList.Count(x => x.AppUserId == 1 && x.JobId == id) > 0)
                IsApplyHidden = true;
            else
                IsApplyHidden = false;

            displayJobDetail = jobListFilter.FirstOrDefault(x => x.Id == id);
        }
    }
}
