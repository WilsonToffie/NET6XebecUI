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

        private List<int> pageNum = new List<int>();
        private IList<Job> jobList = new List<Job>();
        private IList<Job> jobListFilter = new List<Job>();
        private Job displayJobDetail = new Job();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();

        protected override async Task OnInitializedAsync()
        {
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("https://xebecapi.azurewebsites.net/api/Job");
            jobListFilter = jobList;
            jobPagedList = jobListFilter.ToPagedList(1, 17);
            pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
            displayJobDetail = jobListFilter.FirstOrDefault();
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
