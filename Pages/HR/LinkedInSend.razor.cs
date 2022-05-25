using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using XebecPortal.UI.Utils.Handlers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace XebecPortal.UI.Pages.HR
{
    public partial class LinkedInSend
    {
        //[Parameter]
        //public CreateJobPost TempJob { get; set; }

        //[Parameter]
        //public EventCallback<CreateJobPost> TempJobChanged { get; set; }

        //[Parameter]
        //public int jobId { get; set; }

        //
        
        [Parameter]
        public EventCallback<int> jobIdChanged { get; set; }

        private List<JobPlatform> profiles = new();

        private List<JobPlatformHelper> jobPlatformHelper = new List<JobPlatformHelper>();
        private List<Job> changeJobStatus = new List<Job>();
        private Job getJobInfo = new Job();
        private List<JobPlatform> selectedValue= new List<JobPlatform>();
        private List<JobPlatform> displaySelectedValues= new List<JobPlatform>();

        private JobPlatform platform = new JobPlatform();
        private JobPlatform addPlatform = new JobPlatform();

        private List<JobPlatform> recentlyAdded = new List<JobPlatform>();
        // This is for the CheckList
        protected List<string> SelectedIds = new List<string>();
        public string OutPutValue { get; set; }

        private string token;
        private bool validUpdate;

        private bool managejobPlatform;
        private bool createjobPlatform;
        private bool deleteJobPlatform;
        private bool addedNewPlatform;
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("TempjobId :" + TempJob.Id);
            token = await localStorage.GetItemAsync<string>("jwt_token");
            TempJob.Id = 217;
            profiles = await httpClient.GetListJsonAsync<List<JobPlatform>>("https://xebecapi.azurewebsites.net/api/jobplatform", new AuthenticationHeaderValue("Bearer", token));
            getJobInfo = await httpClient.GetListJsonAsync<Job>($"https://xebecapi.azurewebsites.net/api/Job/{TempJob.Id}", new AuthenticationHeaderValue("Bearer", token));
        }

        private async Task Save()
        {
            foreach (var item in selectedValue)
            {
                jobPlatformHelper.Add(new()
                {
                    JobId = TempJob.Id,
                    //Job = getJobInfo,
                    JobPlatformId = item.id,
                });
                Console.WriteLine("JobPlatformID: " + item);
                
            }
            await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/JobPlatformHelper/list", jobPlatformHelper, new AuthenticationHeaderValue("Bearer", token));
            
            changeJobStatus.Add(new()
            {
                Id = TempJob.Id,
                Title = getJobInfo.Title,
                Description = getJobInfo.Description,
                Company = getJobInfo.Company,
                Compensation = getJobInfo.Compensation,
                MinimumExperience = getJobInfo.MinimumExperience,
                Location = getJobInfo.Location,
                DepartmentId = getJobInfo.DepartmentId,
                Department = getJobInfo.Department,
                Policy = getJobInfo.Policy,
                DueDate = getJobInfo.DueDate,
                CreationDate = getJobInfo.CreationDate,
                JobTypes = getJobInfo.JobTypes,
                Status = "Open",
                JobPlatforms = jobPlatformHelper,
            });

            foreach (var item in changeJobStatus)
            {
                item.Id = TempJob.Id;

                var validNewUpload = await httpClient.PutJsonAsync($"https://xebecapi.azurewebsites.net/api/Job/{item.Id}", item, new AuthenticationHeaderValue("Bearer", token));
                if (validNewUpload.IsSuccessStatusCode)
                {
                    validUpdate = true;
                }
                else
                {
                    validUpdate = false;
                }
            }
            if (validUpdate)
            {
                await jsRuntime.InvokeAsync<object>("alert", "The current state of the job creation process has been saved!");
            }
        }
    
        private void CheckboxClicked(JobPlatform platformId, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!selectedValue.Contains(platformId))
                {
                    Console.WriteLine("Plat ID when selecting" + platformId.id);
                    selectedValue.Add(platformId);
                    displaySelectedValues.Add(new()
                    {
                        id = platformId.id,
                        platformName = platformId.platformName,
                    });
                }
            }
            else
            {
                if (selectedValue.Contains(platformId))
                {
                    displaySelectedValues.RemoveAll(x => x.id == platformId.id);
                    selectedValue.Remove(platformId);                    
                }
            }            
        }
        private async Task createPlatform(bool value)
        {
            createjobPlatform = value;
            await OnInitializedAsync();
        }

        private void deletePlatform(bool value)
        {
            deleteJobPlatform = value;
        }

        private void managePlatform(bool value)
        {
            managejobPlatform = value;
        }

        private async Task removePlatform(JobPlatform platform)
        {
            if (platform.id > 0)
            {
                if (await jsRuntime.InvokeAsync<bool>("confirm", "Are You Certain You Want To Remove This Department?"))
                {

                    // Departments.Remove(value);
                    var removePlatform = await httpClient.DeleteJsonAsync($"https://xebecapi.azurewebsites.net/api/JobPlatform/{platform.id}", new AuthenticationHeaderValue("Bearer", token));
                    if (removePlatform.IsSuccessStatusCode)
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

        private void addJobPlatform(JobPlatform platform)
        {
            recentlyAdded.Add(new()
            {
                platformName = platform.platformName,
            });
            addPlatform.platformName = string.Empty;
        }
        private async Task savePlatform()
        {
            if (recentlyAdded.Count > 0)
            {
                foreach (var item in recentlyAdded)
                {
                    var newDepAdded = await httpClient.PostJsonAsync($"https://xebecapi.azurewebsites.net/api/JobPlatform", item, new AuthenticationHeaderValue("Bearer", token));
                    if (newDepAdded.IsSuccessStatusCode)
                    {
                        addedNewPlatform = true;
                    }
                    else
                    {
                        addedNewPlatform = false;
                    }
                }
            }
            else
            {
                await createPlatform(false);
            }

            if (addedNewPlatform)
            {
                await jsRuntime.InvokeAsync<object>("alert", "Recently added Platforms has been saved!");
            }
            recentlyAdded.Clear();
            //await OnInitializedAsync();
            await createPlatform(false);
        }
    }
}
