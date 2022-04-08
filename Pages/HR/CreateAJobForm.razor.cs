using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using X.PagedList;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using XebecPortal.UI.Utils.Handlers;

namespace XebecPortal.UI.Pages.HR
{
    public partial class CreateAJobForm
    {
        [Parameter]
        public CreateJobPost TempJob { get; set; }
        [Parameter]
        public EventCallback<CreateJobPost> TempJobChanged { get; set; }
        private List<AppUser> collaborators = new List<AppUser>();
        private List<AppUser> collaboratorsAdded = new List<AppUser>();
        private List<string> Departments = new List<string>() { "Accounting & Finance", "HR", "Sales & Marketing", "Legal", "Research & Development", "IT", "Admin", "Customer Support"};
        private List<string> Locations = new List<string>() { "Eastern Cape","Free State"," Gauteng","KwaZulu-Natal","Limpopo","Mpumalanga","Northen Cape","North West","Western Cape"} ;
        private IList<JobType> jobTypes = new List<JobType>();
        private IList<JobPlatform> jobPlatforms = new List<JobPlatform>();
        private JobType tempJobType = new JobType();
        private List<JobPlatform> ListOfPlatforms = new List<JobPlatform>();


        string token;
        protected override async Task OnInitializedAsync()
        {
            token = await localStorage.GetItemAsync<string>("jwt_token");

            jobPlatforms = await HttpClient.GetListJsonAsync<List<JobPlatform>>("https://xebecapi.azurewebsites.net/api/jobplatform", new AuthenticationHeaderValue("Bearer", token));
            jobTypes = await HttpClient.GetListJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/jobtype", new AuthenticationHeaderValue("Bearer", token));
            collaborators = await HttpClient.GetListJsonAsync<List<AppUser>>("https://xebecapi.azurewebsites.net/api/user", new AuthenticationHeaderValue("Bearer", token));
            TempJob.DueDate = TempJob.CreationDate = DateTime.Today;
        }


        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            jsRuntime.InvokeVoidAsync("CreateJob");
            return base.OnAfterRenderAsync(firstRender);
        }

        private void AddPlatfrom(JobPlatform platform)
        {

            if (!ListOfPlatforms.Contains(platform))
            {
                ListOfPlatforms.Add(platform);
            }
            else
            {
                ListOfPlatforms.Remove(platform);
            }
        }
    }
}
