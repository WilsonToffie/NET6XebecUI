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
        private List<string> Company = new List<string>() { "Nebula", "Deloitte" };
        private List<string> Locations = new List<string>() { "Remote", "Eastern Cape", "Free State", " Gauteng", "KwaZulu-Natal", "Limpopo", "Mpumalanga", "Northen Cape", "North West", "Western Cape" };
        //private List<string> Statuses = new List<string>() { "Open", "Draft", "Filled", "Closed" };
        private List<string> Policies = new List<string>() { "Remote", "Hybrid", "Onsite" };
        private IList<JobType> jobTypes = new List<JobType>();
        private IList<Department> Departments = new List<Department>();
        private IList<Department> NewDepartments = new List<Department>();
        //private JobType tempJobType = new JobType();
        //private Department tempDepartment = new Department();
        private Department department = new Department();
        private Department departments = new Department(); // this is mainly used for the department display 
        private List<JobPlatform> ListOfPlatforms = new List<JobPlatform>();
        private IList<Job> jobList = new List<Job>();
        private List<JobTypeHelper> jobType = new List<JobTypeHelper>();
        private JobTypeHelper jobtype = new JobTypeHelper();
        private Department displayDepartment = new Department();
        private JobType displayJobType = new JobType();
        private Job checkJob = new Job();
        private IList<Job> checkJobList { get; set; }

        string token;
        private bool manageDep;
        private bool manageComp;
        private bool createNewDep;
        private bool deleteDep;
        private bool createNewComp;
        private bool deleteComp;
        private bool createJobExists;
        private string companyToAdd;
        private string depToDelete;
        private string successMessage;
        private string departmentName;
        private int savedJobId;
        private int existJobId;

        private bool addedNewDep;
        private bool validUpload;
        private bool allowedToRedirect = false;

        public List<FormQuestion> ChosenQuestions { get; set; }

        protected override async Task OnInitializedAsync()
        {            
            token = await localStorage.GetItemAsync<string>("jwt_token");

            jobTypes = await HttpClient.GetListJsonAsync<List<JobType>>($"https://xebecapi.azurewebsites.net/api/jobtype", new AuthenticationHeaderValue("Bearer", token));
            Departments = await HttpClient.GetListJsonAsync<List<Department>>($"https://xebecapi.azurewebsites.net/api/Department", new AuthenticationHeaderValue("Bearer", token));
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
                // The reason for the new Departments is to make posting easier, by adding new departments to a new list and just posting that list
                NewDepartments.Add(new()
                {
                    Name = value.Name,
                });                
                department.Name = string.Empty;
            }
        }

        private async Task removeDepartment(Department value)
        {
            Console.WriteLine("Value received: " + value.Id);
            Departments.Remove(value);
            if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Remove This Department?"))
            {
                var removeDep = await HttpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/Department/{value.Id}", new AuthenticationHeaderValue("Bearer", token));
                if (removeDep.IsSuccessStatusCode)
                {
                    await jsRuntime.InvokeAsync<object>("alert", "Department has successfully been removed!");                   
                }
            }
            
            await OnInitializedAsync();
        }

        private async Task saveDep()
        {
            foreach (var item in NewDepartments)
            {
                var newDepAdded = await HttpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/Department", item, new AuthenticationHeaderValue("Bearer", token));
                if (newDepAdded.IsSuccessStatusCode)
                {
                    addedNewDep = true;
                }
                else
                {
                    addedNewDep = false;
                }
               
            }

            if (addedNewDep)
            {
                await jsRuntime.InvokeAsync<object>("alert", "Newly added departments has been saved!");
            }
            NewDepartments.Clear();
            
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

        private async Task saveJobState()
        {

            jobType.Add(new()
            {
                JobTypeId = jobtype.JobTypeId,
            });
            Console.WriteLine("Joblist count: " + jobList.Count);
            jobList.Add(new()
            {
                Title = TempJob.Title,
                Description = TempJob.Description,
                Company = TempJob.Company,
                Compensation = TempJob.Compensation,
                MinimumExperience = TempJob.MinimumExperience,
                Location = TempJob.Location,
                DepartmentId = departments.Id,
                //Department = departments,
                Policy = TempJob.Policy,
                Status = "Draft",
                DueDate = TempJob.DueDate,
                CreationDate = DateTime.Today,                
                JobTypes = jobType,
            });

            checkJobList = await HttpClient.GetListJsonAsync<List<Job>>($"https://xebecapi.azurewebsites.net/api/Job", new AuthenticationHeaderValue("Bearer", token));
            //checkJob = checkJobList.Where(x => x.Title == TempJob.Title).ToList();
            // Please find a better way....
            foreach (var item in checkJobList)
            {
                if (item.Title.Equals(TempJob.Title) && item.Description.Equals(TempJob.Description) && item.Company.Equals(TempJob.Company) && item.Location.Equals(TempJob.Location) && item.DepartmentId.Equals(departments.Id) && item.Policy.Equals(TempJob.Policy) && item.Status.Equals("Draft")) // && item.DueDate.Equals(TempJob.DueDate) && item.JobTypes.Equals(jobType)
                {                    
                    existJobId = item.Id; 
                    createJobExists = true;
                }
                else
                {
                    createJobExists = false;
                }
            }
            foreach (var item in jobList)
            {
                Console.WriteLine("Title: " + item.Title);
                Console.WriteLine("Description: " + item.Description);
                Console.WriteLine("Company: " + item.Company);
                Console.WriteLine("Location: " + item.Location);
                Console.WriteLine("DepartmentId: " + item.DepartmentId);
                // Console.WriteLine("Department: " + item.Department);
                Console.WriteLine("Policy: " + item.Policy);
                Console.WriteLine("Status: " + item.Status);
                Console.WriteLine("Due Date: " + item.DueDate);
                Console.WriteLine("Creation Date: " + item.CreationDate);
                Console.WriteLine("JobTypes: " + item.JobTypes);

                
                if (createJobExists)
                {
                    Console.WriteLine("Job exists already with ID of: " + existJobId);
                    var validNewUpload = await HttpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Job/{existJobId}", item, new AuthenticationHeaderValue("Bearer", token));
                    if (validNewUpload.IsSuccessStatusCode)
                    {
                        validUpload = true;
                    }
                    else
                    {
                        validUpload = false;
                    }
                    await OnInitializedAsync();
                }
                else
                {
                    Console.WriteLine("Job Doesnt exist yet");
                    var validExistingUpload = await HttpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/Job", item, new AuthenticationHeaderValue("Bearer", token));
                    if (validExistingUpload.IsSuccessStatusCode)
                    {
                        validUpload = true;
                    }
                    else
                    {
                        validUpload = false;
                    }
                    await OnInitializedAsync();
                }

            }

            if (validUpload)
            {
                await jsRuntime.InvokeAsync<object>("alert", "The current state of the job creation process has been saved!");
            }
            allowedToRedirect = true;
            jobList.Clear();
            jobType.Clear();
            checkJobList.Clear();
        }

        private bool redirectpage;
        private async Task RedirectToNextPage(bool value)
        {
            await saveJobState();
            redirectpage = value;
        }

        
        private async Task displayDepName(int depNameID)
        {            
            displayDepartment = await HttpClient.GetListJsonAsync<Department>($"https://xebecapi.azurewebsites.net/api/Department/single/{depNameID}", new AuthenticationHeaderValue("Bearer", token));
            departments.Id = displayDepartment.Id;
            departments.Name = displayDepartment.Name;
        }

        private async Task displayTypeName(int jobTypeId)
        {            
            displayJobType = await HttpClient.GetListJsonAsync<JobType>($"https://xebecapi.azurewebsites.net/api/JobType/{jobTypeId}", new AuthenticationHeaderValue("Bearer", token));
            TempJob.JobType.Id = displayJobType.Id;
            TempJob.JobType.Type = displayJobType.Type;
        }
    }
}
