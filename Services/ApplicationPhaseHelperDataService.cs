using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using XebecPortal.UI.Interfaces;
using XebecPortal.UI.Service_Interfaces;
using XebecPortal.UI.Services.MockServices;
using XebecPortal.UI.Services.Models;

namespace XebecPortal.UI.Services
{
    public class ApplicationPhaseHelperDataService : IApplicationPhaseHelperDataService
    {
        private readonly HttpClient _httpClient;
        private HttpClient AltClient = new HttpClient();
        public ApplicationPhaseHelperDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        

        public async Task<List<ApplicationPhaseHelper>> GetAllApplicationPhaseHelpers()
        {
            var phaseHelpers = await JsonSerializer.DeserializeAsync<List<ApplicationPhaseHelper>>(
                utf8Json: await AltClient.GetStreamAsync($"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper"),
                options: new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            return phaseHelpers;
        }
        //api/ApplicationPhaseHelper/UserId={AppUserId}
        public Task<List<ApplicationPhaseHelper>> GetApplicationPhaseHelpersByUserId(int appUserId)
        {
            //Todo populate application, application.job, applicationPhase, status
            Console.WriteLine($">>>>>ApplicationPhaseHelperDataService : getting helper for {appUserId}");
             return (AltClient.GetFromJsonAsync<List<ApplicationPhaseHelper>>(
                 $"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/userId={appUserId}"));
        }

        public async Task<List<ApplicationPhaseHelper>> GetAssApplicationPhaseHelpers(Applicant applicant,
            List<Applicant> applicants)
        {
            List<ApplicationPhaseHelper> helpers = (await GetApplicationPhaseHelpersByUserId(applicant.Id));
            if(helpers == null)
                Console.WriteLine($"xxxxxxxxxxxx no helpers for {applicant.Id} {applicant.FirstName}");
            return helpers;
            // return AMockDataHub.GetAssApplicationPhaseHelpers(applicant, applicants);
        }

        public async Task UpdateApplicationPhaseHelper(ApplicationPhaseHelper applicationPhaseHelper)
        {
            // var applicationHelperJson = new StringContent(JsonSerializer.Serialize(applicationPhaseHelper), Encoding.UTF8,"application/json");
            // await AltClient.PutAsync($"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/{applicationPhaseHelper.Id}",applicationHelperJson);
            await AltClient.PutAsJsonAsync($"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/{applicationPhaseHelper.Id}", applicationPhaseHelper);
        }

        public async Task<ApplicationPhaseHelper> GetApplicationPhaseHelperByUserId(int appUserId)
        {
            var helpers = (await AltClient.GetFromJsonAsync<List<ApplicationPhaseHelper>>(
                $"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/userId={appUserId}"));
            if (helpers.Count > 0)
            {
                return helpers[0];
            }

            var job = new JobModel
            {
                Id = -1,
                Title = "Db Error",
            };

            var application = new ApplicationModel
            {
                JobId = -1,
                Job = job,
            };
            return new ApplicationPhaseHelper
            {
                Id = -1,
                ApplicationId = 0,
                Application = application,
                ApplicationPhaseId = -1,
                ApplicationPhase = null,
            };
        }

        public ApplicationPhaseHelper GetEmptyHelper()
        {
            var applicationPhaseHelper = new ApplicationPhaseHelper
            {
                Id = -1,
                ApplicationId = 0,
                Application = new ApplicationModel
                {
                    Id = 0,
                    JobId = 0,
                    Job = new JobModel
                    {
                        Id = 0,
                        Title = null,
                        Description = null,
                        Company = null,
                        Compensation = 0,
                        MinimumExperience = 0,
                        Location = null,
                        Department = null,
                        DueDate = default,
                        CreationDate = default,
                        JobTypes = null,
                        JobPlatforms = null,
                        JobPhases = null,
                        Applications = null
                    },
                    AppUserId = 0,
                    AppUser = null,
                    TimeApplied = default,
                    BeginApplication = default
                },
                ApplicationPhaseId = 0,
                ApplicationPhase = new AppPhase {Id = 1, Description = "Db error",},
                StatusId = 0,
                Status = new(),
                TimeMoved = default,
                Comments = "error",
                Rating = 0,
                AppUserId = 0
            };
            return applicationPhaseHelper;
        }
    }
}