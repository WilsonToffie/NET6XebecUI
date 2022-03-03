using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Components;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Pages.HR;
using XebecPortal.UI.Services.MockServices;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace XebecPortal.UI.Services.Models
{
    public class MyJobListDataService : IMyJobListDataService
    {
        private readonly HttpClient _httpClient;
        private readonly Faker _faker;

        public MyJobListDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _faker = new Faker(){};
        }
        // [Inject] 
        // private IApplicationPhaseHelperDataService ApplicationPhaseHelperDataService { get; set; }
        // [Inject]
        // private IApplicationDataService ApplicationDataService { get; set; }
        // [Inject]
        // private IJobDataService JobDataService { get; set; }
        
        
        
        
        public List<MyJob> GetAllJobs()
        {
            var mocks = new MockMyJobListDataService();
            return  mocks.GetAllJobs().ToList();
        }

        public async Task<List<MyJob>> GetAllJobsByAppUserId(int appUserId)
        {
            List<MyJob> UserJobs = new List<MyJob>();
            //Get all applications helpers ass. with user
            //-> api/ApplicationPhaseHelper/UserId={appUserId}
            // var applicationPhaseHelpers= (await ApplicationPhaseHelperDataService.GetApplicationPhaseHelpersByUserId(appUserId)).ToList();
            //var applicationPhaseHelpers = await _httpClient.GetFromJsonAsync<IEnumerable<ApplicationPhaseHelper>>($"api/ApplicationPhaseHelper/UserId={1}");

            HttpClient client = new HttpClient();
            var applicationPhaseHelpers = await JsonSerializer.DeserializeAsync<IEnumerable<ApplicationPhaseHelper>>
             (await client.GetStreamAsync($"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/UserId={appUserId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            //Use ApplicationPhaseHelper.ApplicationModel id get user's applications
            //->api/ApplicationModel/{applicationId}
            if (applicationPhaseHelpers != null)
                foreach (var phaseHelper in applicationPhaseHelpers)
                {
                    MyJob jobInfo = new();
                    //Mock data
                    //Todo these info need to be pulled from the database
                    var locations = Enum.GetNames(typeof(MockMyJobListDataService.Locations));
                    var phases = Enum.GetNames(typeof(MockMyJobListDataService.Phases));
                    var statuses = Enum.GetNames(typeof(MockMyJobListDataService.Statuses));
                    jobInfo.Company = _faker.Company.CompanyName();
                    jobInfo.Location = _faker.PickRandom<string>(locations);
                    jobInfo.Phase = _faker.PickRandom<string>(phases);
                    jobInfo.Status = _faker.PickRandom<string>(statuses);


                    //From database
                    // var applicationModel = ApplicationDataService.GetApplicationById(phaseHelper.ApplicationId).Result;
                    // var jobAppliedFor = JobDataService.GetJobDetails(applicationModel.JobId).Result;
                    var application = await client.GetFromJsonAsync<ApplicationModel>($"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/api/applicationModel/{phaseHelper.ApplicationId}");
                    if (application != null)
                    {
                        jobInfo.ApplicationDate = application.BeginApplication;
                        var jobAppliedFor = await client.GetFromJsonAsync<Job>($"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/api/job/{application.JobId}");
                        if (jobAppliedFor != null) jobInfo.Position = jobAppliedFor.Title;
                    }

                    
                    jobInfo.LastMoved = phaseHelper.TimeMoved;

                    UserJobs.Add(jobInfo);
                }


            return UserJobs;
        }
    }
}