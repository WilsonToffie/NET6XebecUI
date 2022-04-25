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
        private List<string> Departments = new List<string>() { "Accounting & Finance", "HR", "Sales & Marketing", "Legal", "Research & Development", "IT", "Admin", "SSS"};
        private List<string> Company = new List<string>() { "Nebula", "Deloitte"};
        private List<string> Locations = new List<string>() { "Remote", "Eastern Cape","Free State"," Gauteng","KwaZulu-Natal","Limpopo","Mpumalanga","Northen Cape","North West","Western Cape"} ;
        private List<string> Statuses = new List<string>() { "Open", "Draft", "Filled", "Closed"};
        private List<string> Policies = new List<string>() { "Remote", "Hybrid", "Onsite" };
        private IList<JobType> jobTypes = new List<JobType>();
        private IList<JobPlatform> jobPlatforms = new List<JobPlatform>();
        private JobType tempJobType = new JobType();
        private List<JobPlatform> ListOfPlatforms = new List<JobPlatform>();
        private List<CreateJobPost> jobList = new List<CreateJobPost>();

        string token;
        private bool manageDep;
        private bool manageComp;
        private bool createNewDep;
        private bool deleteDep;
        private bool createNewComp;
        private bool deleteComp;
        private string departmentToAdd;
        private string companyToAdd;

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

        private void createDep(bool value)
        {
            createNewDep = value;
        }

        private void deleteDepartment(bool value)
        {
            deleteDep = value;
        }

        private void createComp(bool value)
        {
            createNewComp = value;
        }

        private void deleteCompany(bool value)
        {
            deleteComp = value;
        }
        private void addDepartment(string value)
        {
            if (!value.Equals(string.Empty))
            {
                Departments.Add(value);
                departmentToAdd = string.Empty;
            }            
        }

        private void removeDepartment(string value)
        {
            Departments.Remove(value);
        }

        private void addCompany(string value)
        {
            if (!value.Equals(string.Empty))
            {
                Company.Add(value);
                companyToAdd = string.Empty;
            }            
        }
        private void removeCompany(string value)
        {
            Company.Remove(value);
        }
        private void manageCompany(bool value)
        {
            manageComp = value;
        }

        private void manageDepartment(bool value)
        {
            manageDep = value;
        }

        private void addJobBeforePost()
        {
            
        }

        private void saveJobState()
        {
            jobList.Add(new()
            {
                Title = TempJob.Title,
                Description = TempJob.Description,
                Company = TempJob.Company,
                Compensation = TempJob.Compensation,
                MinimumExperience = TempJob.MinimumExperience,
                Location = TempJob.Location,
                Department = TempJob.Department,
                Policy = TempJob.Policy,
                Status = "Draft",
                DueDate = TempJob.DueDate,
                CreationDate = DateTime.Today,
                JobType = TempJob.JobType,
            });
            // When the user click save, the status of the Job should be "draft", that will be change when they submit the final job
            // The endpoint need to accept Policy attributes, it's already changed on the client side, just back end

            //https://xebecapi.azurewebsites.net/api/Job the endpoint to save the info

            // This will be a post, buuut if the info already exists it needs to be a PUT request
            // Creation date =  DateTime.Today
            // if you save it should conert the info to a list and the write that list to the DB

            
            foreach (var item in jobList)
            {
                //    if (newPersonalInfo)
                //    {
                    //await HttpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/Job", item, new AuthenticationHeaderValue("Bearer", token));
            //    }
            //    else
            //    {
            //        await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/PersonalInformation/{item.Id}", item, new AuthenticationHeaderValue("Bearer", token));
            //    }

            }

        }

        private void RedirectToNextPage()
        {

        }

    }
}
