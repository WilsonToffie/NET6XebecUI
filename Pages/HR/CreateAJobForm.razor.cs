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
        //[Parameter]
        //public CreateJobPost TempJob { get; set; }

        //[Parameter]
        //public EventCallback<CreateJobPost> TempJobChanged { get; set; }
        private List<Company> Company = new List<Company>();
        private List<Company> NewCompanies = new List<Company>();
        private List<Location> Locations = new List<Location>();
        //private List<string> Statuses = new List<string>() { "Open", "Draft", "Filled", "Closed" };
        private List<Policy> Policies = new List<Policy>();
        private IList<JobType> jobTypes = new List<JobType>();
        private IList<Department> Departments = new List<Department>();
        private IList<Department> NewDepartments = new List<Department>();
        //private JobType tempJobType = new JobType();
        //private Department tempDepartment = new Department();
        private Department department = new Department();
        private Company company = new Company();
        private Location location = new Location();
        private Policy policy = new Policy();
        private Department departments = new Department(); // this is mainly used for the department display 
        private Company companies = new Company(); // this is mainly used for the Company display 
        private Location locations = new Location(); // this is mainly used for the Location display 
        private Policy policies = new Policy(); // this is mainly used for the Policy display 
        private List<JobPlatform> ListOfPlatforms = new List<JobPlatform>();
        private IList<Job> jobList = new List<Job>();
        private List<JobTypeHelper> jobType = new List<JobTypeHelper>();
        private JobTypeHelper jobtype = new JobTypeHelper();
        private Department displayDepartment = new Department();
        private JobType displayJobType = new JobType();
        private Company displayCompany= new Company();
        private Location displayLocations = new Location();
        private Policy displayPolicy = new Policy();
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
        private bool addedNewComp;
        private bool validUpload;
        private bool allowedToRedirect = false;

        public List<FormQuestion> ChosenQuestions { get; set; }

        protected override async Task OnInitializedAsync()
        {            
            token = await localStorage.GetItemAsync<string>("jwt_token");

            Company = await HttpClient.GetListJsonAsync<List<Company>>($"https://xebecapi.azurewebsites.net/api/Company", new AuthenticationHeaderValue("Bearer", token));
            Locations = await HttpClient.GetListJsonAsync<List<Location>>($"https://xebecapi.azurewebsites.net/api/Location", new AuthenticationHeaderValue("Bearer", token));
            Policies = await HttpClient.GetListJsonAsync<List<Policy>>($"https://xebecapi.azurewebsites.net/api/Policy", new AuthenticationHeaderValue("Bearer", token));

            jobTypes = await HttpClient.GetListJsonAsync<List<JobType>>($"https://xebecapi.azurewebsites.net/api/jobtype", new AuthenticationHeaderValue("Bearer", token));
            Departments = await HttpClient.GetListJsonAsync<List<Department>>($"https://xebecapi.azurewebsites.net/api/Department", new AuthenticationHeaderValue("Bearer", token));
            TempJob.DueDate = TempJob.CreationDate = DateTime.Now;
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
            if (value.Id > 0)
            {
                if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Remove This Department?"))
                {
                
                   // Departments.Remove(value);
                    var removeDep = await HttpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/Department/{value.Id}", new AuthenticationHeaderValue("Bearer", token));
                    if (removeDep.IsSuccessStatusCode)
                    {
                        await jsRuntime.InvokeAsync<object>("alert", "Department has successfully been removed!");
                    }               
                }
            }
            else
            {
                await jsRuntime.InvokeAsync<object>("alert", "Please select a valid Department!");
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
        private void addCompany(Company value)
        {
            if (!value.Equals(string.Empty))
            {
                NewCompanies.Add(new() { 
                    Name = value.Name
                });
                company.Name = string.Empty;
            }
        }
        private async Task removeCompany(Company value)
        {
            if (value.Id > 0)
            {
                if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Remove This Company?"))
                {

                    // Departments.Remove(value);
                    var removeComp = await HttpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/Company/{value.Id}", new AuthenticationHeaderValue("Bearer", token));
                    if (removeComp.IsSuccessStatusCode)
                    {
                        await jsRuntime.InvokeAsync<object>("alert", "Department has successfully been removed!");
                    }
                }
            }
            else
            {
                await jsRuntime.InvokeAsync<object>("alert", "Please select a valid Company!");
            }
            await OnInitializedAsync();
        }

        private async Task saveCompany()
        {
            foreach (var item in NewCompanies)
            {
                var newCompAdded = await HttpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/Company", item, new AuthenticationHeaderValue("Bearer", token));
                if (newCompAdded.IsSuccessStatusCode)
                {
                    addedNewComp = true;
                }
                else
                {
                    addedNewComp = false;
                }

            }

            if (addedNewComp)
            {
                await jsRuntime.InvokeAsync<object>("alert", "Newly added departments has been saved!");
            }
            NewCompanies.Clear();

            await createDep(false);
        }


        private void manageCompany(bool value)
        {
            manageComp = value;
        }

        private void manageDepartment(bool value)
        {
            manageDep = value;
        }

        private async Task saveJobState()
        {
            Console.WriteLine("policies.Id When entering saveJobState" + policies.Id);
            jobType.Add(new()
            {
                JobTypeId = jobtype.JobTypeId,
            });
            Console.WriteLine("Joblist count: " + jobList.Count);

            jobList.Add(new()
            {
                Title = TempJob.Title,
                Description = TempJob.Description,
                CompanyId = companies.Id,
                Compensation = TempJob.Compensation,
                MinimumExperience = TempJob.MinimumExperience,
                LocationId = locations.Id,
                DepartmentId = departments.Id,
                PolicyId = policies.Id,
                Status = "Draft",
                DueDate = TempJob.DueDate,
                CreationDate = DateTime.Today,                
                JobTypes = jobType,                
        });

            Console.WriteLine("Title " + TempJob.Title);
            Console.WriteLine("Description " + TempJob.Description);
            Console.WriteLine("LocationId " + locations.Id);
            Console.WriteLine("DepartmentID " + departments.Id);
            Console.WriteLine("Policies ID " + policies.Id);
            Console.WriteLine("JobTypes " + jobType);

            checkJobList = await HttpClient.GetListJsonAsync<List<Job>>($"https://xebecapi.azurewebsites.net/api/Job", new AuthenticationHeaderValue("Bearer", token));
            //checkJob = checkJobList.Where(x => x.Title == TempJob.Title).ToList();
            // Please find a better way....
            Console.WriteLine("JOb count" + checkJobList.Count());
            foreach (var item in checkJobList)
            {                

                if (item.Title.Equals(TempJob.Title) && item.Description.Equals(TempJob.Description) && (item.DepartmentId == departments.Id) && (item.LocationId == locations.Id)) //) && item.PolicyId == policies.Id item.PolicyId == policies.Id && item.DueDate.Equals(TempJob.DueDate) && item.JobTypes.Equals(jobType)
                {
                    TempJob.Id = item.Id;
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
                if (createJobExists)
                {
                    Console.WriteLine("Job exists already with ID of: " + existJobId);
                    item.Id = existJobId;
                    var validNewUpload = await HttpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Job/{existJobId}", item, new AuthenticationHeaderValue("Bearer", token));
                    //var validNewUpload = await HttpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Job/213", item, new AuthenticationHeaderValue("Bearer", token));
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
                    //var validExistingUpload = await HttpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Job/213", item, new AuthenticationHeaderValue("Bearer", token));
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
            jobType.Clear();
            jobList.Clear();            
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
            if (depNameID > 0)
            {
                displayDepartment = await HttpClient.GetListJsonAsync<Department>($"https://xebecapi.azurewebsites.net/api/Department/single/{depNameID}", new AuthenticationHeaderValue("Bearer", token));
                departments.Id = displayDepartment.Id;
                departments.Name = displayDepartment.Name;
            }            
        }

        private async Task displayTypeName(int jobTypeId)
        {
            if (jobTypeId > 0)
            {
                displayJobType = await HttpClient.GetListJsonAsync<JobType>($"https://xebecapi.azurewebsites.net/api/JobType/{jobTypeId}", new AuthenticationHeaderValue("Bearer", token));
                TempJob.JobType.Id = displayJobType.Id;
                TempJob.JobType.Type = displayJobType.Type;
            }            
        }

        private async Task displayCompName(int compID)
        {
            if (compID > 0)
            {
                displayCompany = await HttpClient.GetListJsonAsync<Company>($"https://xebecapi.azurewebsites.net/api/Company/single/{compID}", new AuthenticationHeaderValue("Bearer", token));
                TempJob.Company.Id = displayCompany.Id;
                TempJob.Company.Name = displayCompany.Name;
            }
        }

        private async Task displayPolicyName(int policyID)
        {
            if (policyID > 0)
            {
                Console.WriteLine("Policy ID when selected: " + policyID);
                displayPolicy = await HttpClient.GetListJsonAsync<Policy>($"https://xebecapi.azurewebsites.net/api/Policy/single/{policyID}", new AuthenticationHeaderValue("Bearer", token));
                TempJob.Policy.Id = displayPolicy.Id;
                TempJob.Policy.Name = displayPolicy.Name;
            }
        }

        private async Task displayLocationName(int locationId)
        {
            if (locationId > 0)
            {
                displayLocations = await HttpClient.GetListJsonAsync<Location>($"https://xebecapi.azurewebsites.net/api/Location/single/{locationId}", new AuthenticationHeaderValue("Bearer", token));
                TempJob.Location.Id = displayLocations.Id;
                TempJob.Location.Name = displayLocations.Name;
            }
        }
    }
}
