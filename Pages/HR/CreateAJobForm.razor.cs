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
        public Job job = new();
        [Parameter]
        public CreateJobPost TempJob { get; set; }

        [Parameter]
        public EventCallback<CreateJobPost> TempJobChanged { get; set; }
        private List<AppUser> collaborators = new List<AppUser>();
        private List<AppUser> collaboratorsAdded = new List<AppUser>();
        private List<string> Company = new List<string>() { "Nebula", "Deloitte" };
        private List<string> Locations = new List<string>() { "Remote", "Eastern Cape", "Free State", " Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northen Cape", "North West", "Western Cape" };
        private List<string> Statuses = new List<string>() { "Open", "Draft", "Filled", "Closed" };
        private List<string> Policies = new List<string>() { "Remote", "Hybrid", "Onsite" };
        private IList<JobType> jobTypes = new List<JobType>();
        private IList<JobPlatform> jobPlatforms = new List<JobPlatform>();
        private IList<Department> Departments = new List<Department>();
        private IList<Department> NewDepartments = new List<Department>();
        private JobType tempJobType = new JobType();
        private Department department = new Department();
        private List<JobPlatform> ListOfPlatforms = new List<JobPlatform>();
        private List<CreateJobPost> jobList = new List<CreateJobPost>();        


        string token;
        private bool manageDep;
        private bool manageComp;
        private bool createNewDep;
        private bool deleteDep;
        private bool createNewComp;
        private bool deleteComp;
        private string companyToAdd;
        private string depToDelete;
        private string successMessage;

        private string departmentName;
        protected override async Task OnInitializedAsync()
        {            
            token = await localStorage.GetItemAsync<string>("jwt_token");

            jobPlatforms = await HttpClient.GetListJsonAsync<List<JobPlatform>>("https://xebecapi.azurewebsites.net/api/jobplatform", new AuthenticationHeaderValue("Bearer", token));
            jobTypes = await HttpClient.GetListJsonAsync<List<JobType>>("https://xebecapi.azurewebsites.net/api/jobtype", new AuthenticationHeaderValue("Bearer", token));
            collaborators = await HttpClient.GetListJsonAsync<List<AppUser>>("https://xebecapi.azurewebsites.net/api/user", new AuthenticationHeaderValue("Bearer", token));
            Departments = await HttpClient.GetListJsonAsync<List<Department>>("https://xebecapi.azurewebsites.net/api/Department", new AuthenticationHeaderValue("Bearer", token));

            job.DueDate = TempJob.CreationDate = DateTime.Today;
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

        private async Task createDep(bool value)
        {
            createNewDep = value;
            successMessage = string.Empty;
            await OnInitializedAsync();
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
        private void addDepartment(Department value)
        {
            if (!value.Equals(string.Empty))
            {
                //Departments.Add(value);
                // department.name = string.Empty;
                //Departments.Add(new()
                //{
                //    Name = value.Name
                //});
                // The reason for the new Departments is to make posting easier, by adding new departments to a new list and just posting that list
                NewDepartments.Add(new()
                {
                    Name = value.Name
                });
                department.Name = string.Empty;
                successMessage = "Department has successfully been added!";
            }
        }

        private async Task removeDepartment(Department value)
        {
            Console.WriteLine("Value received: " + value.Id);
            await HttpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/Department/{value.Id}", new AuthenticationHeaderValue("Bearer", token));
            await OnInitializedAsync();
        }

        private async Task saveDep()
        {
            foreach (var item in NewDepartments)
            {
                await HttpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/Department", item, new AuthenticationHeaderValue("Bearer", token));
            }
            await createDep(false);
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

        private IList<Department> displayDepartment = new List<Department>();
        private async Task OnDepartmentChanged(ChangeEventArgs e)
        {
            Console.WriteLine("Value: " + e.Value.ToString());
            displayDepartment = await HttpClient.GetListJsonAsync<List<Department>>("https://xebecapi.azurewebsites.net/api/Department/single/", new AuthenticationHeaderValue("Bearer", token));
            
        //return ValueChanged.InvokeAsync();
        //Console.WriteLine("Departments value: " + TempJob.Department);
        }
    }
}
