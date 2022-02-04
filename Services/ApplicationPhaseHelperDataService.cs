using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using XebecPortal.UI.Interfaces;
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
        public List<ApplicationPhaseHelper> GetApplicationPhaseHelpersByUserId(int appUserId)
        {
            Console.WriteLine($">>>>>ApplicationPhaseHelperDataService : getting helper for {appUserId}");
             return Task.FromResult((AltClient.GetFromJsonAsync<List<ApplicationPhaseHelper>>(
                 $"https://xebecapi.azurewebsites.net/api/ApplicationPhaseHelper/userId={appUserId}"))).Result.Result;
        }

        public List<ApplicationPhaseHelper> GetAssApplicationPhaseHelpers(Applicant applicant, List<Applicant> applicants)
        {
            //Get all associated helpers
            var mockHelpers =  AMockDataHub.GetAssApplicationPhaseHelpers(applicant, applicants);
            
            //Filter helpers
            mockHelpers = GetLatestHelpers(mockHelpers);
            return mockHelpers;
        }

        public async Task<List<ApplicationPhaseHelper>> UpdatePhaseHelper(ApplicationPhaseHelper applicationPhaseHelper)
        {
            //All helpers
            var mockHelpers = AMockDataHub._mockPhaseHelpers;
            return mockHelpers;
        }

        public List<ApplicationPhaseHelper> GetLatestHelpers(List<ApplicationPhaseHelper> unFilteredHelpers)
        {
            //applicationModel - latest ApplicationPhaseHelper
            Dictionary<ApplicationModel, ApplicationPhaseHelper>
                dict = new Dictionary<ApplicationModel, ApplicationPhaseHelper>();
            foreach(var currentHelper in unFilteredHelpers)
            {
                var tempApplication = currentHelper.ApplicationModel;
                if (!dict.ContainsKey(tempApplication))
                {
                    dict.Add(tempApplication, currentHelper);
                }
                else
                {
                    var prevHelper = dict[tempApplication];
                    var lastDate = prevHelper.TimeMoved;
                    if (lastDate < currentHelper.TimeMoved)
                    {
                        dict[tempApplication] = currentHelper;
                    }
                }
            }

            return dict.Values.OrderByDescending(a => a.TimeMoved).ToList();
        }

        public ApplicationPhaseHelper GetEmptyHelper()
        {
            var application = new ApplicationModel {Id = 0, JobId = 0, Job = new JobModel {Id = 0, Title = null, Description = null, Company = null, Compensation = 0, MinimumExperience = 0, Location = null, Department = null, DueDate = default, CreationDate = default, JobTypes = null, JobPlatforms = null, JobPhases = null, Applications = null}, AppUserId = 0, AppUser = null, TimeApplied = default, BeginApplication = default};
            
           
            var applicationPhaseHelper = new ApplicationPhaseHelper
            {
                Id = -1,
                ApplicationId = 0,
                ApplicationModel = application,
                ApplicationPhaseId = 0,
                ApplicationPhase = new AppPhase {Id = 1, Description = "Db error", PhaseEnum = PhaseEnum.Error},
                StatusId = 0,
                Status = new(),
                TimeMoved = default,
                Comments = "error",
                Rating = 0,
                AppUserId = 0
            };
            return applicationPhaseHelper;
        }
        public ApplicationPhaseHelper GetLatestHelper(List<ApplicationPhaseHelper> helpers)
        {
            ApplicationPhaseHelper latestHelper = new();
            if (helpers is {Count: > 0 })
            {
                latestHelper = GetLatestHelpers(helpers)[0];
            }
            else
            {
                latestHelper = GetEmptyHelper();
            }
            return latestHelper;
        }
    }
}