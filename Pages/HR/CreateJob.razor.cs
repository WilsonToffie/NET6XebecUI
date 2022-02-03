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
    public partial class CreateJob
    {
        private List<Developer> developers = new List<Developer>();
        private List<Developer> developersAdded = new List<Developer>();
        private IList<MockDepartment> mockDepartments = new List<MockDepartment>();
        private IList<MockLocation> mockLocations = new List<MockLocation>();
        private IList<JobType> jobTypes = new List<JobType>();
        private IList<JobPlatform> jobPlatforms = new List<JobPlatform>();
        private JobType tempJobType = new JobType();
        private List<JobPlatform> ListOfPlatforms = new List<JobPlatform>();
        private bool isChecked = false;
        private CreateJobPost tempJob = new CreateJobPost();


        protected override async Task OnInitializedAsync()
        {
            mockDepartments = await HttpClient.GetFromJsonAsync<List<MockDepartment>>("/mockData/departmentMockData.json");
            mockLocations = await HttpClient.GetFromJsonAsync<List<MockLocation>>("/mockData/locationMockData.json");
            jobPlatforms = await HttpClient.GetFromJsonAsync<List<JobPlatform>>("https://xebecapi.azurewebsites.net/api/jobplatform");
            developers = await HttpClient.GetFromJsonAsync<List<Developer>>("https://my-json-server.typicode.com/WilsonToffie/JSONTesting/Developers");
            jobTypes = await HttpClient.GetFromJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/jobtype");
        }

        private void AddCollaborator(Developer developer)
        {
            if (!developersAdded.Contains(developer))
            {
                developersAdded.Add(developer);
            }
        }

        private void RemoveCollaborator(Developer developer)
        {
            developersAdded.Remove(developer);
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("CreateJob");
            return base.OnAfterRenderAsync(firstRender);
        }

        private void pageRedirect(Job job)
        {
            CreateNewJob(job);

            nav.NavigateTo("/applicationformcontroltool");
        }

        private void CreateNewJob(Job job)
        {

        }

        private void AddPlatfrom(JobPlatform platform, bool checkedPlatform)
        {
            if (checkedPlatform)
            {
                ListOfPlatforms.Remove(platform);
            }
            else
            {
                 ListOfPlatforms.Add(platform);   
            }
        }

        private bool ToggleChecked()
        {
            return isChecked = !isChecked;
        }
    }
}
