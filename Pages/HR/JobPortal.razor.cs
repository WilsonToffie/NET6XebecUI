using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using X.PagedList;

namespace XebecPortal.UI.Pages.HR
{
    public partial class JobPortal
    {
        private bool changeForm;
        private List<int> pageNum = new List<int>();
        private List<Job> jobList = new List<Job>();
        private IPagedList<Job> jobPagedList = new List<Job>().ToPagedList();

        protected override async Task OnInitializedAsync()
        {
            jobList = await httpClient.GetFromJsonAsync<List<Job>>("/mockData/jobMockData.json");
            jobPagedList = jobList.ToPagedList(1, 17);
            pageNum.AddRange(Enumerable.Range(1, jobPagedList.PageCount));
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("HrJobPortalJs");
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

        private async Task DeleteData()
        {
            if (!await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Delete This Item?"))
                return;
        }

        private async Task SaveData()
        {
            if (!await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Override This Item?"))
                return;
        }

        private void PageListNav(int value)
        {
            jobPagedList = jobList.ToPagedList(value, 17);
        }
    }
}
